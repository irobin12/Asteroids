using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public RockData startingRockData;
        public int startingRocksToSpawn;
    }
}