using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
    public class GameData : ScriptableObject
    {
        public PlayerData player;
        public RockData rock;
        public Enemy enemy;

        public LevelData[] levels;
    }
}