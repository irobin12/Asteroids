using System;
using System.Collections.Generic;
using Data;
using Entities;
using UnityEngine;

public class RocksManager: MonoBehaviour
{
    public Action<int> OnScoreChanged;
    public Action<Rock> OnRockCollected;
    // private RockSpawner[] rockSpawners;
    private Dictionary<int, RockSpawner> spawnerByRockDataId;
    // private RockSpawner rockSpawner;
    private LevelData levelData;

    public void SetUp(LevelData data)
    {
        levelData = data;
        spawnerByRockDataId = new Dictionary<int, RockSpawner>();
        
        // spawnerByRockData.Add(firstRockData, firstRockSpawner);

        // if (!TryGetComponent(out rockSpawner))
        // {
            // rockSpawner = gameObject.AddComponent<RockSpawner>();
        // }
        
        CreateFirstRocks(data.startingRockData, levelData.startingRocksToSpawn, GetOrCreateNewRockSpawner(data.startingRockData));
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
            CreateChildRocks(rock, GetOrCreateNewRockSpawner(rock.Data.spawnedRock));
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
        }

        return rockSpawner;
    }

    private void CreateFirstRocks(RockData rockToSpawn, int rockCount, RockSpawner rockSpawner)
    {
        SetUpSpawner(rockToSpawn.prefab, rockSpawner);
        rockSpawner.SpawnFirstRocks(rockCount, rockToSpawn);
    }
    
    private void CreateChildRocks(Rock parentRock, RockSpawner rockSpawner)
    {
        SetUpSpawner(parentRock.Data.spawnedRock.prefab, rockSpawner);
        rockSpawner.SpawnChildRocks(parentRock);
    }

    private void SetUpSpawner(Rock rockToSpawn, RockSpawner rockSpawner)
    {
        rockSpawner.SetUp(rockToSpawn, 5, 50);
        rockSpawner.OnRockDestroyed += OnRockDestroyed;
    }
}
