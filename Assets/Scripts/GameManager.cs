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
        SetHealth(gameData.startingHealth);
        
        playerManager.SetUp(gameData.player);
        playerManager.PlayerDeath += OnPlayerDeath;
        
        rocksManager.SetUp(gameData.levels[0]);
        rocksManager.OnScoreChanged += SetScore;
    }

    private void OnPlayerDeath()
    {
        SetHealth(currentHealth - 1);
        if (currentHealth <= 0)
        {
            hud.SetGameOverTextActive(true);
        }
        else
        {
            StartCoroutine(playerManager.RespawnPlayer());
        }
    }

    private void SetHealth(int health)
    {
        if (health == currentHealth || health >= gameData.maxHealth) return;
        
        currentHealth = health;
        HealthChanged?.Invoke(currentHealth);
    }

    private void SetScore(int scoreAdded)
    {
        currentScore += scoreAdded;
        ScoreChanged?.Invoke(currentScore);
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