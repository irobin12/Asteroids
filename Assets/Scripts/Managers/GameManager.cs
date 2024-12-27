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
        VerifyData();

        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
        InputManager.SetUp(gameData.inputData);

        playerManager = GetComponent<PlayerManager>();
        rocksManager = GetComponent<RocksManager>();
        enemiesManager = GetComponent<EnemiesManager>();
    }

    private void Start()
    {
        InputManager.RestartKeyPressed += RestartGame;

        if (hud) hud.SetUp(this, gameData.maxHealth, gameData.startingHealth);

        playerManager.SetUp(gameData.player);
        playerManager.PlayerDeath += OnPlayerDeath;
        playerManager.PlayerStarted += OnPlayerStarted;

        rocksManager.SetUp(gameData.levels[0]);
        rocksManager.OnScoreChanged += ChangeScore;
        
        enemiesManager.SetUp(gameData.bigEnemy, gameData.smallEnemy);
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

    private void VerifyData()
    {
        Assert.IsNotNull(gameData, "Game Data is null, ensure there is one assigned in the Game Manager.");
        Assert.IsNotNull(mainCamera, "No main camera assigned, ensure it is present in the Game Manager.");
        Assert.IsNotNull(hud, "No HUD assigned, ensure it is present in the Game Manager.");
        Assert.IsNotNull(gameData.bigEnemy?.prefab, "No prefab assigned for Big Enemy, ensure both are present in the Game Manager.");
        Assert.IsNotNull(gameData.smallEnemy?.prefab, "No prefab assigned for Small Enemy, ensure both are present in the Game Manager.");
        Assert.IsNotNull(gameData.player?.prefab, "No prefab assigned for Player, ensure it is present in the Game Data.");
        Assert.IsNotNull(gameData.inputData, "No Input Data is present in the Game Manager.");

        Assert.IsTrue(gameData.startingHealth >= 1,
            "You cannot play the game with less than 1 health! Please put a higher value in the starting health field of Game Data.");
        Assert.IsTrue(gameData.maxHealth >= gameData.startingHealth,
            "Max health cannot be inferior to starting health! Check your values in the Game Data file.");
        Assert.IsTrue(gameData.levels?.Length > 0, "There must be at least on level configured in the Game Data file.");

        VerifyKeyCodes(gameData.inputData.moveForwardKeys);
        VerifyKeyCodes(gameData.inputData.moveLeftKeys);
        VerifyKeyCodes(gameData.inputData.moveRightKeys);
        VerifyKeyCodes(gameData.inputData.shootKeys);
        VerifyKeyCodes(gameData.inputData.teleportationKeys);
        VerifyKeyCodes(gameData.inputData.restartKeys);
    }

    private static void VerifyKeyCodes(KeyCode[] keyCodes)
    {
        Assert.IsTrue(keyCodes.Length > 0, "All key codes fields in the Input Data must have at least one entry assigned.");
    }

    private void ResetUserData()
    {
        SetScoreAt(0);
        TrySetHealth(gameData.startingHealth);
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
        if (newHealth == currentHealth || newHealth > gameData.maxHealth) return;

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
        var currentScoreMultipleForBonus = currentScore / gameData.scorePerBonusLife;
        if (currentScoreMultipleForBonus >= 1)
        {
            var previousScoreMultipleForBonus = previousScore / gameData.scorePerBonusLife;
            if (previousScoreMultipleForBonus < currentScoreMultipleForBonus) TrySetHealth(currentHealth + 1);
        }
    }
}