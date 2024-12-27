using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleCount : MonoBehaviour
{
    [SerializeField] private Image fillableImage;
    [SerializeField] private TextMeshProUGUI countText;

    private GameManager gameManager;
    private int winningCount;
    private float fillAmountPerCount;

    public void SetUp(GameManager gameManager, int winningCollectiblesCount)
    {
        gameManager.CollectiblesCountChanged += OnCollectiblesCountChanged;
        winningCount = winningCollectiblesCount;
        fillAmountPerCount = 1f / winningCollectiblesCount;
        SetFillAmount(0);
    }

    private void OnCollectiblesCountChanged(int count)
    {
        if (count >= winningCount)
        {
            SetFullFill();
        }
        else
        {
            SetFillAmount(count);
        }
    }

    private void SetFillAmount(int count)
    {
        FillImage(count * fillAmountPerCount);
        SetText($"{count}/{winningCount}");
    }

    private void SetFullFill()
    {
        FillImage(1);
        SetText($"{winningCount}/{winningCount}");
    }

    private void FillImage(float fillAmount)
    {
        if(!fillableImage) return;
        fillableImage.fillAmount = fillAmount;
    }

    private void SetText(string text)
    {
        if(countText == null) return;
        countText.text = text;
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.CollectiblesCountChanged -= OnCollectiblesCountChanged;
        }
    }
}