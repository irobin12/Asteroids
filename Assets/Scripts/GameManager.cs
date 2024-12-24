using System;
using System.Collections;
using Data;
using Entities;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class GameManager: MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HUD hud;
    [SerializeField]private PlayerManager playerManager;
    
    private RocksManager rocksManager;
    private int currentHealth;
    private int currentScore;
    
    /// <summary>
    /// int is the new health
    /// </summary>
    public Action<int> HealthChanged;    
    
    /// <summary>
    /// int is the new score
    /// </summary>
    public Action<int> ScoreChanged;

    private void Awake()
    {
        DataValidator.VerifyData(gameData);

        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
        InputManager.SetUp(gameData.inputData);

        gameObject.TryAddComponent(out rocksManager);
    }

    private void Start()
    {
        hud.SetUp(this, gameData.maxHealth, gameData.startingHealth);
        TrySetHealth(gameData.startingHealth);
        
        playerManager.SetUp(gameData.player);
        playerManager.PlayerDeath += OnPlayerDeath;
        
        rocksManager.SetUp(gameData.levels[0]);
        rocksManager.OnScoreChanged += SetScore;
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

    private void SetScore(int scoreAdded)
    {
        var previousScore = currentScore;
        currentScore += scoreAdded;
        TryAddBonusLife(previousScore);
        ScoreChanged?.Invoke(currentScore);
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
        rocksManager.OnScoreChanged -= SetScore;
    }
}