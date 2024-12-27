using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : EntitySpawner<Rock>
{
    public Action<Rock> RockDestroyed;

    public void SpawnFirstRocks(int rocksToSpawn, RockData rockData)
    {
        for (var i = 0; i < rocksToSpawn; i++)
        {
            var randomPosition = CreateRandomRockPosition(Random.Range(0, 4));
            var randomRotation = Random.Range(0f, 360f);

            SpawnRock(randomPosition, Quaternion.Euler(0, 0, randomRotation), rockData);
        }
    }

    /// <summary>
    ///     Randomly pick between top, bottom, left or right border.
    ///     The game being played in landscape, top and bottom should be weighted against left and right.
    /// </summary>
    /// <param name="border">0 = top, 1 = bottom, 2 = left, 3 = right</param>
    /// <returns></returns>
    private static Vector3 CreateRandomRockPosition(int border)
    {
        Vector3 randomPosition;
        if (border is 0 or 1)
        {
            // Horizontal border (top or bottom)
            var randomX = Random.Range(ScreenManager.WorldMinCorner.x, ScreenManager.WorldMaxCorner.x);
            var randomY = border == 1 ? ScreenManager.WorldMinCorner.y : ScreenManager.WorldMaxCorner.y;
            randomPosition = new Vector3(randomX, randomY, 0);
        }
        else
        {
            // Vertical border (left or right)
            var randomY = Random.Range(ScreenManager.WorldMinCorner.y, ScreenManager.WorldMaxCorner.y);
            var randomX = border == 2 ? ScreenManager.WorldMinCorner.x : ScreenManager.WorldMaxCorner.x;
            randomPosition = new Vector3(randomX, randomY, 0);
        }

        return randomPosition;
    }

    private void SpawnRock(Vector3 position, Quaternion rotation, RockData data)
    {
        var rock = Pool.GetObject(position, rotation);
        rock.Destroyed += OnRockDestroyed;
        rock.Released += OnRockReleased;
        rock.SetUp(data);
    }

    private void OnRockReleased(Rock rock)
    {
        ReleaseRock(rock);
    }

    private void OnRockDestroyed(Rock rock)
    {
        ReleaseRock(rock);
        RockDestroyed?.Invoke(rock);
    }

    private void ReleaseRock(Rock rock)
    {
        rock.Destroyed -= OnRockDestroyed;
        rock.Released -= OnRockReleased;
        Pool.ReleaseGameObject(rock);
    }

    public void SpawnChildRocks(Rock parentRock)
    {
        var childRockData = parentRock.Data.SpawnedRock;
        // In 2D space, z of position is always 0
        var parentPosition = parentRock.transform.position;
        // In 2D space, x and y of rotation is always 0
        var parentRotation = parentRock.transform.rotation;

        var angleDeviation = parentRock.Data.MaxSpawnAngleDeviation;

        for (var i = 0; i < parentRock.Data.SpawnedRocksAmount; i++)
        {
            var minAngleDeviation = parentRotation.z + parentRock.Data.MinSpawnAngleDeviation;
            var childNewAngle = Random.Range(minAngleDeviation - angleDeviation, minAngleDeviation + angleDeviation);
            var childRotation = parentRotation * Quaternion.Euler(0, 0, childNewAngle);

            var velocityMultiplier = Random.Range(parentRock.Data.MinVelocityMultiplier, parentRock.Data.MaxVelocityMultiplier);
            childRockData.LaunchVelocity = parentRock.Data.LaunchVelocity * velocityMultiplier;

            SpawnRock(parentPosition, childRotation, childRockData);
        }
    }
}