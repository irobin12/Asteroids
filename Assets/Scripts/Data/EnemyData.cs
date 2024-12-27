using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 1)]
public class EnemyData : EntityData
{
    public Enemy prefab;
    public ProjectileData projectileData;

    [Tooltip("Score gained for destroying this enemy.")]
    public int score;
    
    [Range(1f, 60f)] [Tooltip("Time to wait before an enemy appears on screen, from start or from the last one.")]
    public float cooldown = 30f;

    [Range(0f, 0.9f)] [Tooltip("How random the cooldown time will be in %. 0 means cooldown time will always stay the same, 0.9 means it can be up to 90% more or less time.")]
    public float cooldownRandomness = 0.2f;

    [Range(0, 90)] [Tooltip("Angle at which the enemy will divert from its original y position.")]
    public int movementAngle = 45;
}