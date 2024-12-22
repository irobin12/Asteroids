using System;
using System.Collections;
using Data;
using Entities;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HUD hud;

    private Player player;
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
        InputManager.SetUp(gameData.player);

        gameObject.TryAddComponent(out rocksManager);
        // rocksManager = TryAddComponent();
    }

    private void Start()
    {
        hud.SetUp(this, gameData.maxHealth, gameData.startingHealth);
        SetHealth(gameData.startingHealth);
        CreatePlayer();
        rocksManager.SetUp(gameData.levels[0]);
        rocksManager.OnScoreChanged += SetScore;
    }

    private void SetHealth(int health)
    {
        if (health == currentHealth || health >= gameData.maxHealth) return;
        
        currentHealth = health;
        HealthChanged?.Invoke(currentHealth);
    }

    private void CreatePlayer()
    {
        player = Instantiate(gameData.player.prefab);
        if (player != null)
        {
            player.SetUp(gameData.player);
            player.Destroyed += OnPlayerDeath;
        }
        else
        {
            Debug.LogWarning($"PlayerData needs a prefab with a Player component attached for the game to run!");
        }
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
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer()
    {
        player.Reset();
        yield return new WaitForSeconds(gameData.player.respawnTime);
        player.gameObject.SetActive(true);
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
}