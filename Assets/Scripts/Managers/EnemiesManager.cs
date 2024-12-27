using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    public Action<int> OnScoreChanged;

    private Enemy bigEnemy;
    private Enemy smallEnemy;
    bool enemyAlive;
    
    public void SetUp(EnemyData bigEnemyData, EnemyData smallEnemyData)
    {
        bigEnemy = SetUpEnemy(bigEnemyData);
        smallEnemy = SetUpEnemy(smallEnemyData);
    }

    private Enemy SetUpEnemy(EnemyData data)
    {
        var enemy = Instantiate(data.prefab);
        enemy.ReachedEndOfScreen += OnEnemyReachedEndOfScreen;
        enemy.Destroyed += OnEnemyDestroyed;
        enemy.gameObject.SetActive(false);
        enemy.SetUp(data);
        return enemy;
    }

    private void OnEnemyReachedEndOfScreen(Enemy enemy)
    {
        RenewEnemy(enemy);
    }
    
    private void OnEnemyDestroyed(Enemy enemy)
    {
        OnScoreChanged?.Invoke(enemy.Data.score);
        RenewEnemy(enemy);
    }

    private void RenewEnemy(Enemy enemy)
    {
        DeactivateEnemy(enemy);
        StartEnemySpawn();
    }

    private void DeactivateEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemyAlive = false;
    }

    // public void SetFromStart()
    // {
    //     StartEnemySpawn();
    // }

    private void StartEnemySpawn()
    {
        var enemy = Random.Range(0, 2) == 0 ? bigEnemy : smallEnemy;
        enemy.SetSpawnPosition();
        
        var data = enemy.Data;
        var cooldown = Random.Range(data.cooldown * data.cooldownRandomness, data.cooldown * (1 + data.cooldownRandomness));
        
        StartCoroutine(WaitToSpawnEnemy(enemy, cooldown));
    }
    

    private IEnumerator WaitToSpawnEnemy(Enemy enemy, float cooldown)
    {
        while (enemyAlive)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(cooldown);
        enemy.gameObject.SetActive(true);
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
        enemy.ReachedEndOfScreen -= OnEnemyReachedEndOfScreen;
        enemy.Destroyed -= OnEnemyDestroyed;
    }
}