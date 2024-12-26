using System;
using System.Collections.Generic;
using UnityEngine;

public class RocksManager : MonoBehaviour
{
    private LevelData levelData;
    public Action<Rock> OnRockCollected;
    public Action<int> OnScoreChanged;
    private Dictionary<int, RockSpawner> spawnerByRockDataId;

    public void SetUp(LevelData data)
    {
        levelData = data;
        spawnerByRockDataId = new Dictionary<int, RockSpawner>();
    }

    private void CreateFirstRocks()
    {
        if (levelData.startingRockData == null) return;
        GetOrCreateNewRockSpawner(levelData.startingRockData)
            .SpawnFirstRocks(levelData.startingRocksToSpawn, levelData.startingRockData);
    }

    public void ResetFromStart()
    {
        RemoveAllRocks();
        SetFromStart();
    }

    public void SetFromStart()
    {
        CreateFirstRocks();
    }

    private void RemoveAllRocks()
    {
        foreach (var spawner in spawnerByRockDataId) spawner.Value.ReleaseAll();
    }

    private RockSpawner AddRockSpawner()
    {
        return gameObject.AddComponent<RockSpawner>();
    }

    private void OnRockDestroyed(Rock rock)
    {
        if (rock.Data.collectable) // TODO not the best logic for this new feature
        {
            OnRockCollected?.Invoke(rock);
        }
        else
        {
            if (rock.Data.spawnedRock != null) GetOrCreateNewRockSpawner(rock.Data.spawnedRock).SpawnChildRocks(rock);
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
        rockSpawner.RockDestroyed += OnRockDestroyed;
    }
}