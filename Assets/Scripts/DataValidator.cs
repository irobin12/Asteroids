using Data;
using UnityEngine;
using UnityEngine.Assertions;

public class DataValidator
{
    private readonly GameData data;
    
    public DataValidator(GameData data)
    {
        this.data = data;
    }
    
    public void VerifyData()
    {
        Assert.IsNotNull(data, "Game Data is null, the game cannot be played without it assigned in the Game Manager.");
        Assert.IsTrue(data.startingHealth >= 1, "You cannot play the game with less than 1 health! Please put a higher value in the starting health field of Game Data.");
        Assert.IsTrue(data.maxHealth >= data.startingHealth, "Max health cannot be inferior to starting health! Check your values in the Game Data file.");
        Assert.IsTrue(data.levels.Length > 0, "There must be at least on level configured.");
    }
}