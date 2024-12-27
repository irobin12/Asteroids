using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerManager), typeof(RocksManager), typeof(EnemiesManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HUD hud;

    private int currentHealth;
    private int currentScore;
    private int currentCollectiblesCount;

    /// <summary>
    ///     bool is true if the game was won, false if lost
    /// </summary>
    public Action<bool> GameOver;

    /// <summary>
    ///     int is the new health
    /// </summary>
    public Action<int> HealthChanged;

    /// <summary>
    ///     int is the new score
    /// </summary>
    public Action<int> ScoreChanged;
    
    /// <summary>
    ///     int is the new collectibles count
    /// </summary>
    public Action<int> CollectiblesCountChanged;

    private PlayerManager playerManager;
    private RocksManager rocksManager;
    private EnemiesManager enemiesManager;

    private void Awake()
    {
        Assert.IsNotNull(gameData, "Game Data is null, ensure there is one assigned in the Game Manager.");
        Assert.IsNotNull(mainCamera, "No main camera assigned, ensure it is present in the Game Manager.");
        Assert.IsNotNull(hud, "No HUD assigned, ensure it is present in the Game Manager.");

        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
        InputManager.SetUp(gameData.Input);

        playerManager = GetComponent<PlayerManager>();
        rocksManager = GetComponent<RocksManager>();
        enemiesManager = GetComponent<EnemiesManager>();
    }

    private void Start()
    {
        InputManager.RestartKeyPressed += RestartGame;

        if (hud)
        {
            hud.SetUp(this, gameData.MaxHealth, gameData.StartingHealth, gameData.WinningCollectibleCount);
        }

        playerManager.SetUp(gameData.Player);
        playerManager.PlayerDeath += OnPlayerDeath;
        playerManager.PlayerStarted += OnPlayerStarted;

        rocksManager.SetUp(gameData.Levels[0]);
        rocksManager.ScoreChanged += OnScoreChanged;
        rocksManager.RockCollected += OnRockCollected;
        
        enemiesManager.SetUp(playerManager, gameData.BigEnemy, gameData.SmallEnemy);
        enemiesManager.ScoreChanged += OnScoreChanged;

        ResetUserData();
        playerManager.SetFromStart();
        rocksManager.SetFromStart();
    }

    private void OnRockCollected(Rock rock)
    {
        SetCollectibleCount(currentCollectiblesCount + 1);
    }

    private void SetCollectibleCount(int collectibleCount)
    {
        currentCollectiblesCount = collectibleCount;
        CollectiblesCountChanged?.Invoke(currentCollectiblesCount);
        
        if (currentCollectiblesCount == gameData.WinningCollectibleCount)
        {
            GameOver?.Invoke(true);
        }
    }

    private void Update()
    {
        InputManager.Update();
    }

    private void ResetUserData()
    {
        InvokeScoreChanged(0);
        SetCollectibleCount(0);
        TrySetHealth(gameData.StartingHealth);
        hud.Reset();
        // GameOver?.Invoke(false);
    }

    private void RestartGame()
    {
        ResetUserData();
        playerManager.ResetFromRestart();
        rocksManager.ResetFromRestart();
        enemiesManager.ResetFromRestart();
    }

    private void OnPlayerDeath()
    {
        TrySetHealth(currentHealth - 1);
        
        if (currentHealth <= 0)
            GameOver?.Invoke(false);
        else
            StartCoroutine(playerManager.RespawnPlayer());
    }

    private void OnPlayerStarted()
    {
        enemiesManager.SetFromStart();
    }

    private void TrySetHealth(int newHealth)
    {
        if (newHealth == currentHealth || newHealth > gameData.MaxHealth) return;

        currentHealth = newHealth;
        HealthChanged?.Invoke(currentHealth);
    }

    private void OnScoreChanged(int scoreAdded)
    {
        var previousScore = currentScore;
        currentScore += scoreAdded;
        TryAddBonusLife(previousScore);

        InvokeScoreChanged(currentScore);
    }

    private void InvokeScoreChanged(int score)
    {
        ScoreChanged?.Invoke(score);
    }

    /// <summary>
    ///     Test if the player has reached a new threshold for getting a bonus life, and give one more health if that is the case.
    /// </summary>
    private void TryAddBonusLife(int previousScore)
    {
        var currentScoreMultipleForBonus = currentScore / gameData.ScorePerBonusLife;
        if (currentScoreMultipleForBonus >= 1)
        {
            var previousScoreMultipleForBonus = previousScore / gameData.ScorePerBonusLife;
            if (previousScoreMultipleForBonus < currentScoreMultipleForBonus) TrySetHealth(currentHealth + 1);
        }
    }

    private void OnDestroy()
    {
        if (playerManager)
        {
            playerManager.PlayerDeath -= OnPlayerDeath;
            playerManager.PlayerStarted -= OnPlayerStarted;
        }

        if (enemiesManager)
        {
            enemiesManager.ScoreChanged -= OnScoreChanged;
        }
        
        if (rocksManager)
        {
            rocksManager.ScoreChanged -= OnScoreChanged;
            rocksManager.RockCollected -= OnRockCollected;
        }
    }
}