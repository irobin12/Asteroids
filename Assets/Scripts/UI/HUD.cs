using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private HorizontalLayoutGroup healthBar;
    [SerializeField] private Image lifeIcon;
    [SerializeField] private RectTransform gameOverOverlay;
    [SerializeField] private TextMeshProUGUI restartPrompt;

    private int currentHealth;
    private GameManager gameManager;
    private Image[] lifeIcons;

    private void OnDestroy()
    {
        if (!gameManager) return;
        
        gameManager.GameOver -= OnGameOver;
        gameManager.ScoreChanged -= OnScoreChanged;
        gameManager.HealthChanged -= OnHealthChanged;
    }

    public void SetUp(GameManager manager, int maxHealth, int startingHealth)
    {
        gameManager = manager;

        foreach (Transform child in healthBar.transform) Destroy(child.gameObject);

        SetUpGameOver(manager);
        SetUpScore(manager);
        SetUpHealth(manager, maxHealth, startingHealth);
    }

    private void SetUpGameOver(GameManager gameManager)
    {
        gameManager.GameOver += OnGameOver;
        SetGameOverOverlayActive(false);

        if (restartPrompt)
        {
            var restartKey = InputManager.RestartKeys[0].ToString();
            restartPrompt.SetText($"Press {restartKey} to restart");
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

    private void OnGameOver(bool gameLost)
    {
        SetGameOverOverlayActive(gameLost);
    }
}