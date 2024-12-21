using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Player data")]
    public Player playerPrefab;
    [Range(0f, 20f)] 
    public float playerThrust = 10f;
    [Range(0f, 20f)] 
    public float playerTorque = 5f;
    
    [Space]
    public Rock rock;
    public Enemy enemy;
}