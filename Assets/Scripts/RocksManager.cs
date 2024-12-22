using System;
using Data;
using UnityEngine;

public class RocksManager: MonoBehaviour
{
    public Action<int> OnScoreChanged;
    private RockSpawner rockSpawner;
    private LevelData levelData;

    public void SetUp(LevelData data)
    {
        levelData = data;
        if (!TryGetComponent(out rockSpawner))
        {
            rockSpawner = gameObject.AddComponent<RockSpawner>();
        }
        
        CreateRocks();
    }
    
    private void CreateRocks()
    {
        rockSpawner.SetUp(levelData.startingRockData.prefab, 5, 50);
        rockSpawner.SpawnFirstRocks(levelData.startingRocksToSpawn, levelData.startingRockData);
        rockSpawner.OnRockDestroyed += OnRockDestroyed;
    }

    private void OnRockDestroyed(int addedScore)
    {
        OnScoreChanged?.Invoke(addedScore);
    }
}