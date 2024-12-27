using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    public Action<int> OnScoreChanged;

    private BigEnemy bigEnemy;
    private SmallEnemy smallEnemy;
    bool enemyAlive;
    
    public void SetUp(PlayerManager playerManager, EnemyData bigEnemyData, EnemyData smallEnemyData)
    {
        var big = SetUpEnemy(bigEnemyData);
        Assert.IsTrue(big is BigEnemy, "The prefab set up in the Big Enemy Data must have a BigEnemy component attached to it!");
        bigEnemy = big as BigEnemy;

        var small = SetUpEnemy(smallEnemyData);
        Assert.IsTrue(small is SmallEnemy, "The prefab set up in the Small Enemy Data must have a SmallEnemy component attached to it!");
        smallEnemy = small as SmallEnemy;
        smallEnemy.SetPlayerManager(playerManager);
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

    private void OnEnemyReachedEndOfScreen(Enemy enemy)
    {
        RenewEnemy(enemy);
    }
    
    private void OnEnemyDestroyed(Enemy enemy)
    {
        OnScoreChanged?.Invoke(enemy.Data.Score);
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

    public void ResetFromStart()
    {
        StopAllCoroutines();
        DeactivateEnemy(bigEnemy);
        DeactivateEnemy(smallEnemy);
        StartEnemySpawn();
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