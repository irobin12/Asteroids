using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 1)]
public class EnemyData : EntityData
{
    [SerializeField] private Enemy prefab;
    public Enemy Prefab
    {
        get
        {
            Assert.IsNotNull(prefab, $"Prefab is null in {name} data, please assign one.");
            return prefab;
        }
    }

    [SerializeField] private ProjectileData projectileData;
    public ProjectileData ProjectileData
    {
        get
        {
            Assert.IsNotNull(projectileData, $"ProjectileData is null in {name} data, please assign one.");
            return projectileData;
        }
    }

    [Tooltip("Score gained for destroying this enemy.")]
    [SerializeField] private int score;
    public int Score => score;
    
    [Range(1f, 60f)] [Tooltip("Time to wait before an enemy appears on screen, from start or from the last one.")]
    [SerializeField] private float cooldown = 30f;
    public float Cooldown => cooldown;

    [Range(0f, 0.9f)] [Tooltip("How random the cooldown time will be in %. 0 means cooldown time will always stay the same, 0.9 means it can be up to 90% more or less time.")]
    [SerializeField] private float cooldownRandomness = 0.2f;
    public float CooldownRandomness => cooldownRandomness;

    [Range(0, 90)] [Tooltip("Angle at which the enemy will divert from its original y position.")]
    [SerializeField] private int movementAngle = 45;
    public float MovementAngle => movementAngle;

    [Range(0, 90)][Tooltip("Minimum angle from which the projectile spawner will start shooting away from its entity.")]
    [SerializeField] private int shootingAnglePadding = 20;
    public int ShootingAnglePadding => shootingAnglePadding;
}