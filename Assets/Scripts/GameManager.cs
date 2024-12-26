using System;
using Data;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerManager), typeof(RocksManager))]
public class GameManager: MonoBehaviour
{
    /// <summary>
    /// int is the new health
    /// </summary>
    public Action<int> HealthChanged;    
    
    /// <summary>
    /// int is the new score
    /// </summary>
    public Action<int> ScoreChanged;
    
    /// <summary>
    /// bool is true when game was just lost, false when it restarted (to reset the HUD)
    /// </summary>
    public Action<bool> GameOver;
    
    [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HUD hud;
    
    private PlayerManager playerManager;
    private RocksManager rocksManager;
    
    private int currentHealth;
    private int currentScore;

    private void Awake()
    {
        VerifyData();

        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
        InputManager.SetUp(gameData.inputData);

        playerManager = GetComponent<PlayerManager>();
        rocksManager = GetComponent<RocksManager>();
    }

    private void VerifyData()
    {
        Assert.IsNotNull(gameData, "Game Data is null, ensure there is one assigned in the Game Manager.");
        Assert.IsNotNull(mainCamera, "No main camera assigned, ensure it is present in the Game Manager.");
        Assert.IsNotNull(hud, "No HUD assigned, ensure it is present in the Game Manager.");

        Assert.IsTrue(gameData.startingHealth >= 1,
            "You cannot play the game with less than 1 health! Please put a higher value in the starting health field of Game Data.");
        Assert.IsTrue(gameData.maxHealth >= gameData.startingHealth,
            "Max health cannot be inferior to starting health! Check your values in the Game Data file.");
        Assert.IsTrue(gameData.levels.Length > 0, "There must be at least on level configured.");

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

    private void Start()
    {
        InputManager.RestartKeyPressed += RestartGame;

        if (hud)
        {
            hud.SetUp(this, gameData.maxHealth, gameData.startingHealth);
        }
        
        playerManager.SetUp(gameData.player);
        playerManager.PlayerDeath += OnPlayerDeath;
        
        rocksManager.SetUp(gameData.levels[0]);
        rocksManager.OnScoreChanged += ChangeScore;
        
        ResetUserData();
        playerManager.SetFromStart();
        rocksManager.SetFromStart();
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
    }

    private void OnPlayerDeath()
    {
        TrySetHealth(currentHealth - 1);
        if (currentHealth <= 0)
        {
            GameOver?.Invoke(true);
        }
        else
        {
            StartCoroutine(playerManager.RespawnPlayer());
        }
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
    /// Test if the player has reached a new threshold for getting a bonus life, and give one more health if that is the case.
    /// </summary>
    private void TryAddBonusLife(int previousScore)
    {
        var currentScoreMultipleForBonus = currentScore / gameData.scorePerBonusLife;
        if (currentScoreMultipleForBonus >= 1)
        {
            var previousScoreMultipleForBonus = previousScore / gameData.scorePerBonusLife;
            if (previousScoreMultipleForBonus < currentScoreMultipleForBonus)
            {
                TrySetHealth(currentHealth + 1);
            }
        }
    }

    private void Update()
    {
        InputManager.Update();
    }

    private void OnDestroy()
    {
        if(playerManager) playerManager.PlayerDeath -= OnPlayerDeath;
        if(rocksManager) rocksManager.OnScoreChanged -= ChangeScore;
    }
}