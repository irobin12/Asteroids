using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private HorizontalLayoutGroup healthBar;
    [SerializeField] private Image lifeIcon;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private int currentHealth;
    private Image[] lifeIcons;

    public void SetUp(GameManager gameManager, int maxHealth, int startingHealth)
    {
        foreach (Transform child in healthBar.transform)
        {
            Destroy(child.gameObject);
        }
        
        SetUpScore(gameManager);
        SetUpHealth(gameManager, maxHealth, startingHealth);
        SetGameOverTextActive(false);
    }

    private void SetUpScore(GameManager gameManager)
    {
        gameManager.ScoreChanged += OnScoreChanged;
        SetScore(0);
    }

    private void SetUpHealth(GameManager gameManager, int maxHealth, int startingHealth)
    {
        lifeIcons = new Image[maxHealth];
        
        for (int i = 0; i < maxHealth; i++)
        {
            var icon = Instantiate(lifeIcon, healthBar.transform);
            if (i >= startingHealth)
            {
                icon.gameObject.SetActive(false);
            }
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
        {
            for (int i = currentHealth; i < newHealth; i++)
            {
                SetLifeIconActive(lifeIcons[i], true);
            }
        } 
        else if (newHealth < currentHealth)
        {
            for (int i = newHealth; i < currentHealth; i++)
            {
                SetLifeIconActive(lifeIcons[i], false);
            }
        }
        
        currentHealth = newHealth;
    }

    private void SetLifeIconActive(Image icon, bool setActive)
    {
        icon.gameObject.SetActive(setActive);
    }

    public void SetGameOverTextActive(bool setActive)
    {
        gameOverText.gameObject.SetActive(setActive);
    }
}