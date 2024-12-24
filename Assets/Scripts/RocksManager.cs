using System;
using System.Collections.Generic;
using Data;
using Entities;
using UnityEngine;

public class RocksManager: MonoBehaviour
{
    public Action<int> OnScoreChanged;
    public Action<Rock> OnRockCollected;
    private Dictionary<int, RockSpawner> spawnerByRockDataId;
    private LevelData levelData;

    public void SetUp(LevelData data)
    {
        levelData = data;
        spawnerByRockDataId = new Dictionary<int, RockSpawner>();
        
        GetOrCreateNewRockSpawner(data.startingRockData).SpawnFirstRocks(levelData.startingRocksToSpawn, data.startingRockData);
    }

    private RockSpawner AddRockSpawner()
    {
        return gameObject.AddComponent<RockSpawner>();
    }

    private void OnRockDestroyed(Rock rock)
    {
        if (rock.Data.collectable)
        {
            OnRockCollected?.Invoke(rock);
        }
        else
        {
            GetOrCreateNewRockSpawner(rock.Data.spawnedRock).SpawnChildRocks(rock);
            var addedScore = rock.Data.score;
            OnScoreChanged?.Invoke(addedScore);
        }
    }

    private RockSpawner GetOrCreateNewRockSpawner(RockData rockToSpawn)
    {
        var rockToSpawnId = rockToSpawn.GetInstanceID();
        if (!spawnerByRockDataId.TryGetValue(rockToSpawnId, out var rockSpawner))
        {
            rockSpawner = AddRockSpawner();
            spawnerByRockDataId[rockToSpawnId] = rockSpawner;
            SetUpSpawner(rockToSpawn.prefab, rockSpawner);
        }

        return rockSpawner;
    }

    private void SetUpSpawner(Rock rockToSpawn, RockSpawner rockSpawner)
    {
        rockSpawner.SetUp(rockToSpawn, 5, 50);
        rockSpawner.OnRockDestroyed += OnRockDestroyed;
    }
}
