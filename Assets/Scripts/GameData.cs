using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [FormerlySerializedAs("player")] [Header("Player data")]
    public Player playerPrefab;
    [Range(0f, 10f)] 
    public float playerThrust = 1f;
    [Range(0f, 10f)] 
    public float playerTorque = 0.05f;
    [Range(0, 200)] 
    public int screenPadding = 10;
    
    [Space]
    public Rock rock;
    public Enemy enemy;
}