using Data;
using UnityEngine;

public class DataValidator
{
    public static void VerifyData(GameData data)
    {
        if (data == null)
        {
            Debug.LogWarning($"Game Data is null, the game cannot be played without it assigned in the Game Manager.");
        }

        if (data.startingHealth < 1)
        {
            Debug.LogWarning("You cannot play the game with less than 1 health! Please put a higher value in the starting health field of Game Data.");
        }

        if (data.maxHealth <= data.startingHealth)
        {
            Debug.LogWarning("Max health cannot be inferior to starting health! Check your values in the Game Data file.");
        }
    }
}