using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] private RockData startingRockData;
    public RockData StartingRockData
    {
        get
        {
            Assert.IsNotNull(startingRockData, $"Starting Rock Data is null in {name} data, please assign one.");
            return startingRockData;
        }
    }
    
    [SerializeField] private int startingRocksToSpawn;
    public int StartingRocksToSpawn => startingRocksToSpawn;
}