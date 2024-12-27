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

    /// <summary>
    ///     bool is true when game was just lost, false when it restarted (to reset the HUD)
    /// </summary>
    public Action<bool> GameOver;

    /// <summary>
    ///     int is the new health
    /// </summary>
    public Action<int> HealthChanged;

    private PlayerManager playerManager;
    private RocksManager rocksManager;
    private EnemiesManager enemiesManager;

    /// <summary>
    ///     int is the new score
    /// </summary>
    public Action<int> ScoreChanged;

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

        if (hud) hud.SetUp(this, gameData.MaxHealth, gameData.StartingHealth);

        playerManager.SetUp(gameData.Player);
        playerManager.PlayerDeath += OnPlayerDeath;
        playerManager.PlayerStarted += OnPlayerStarted;

        rocksManager.SetUp(gameData.Levels[0]);
        rocksManager.OnScoreChanged += ChangeScore;
        
        enemiesManager.SetUp(gameData.BigEnemy, gameData.SmallEnemy);
        enemiesManager.OnScoreChanged += ChangeScore;

        ResetUserData();
        playerManager.SetFromStart();
        rocksManager.SetFromStart();
    }

    private void Update()
    {
        InputManager.Update();
    }

    private void OnDestroy()
    {
        if (playerManager) playerManager.PlayerDeath -= OnPlayerDeath;
        if (playerManager) playerManager.PlayerStarted -= OnPlayerStarted;
        if (rocksManager) rocksManager.OnScoreChanged -= ChangeScore;
        if (enemiesManager) enemiesManager.OnScoreChanged -= ChangeScore;
    }

    private void ResetUserData()
    {
        SetScoreAt(0);
        TrySetHealth(gameData.StartingHealth);
        GameOver?.Invoke(false);
    }

    private void RestartGame()
    {
        ResetUserData();
        playerManager.ResetPlayerFromStart();
        rocksManager.ResetFromStart();
        enemiesManager.ResetFromStart();
    }

    private void OnPlayerDeath()
    {
        // TODO disable respawn of enemies, avoid resetting them after restart from death
        TrySetHealth(currentHealth - 1);
        if (currentHealth <= 0)
            GameOver?.Invoke(true);
        else
            StartCoroutine(playerManager.RespawnPlayer());
    }

    private void OnPlayerStarted()
    {
        enemiesManager.ResetFromStart();
    }

    private void TrySetHealth(int newHealth)
    {
        if (newHealth == currentHealth || newHealth > gameData.MaxHealth) return;

        currentHealth = newHealth;
        HealthChanged?.Invoke(currentHealth);
    }

    private void ChangeScore(int scoreAdded)
    {
        var previousScore = currentScore;
        currentScore += scoreAdded;
        TryAddBonusLife(previousScore);

        SetScoreAt(currentScore);
    }

    private void SetScoreAt(int score)
    {
        ScoreChanged?.Invoke(score);
    }

    /// <summary>
    ///     Test if the player has reached a new threshold for getting a bonus life, and give one more health if that is the
    ///     case.
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
}