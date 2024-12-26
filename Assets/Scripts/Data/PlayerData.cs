using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : EntityData
{
    public Player prefab;
    public ProjectileData projectileData;

    [Range(0f, 20f)] [Tooltip("Optional. Force (speed) at which the entity starts rotating. (Torque)")]
    public float rotationSpeed = 5f;

    [Tooltip("Player respawn countdown time in seconds after death.")]
    public float respawnTime = 2f;

    [Tooltip("Time between the beginning of the teleportation and the reappearance on screen.")]
    public float teleportationTime = 0.5f;
}