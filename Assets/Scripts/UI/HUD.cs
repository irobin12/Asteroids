using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private HorizontalLayoutGroup healthBar;
    [SerializeField] private Image lifeIcon;
    [SerializeField] private GameOverOverlay gameOverOverlay;
    [SerializeField] private CollectibleCount collectibleCount;

    private int currentHealth;
    private GameManager gameManager;
    private Image[] lifeIcons;

    public void SetUp(GameManager manager, int maxHealth, int startingHealth, int winningCollectiblesCount)
    {
        gameManager = manager;

        foreach (Transform child in healthBar.transform) Destroy(child.gameObject);

        SetUpGameOver(manager);
        SetUpScore(manager);
        SetUpHealth(manager, maxHealth, startingHealth);
        if (collectibleCount)
        {
            collectibleCount.SetUp(manager, winningCollectiblesCount);
        }
    }

    private void SetUpGameOver(GameManager gameManager)
    {
        SetGameOverOverlayActive(false);
        gameManager.GameOver += OnGameOver;
        
        if (gameOverOverlay)
        {
            gameOverOverlay.SetUp();
        }
    }

    private void SetUpScore(GameManager gameManager)
    {
        gameManager.ScoreChanged += OnScoreChanged;
        SetScore(0);
    }

    private void SetUpHealth(GameManager gameManager, int maxHealth, int startingHealth)
    {
        lifeIcons = new Image[maxHealth];

        for (var i = 0; i < maxHealth; i++)
        {
            var icon = Instantiate(lifeIcon, healthBar.transform);
            if (i >= startingHealth) icon.gameObject.SetActive(false);
            lifeIcons[i] = icon;
        }

        currentHealth = startingHealth;
        gameManager.HealthChanged += OnHealthChanged;
    }

    private void OnScoreChanged(int newScore)
    {
        SetScore(newScore);
    }

    private void SetScore(int newScore)
    {
        scoreText.text = newScore.ToString("N0");
    }

    private void OnHealthChanged(int newHealth)
    {
        SetNewHealth(newHealth);
    }

    private void SetNewHealth(int newHealth)
    {
        if (newHealth > currentHealth)
            for (var i = currentHealth; i < newHealth; i++)
                SetLifeIconActive(lifeIcons[i], true);
        else if (newHealth < currentHealth)
            for (var i = newHealth; i < currentHealth; i++)
                SetLifeIconActive(lifeIcons[i], false);

        currentHealth = newHealth;
    }

    private void SetLifeIconActive(Image icon, bool setActive)
    {
        if (!icon) return;
        
        icon.gameObject.SetActive(setActive);
    }

    private void SetGameOverOverlayActive(bool setActive)
    {
        if (!gameOverOverlay) return;
        
        gameOverOverlay.gameObject.SetActive(setActive);
    }

    private void OnGameOver(bool gameWon)
    {
        SetGameOverOverlayActive(true);
        if (gameOverOverlay)
        {
            gameOverOverlay.SetWinOrLoseText(gameWon);
        }
    }

    private void OnDestroy()
    {
        if (!gameManager) return;
        
        gameManager.GameOver -= OnGameOver;
        gameManager.ScoreChanged -= OnScoreChanged;
        gameManager.HealthChanged -= OnHealthChanged;
    }

    public void Reset()
    {
        SetGameOverOverlayActive(false);
    }
}