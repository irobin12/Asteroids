using System;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : MonoBehaviour
{
    public Action<int> OnRockDestroyed;
    
    [SerializeField] private Projectile projectilePrefab;
    
    private RockData data;
    private MovingEntityPool pool;
    // private int rocksToSpawn;

    public void SetUp(RockData rockData)
    {
        data = rockData;
        pool = new MovingEntityPool(data, 5, 50);
    }

    public void SpawnFirstRocks(int rocksToSpawn)
    {
        for (int i = 0; i < rocksToSpawn; i++)
        {
            // Randomly pick between top, bottom, left or right border.
            // The game being played in landscape, top and bottom should be weighted against left and right.
            int randomBorder = Random.Range(0, 4);
            
            Vector3 randomPosition;                                                                                                                                       
            
            if (randomBorder is 0 or 1)
            {
                // Horizontal border (top or bottom)
                var randomX = Random.Range(ScreenManager.WorldMinCorner.x, ScreenManager.WorldMaxCorner.x);
                var randomY = randomBorder == 0 ? ScreenManager.WorldMinCorner.y : ScreenManager.WorldMaxCorner.y;
                randomPosition = new Vector3(randomX, randomY, 0);
            }
            else
            {
                // Vertical border (left or right)
                var randomY = Random.Range(ScreenManager.WorldMinCorner.y, ScreenManager.WorldMaxCorner.y);
                var randomX = randomBorder == 2 ? ScreenManager.WorldMinCorner.x : ScreenManager.WorldMaxCorner.x;
                randomPosition = new Vector3(randomX, randomY, 0);
            }
            
            var randomRotation = Random.Range(0f, 360f);
            
            SpawnRock(randomPosition, Quaternion.Euler(0, 0, randomRotation));
        }
    }
    
    private void SpawnRock(Vector3 position, Quaternion rotation)
    {
        var rock = pool.GetEntity(position, rotation).GetComponent<Rock>();
        rock.Death += ReleaseRock;
        rock.SetUp(data,true);
    }

    private void ReleaseRock(MovingEntity entity)
    {
        if (entity is Rock rock)
        {
            OnRockDestroyed?.Invoke(rock.Data.score);
        }
        entity.Death -= ReleaseRock;
        pool.ReleaseEntity(entity);
    }

}