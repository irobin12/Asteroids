using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : EntityData
{
    [SerializeField] private Player prefab;
    public Player Prefab
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
            Assert.IsNotNull(projectileData, $"Projectile Data is null in {name} data, please assign one.");
            return projectileData;
        }
    }

    [Range(0f, 20f)] [Tooltip("Optional. Force (speed) at which the entity starts rotating. (Torque)")]
    [SerializeField] private float rotationSpeed = 5f;
    public float RotationSpeed => rotationSpeed;

    [Tooltip("Player respawn countdown time in seconds after death.")]
    [SerializeField] private float respawnTime = 2f;
    public float RespawnTime => respawnTime;

    [Tooltip("Time between the beginning of the teleportation and the reappearance on screen.")]
    [SerializeField] private float teleportationTime = 0.5f;
    public float TeleportationTime => teleportationTime;
}