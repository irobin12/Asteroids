using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField] private InputData input;
    public InputData Input 
    {
        get
        {
            Assert.IsNotNull(input, $"Input is null in {name} data, please assign one.");
            return input;
        }
    }
    
    [SerializeField] private PlayerData player;
    public PlayerData Player
    {
        get
        {
            Assert.IsNotNull(player, $"Player is null in {name} data, please assign one.");
            return player;
        }
    }
    
    [SerializeField] private EnemyData bigEnemy;
    public EnemyData BigEnemy
    {
        get
        {
            Assert.IsNotNull(bigEnemy, $"Big Enemy is null in {name} data, please assign one.");
            return bigEnemy;
        }
    }
    
    [SerializeField] private EnemyData smallEnemy;
    public EnemyData SmallEnemy
    {
        get
        {
            Assert.IsNotNull(smallEnemy, $"Small Enemy is null in {name} data, please assign one.");
            return smallEnemy;
        }
    }
    
    [Tooltip("How much collectables the player needs to gather to win the game.")]
    [Min(1)][SerializeField] private int winningCollectibleCount = 6;
    public int WinningCollectibleCount => winningCollectibleCount;

    [Tooltip("The number of lives the player starts the game with.")]
    [Min(1)][SerializeField] private int startingHealth;
    public int StartingHealth => startingHealth;

    [Tooltip("The maximum number of lives the player can accumulate.")]
    [SerializeField] private int maxHealth;
    public int MaxHealth
    {
        get
        {
            Assert.IsTrue(maxHealth >= startingHealth, $"Max health cannot be inferior to starting health! Check your values in {name} data.");
            return maxHealth;
        }
    }

    [Tooltip("The additional score required to gain a life.")]
    [SerializeField] private int scorePerBonusLife;
    public int ScorePerBonusLife => scorePerBonusLife;
    
    [SerializeField] private LevelData[] levels;
    public LevelData[] Levels
    {
        get
        {
            Assert.IsTrue(levels?.Length > 0, $"There must be at least on level configured in {name} data.");
            return levels;
        }
    }
}