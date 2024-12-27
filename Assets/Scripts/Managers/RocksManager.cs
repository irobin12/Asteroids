using System;
using System.Collections.Generic;
using UnityEngine;

public class RocksManager : MonoBehaviour
{
    private LevelData levelData;
    public Action<Rock> RockCollected;
    public Action<int> ScoreChanged;
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

    private RockSpawner AddRockSpawner()
    {
        return gameObject.AddComponent<RockSpawner>();
    }

    private void OnRockDestroyed(Rock rock)
    {
        if (!rock.Data.Collectable)
        {
            if (rock.Data.SpawnedRock != null)
            {
                GetOrCreateNewRockSpawner(rock.Data.SpawnedRock).SpawnChildRocks(rock);
            }

            if (!rock.DestroyedByEnemy)
            {
                ScoreChanged?.Invoke(rock.Data.Score);
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
        rockSpawner.RockCollected += RockCollected;
    }

    private void RemoveAllRocks()
    {
        foreach (var spawner in spawnerByRockDataId)
        {
            spawner.Value.ReleaseAll();
        }
    }
}