using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    public Action<int> ScoreChanged;

    [SerializeField] private ParticleSystem vfxOnDeathPrefab;
    private ParticleSystem vfxOnDeath;
    
    private BigEnemy bigEnemy;
    private SmallEnemy smallEnemy;
    private bool enemyAlive;
    
    public void SetUp(PlayerManager playerManager, EnemyData bigEnemyData, EnemyData smallEnemyData)
    {
        var big = SetUpEnemy(bigEnemyData);
        Assert.IsTrue(big is BigEnemy, "The prefab set up in the Big Enemy Data must have a BigEnemy component attached to it!");
        bigEnemy = big as BigEnemy;

        var small = SetUpEnemy(smallEnemyData);
        Assert.IsTrue(small is SmallEnemy, "The prefab set up in the Small Enemy Data must have a SmallEnemy component attached to it!");
        smallEnemy = small as SmallEnemy;
        smallEnemy.SetPlayerManager(playerManager);
        
        if (vfxOnDeathPrefab)
        {
            // Hacky assumption that there should be only 1 enemy on screen at any time.
            // Really there should be a particle systems spawner/manager to handle them across all entities,
            // But I'm adding this quickly to demonstrate a vertical slice of art polish. 
            vfxOnDeath = Instantiate(vfxOnDeathPrefab); 
        }
    }

    private Enemy SetUpEnemy(EnemyData data)
    {
        var enemy = Instantiate(data.Prefab);
        enemy.ReachedEndOfScreen += OnEnemyReachedEndOfScreen;
        enemy.Destroyed += OnEnemyDestroyed;
        enemy.SetUp(data);
        enemy.SetActive(false);
        return enemy;
    }

    public void SetFromStart()
    {
        StartEnemySpawn();
    }

    private void OnEnemyReachedEndOfScreen(Enemy enemy)
    {
        RenewEnemy(enemy);
    }
    
    private void OnEnemyDestroyed(Enemy enemy)
    {
        if (vfxOnDeath)
        {
            vfxOnDeath.transform.position = enemy.transform.position;
            vfxOnDeath.Play();
        }
        
        ScoreChanged?.Invoke(enemy.Data.Score);
        RenewEnemy(enemy);
    }

    private void RenewEnemy(Enemy enemy)
    {
        DeactivateEnemy(enemy, false);
        StartEnemySpawn();
    }

    private void DeactivateEnemy(Enemy enemy, bool removeProjectiles = true)
    {
        enemy.SetActive(false, removeProjectiles);
        enemyAlive = false;
    }

    private void StartEnemySpawn()
    {
        var enemy = Random.Range(0, 2) == 0 ? bigEnemy as Enemy : smallEnemy;
        enemy.SetFromStart();
        
        var data = enemy.Data;
        var cooldown = Random.Range(data.Cooldown * data.CooldownRandomness, data.Cooldown * (1 + data.CooldownRandomness));
        
        StopAllCoroutines();
        StartCoroutine(WaitToSpawnEnemy(enemy, cooldown));
    }

    private IEnumerator WaitToSpawnEnemy(Enemy enemy, float cooldown)
    {
        while (enemyAlive)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(cooldown);
        enemy.SetActive(true);
        enemyAlive = true;
    }

    public void ResetFromRestart()
    {
        DeactivateEnemy(bigEnemy);
        DeactivateEnemy(smallEnemy);
    }

    private void OnDestroy()
    {
        UnsubscribeEnemy(bigEnemy);
        UnsubscribeEnemy(smallEnemy);
    }

    private void UnsubscribeEnemy(Enemy enemy)
    {
        if(!enemy) return;
        
        enemy.ReachedEndOfScreen -= OnEnemyReachedEndOfScreen;
        enemy.Destroyed -= OnEnemyDestroyed;
    }
}