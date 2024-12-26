using System;
using Data;
using UnityEngine;

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
    
    [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HUD hud;
    
    private DataValidator dataValidator;
    private PlayerManager playerManager;
    private RocksManager rocksManager;
    
    private int currentHealth;
    private int currentScore;

    private void Awake()
    {
        dataValidator = new DataValidator(gameData);
        dataValidator.VerifyData();

        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
        InputManager.SetUp(gameData.inputData);

        playerManager = GetComponent<PlayerManager>();
        rocksManager = GetComponent<RocksManager>();
    }

    private void Start()
    {
        InputManager.RestartKeyPressed += RestartGame;
        
        hud.SetUp(this, gameData.maxHealth, gameData.startingHealth);
        
        playerManager.SetUp(gameData.player);
        playerManager.PlayerDeath += OnPlayerDeath;
        
        rocksManager.SetUp(gameData.levels[0]);
        rocksManager.OnScoreChanged += ChangeScore;
        
        SetGameFromStart();
    }

    private void SetGameFromStart()
    {
        TrySetHealth(gameData.startingHealth);
        SetScoreAt(0);
        playerManager.ResetPlayer();
        rocksManager.SetFromStart();
        // Reset player
        // Reset projectiles
        // Reset rocks
        // Reset enemies
    }

    private void RestartGame()
    {
        SetGameFromStart();
    }

    private void OnPlayerDeath()
    {
        TrySetHealth(currentHealth - 1);
        if (currentHealth <= 0)
        {
            hud.SetGameOverTextActive(true);
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