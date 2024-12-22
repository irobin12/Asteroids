using System;
using System.Collections;
using Data;
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
        VerifyData();

        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
        InputManager.SetUp(gameData.player);
        
        if (!TryGetComponent(out rocksManager))
        {
            rocksManager = gameObject.AddComponent<RocksManager>();
        }
    }

    private void VerifyData()
    {
        if (gameData == null)
        {
            Debug.LogWarning($"Game Data is null, the game cannot be played without it assigned in the Game Manager.");
        }

        if (gameData.startingHealth < 1)
        {
            Debug.LogWarning("You cannot play the game with less than 1 health! Please put a higher value in the starting health field of Game Data.");
        }

        if (gameData.maxHealth <= gameData.startingHealth)
        {
            Debug.LogWarning("Max health cannot be inferior to starting health! Check your values in the Game Data file.");
        }
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
        var playerInstance = Instantiate(gameData.player.prefab);
        if (playerInstance.TryGetComponent(out player))
        {
            player.SetUp(gameData.player, gameData.player.projectileData);
        }
        else
        {
            Debug.LogError($"The MovingEntity assigned to the {nameof(MovingEntityData.prefab)} field of " +
                           $"{nameof(gameData.player)} in {nameof(gameData)} must have a {nameof(Player)} " +
                           $"component attached for the game to run. ");
        }

        player.Death += OnPlayerDeath;
    }

    private void OnPlayerDeath(MovingEntity entity)
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