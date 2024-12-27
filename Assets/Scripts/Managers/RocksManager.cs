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
        if (levelData.StartingRockData == null) return;
        GetOrCreateNewRockSpawner(levelData.StartingRockData).SpawnFirstRocks(levelData.StartingRocksToSpawn, levelData.StartingRockData);
    }

    public void ResetFromRestart()
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
        if (!rock.DestroyedByEnemy && rock.Data.Collectable) // TODO not the best logic for this new feature
        {
            OnRockCollected?.Invoke(rock);
        }
        else
        {
            if (rock.Data.SpawnedRock != null)
            {
                GetOrCreateNewRockSpawner(rock.Data.SpawnedRock).SpawnChildRocks(rock);
            }

            if (!rock.DestroyedByEnemy)
            {
                OnScoreChanged?.Invoke(rock.Data.Score);
            }
        }
    }

    private RockSpawner GetOrCreateNewRockSpawner(RockData rockToSpawn)
    {
        var rockToSpawnId = rockToSpawn.GetInstanceID();
        if (!spawnerByRockDataId.TryGetValue(rockToSpawnId, out var rockSpawner))
        {
            rockSpawner = AddRockSpawner();
            spawnerByRockDataId[rockToSpawnId] = rockSpawner;
            SetUpSpawner(rockToSpawn.Prefab, rockSpawner);
        }

        return rockSpawner;
    }

    private void SetUpSpawner(Rock rockToSpawn, RockSpawner rockSpawner)
    {
        rockSpawner.SetUp(rockToSpawn, 5, 50);
        rockSpawner.RockDestroyed += OnRockDestroyed;
    }
}