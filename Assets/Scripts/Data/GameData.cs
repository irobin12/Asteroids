using Entities;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
    public class GameData : ScriptableObject
    {
        public PlayerData player;
        public Enemy enemy;

        [Tooltip("The number of lives the player starts the game with.")]
        public int startingHealth;
        [Tooltip("The maximum number of lives the player can accumulate.")]
        public int maxHealth;
        [Tooltip("The additional score required to gain a life.")]
        public int scorePerBonusLife;
        
        public LevelData[] levels;
    }
}