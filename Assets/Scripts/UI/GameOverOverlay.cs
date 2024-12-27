using TMPro;
using UnityEngine;

public class GameOverOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winOrLoseText;
    [SerializeField] private TextMeshProUGUI restartPrompt;

    public void SetUp()
    {
        if (restartPrompt)
        {
            var restartKey = InputManager.RestartKeys[0].ToString();
            restartPrompt.SetText($"Press {restartKey} to restart");
        }
    }
    
    public void SetWinOrLoseText(bool won)
    {
        if (won)
        {
            winOrLoseText.text = "You've won!";
        }
        else
        {
            winOrLoseText.text = "You've lost!";
        }
    }
}