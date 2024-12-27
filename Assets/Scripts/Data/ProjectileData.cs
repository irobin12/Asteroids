using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData", order = 1)]
public class ProjectileData : EntityData
{
    public Projectile prefab;

    [Range(0f, 5f)] [Tooltip("Time to wait the projectile disappears after being launched?")]
    public float lifetime = 0.5f;

    [Range(0f, 5f)] [Tooltip("Time to wait before a new projectile can be launched after the last one.")]
    public float cooldown = 0.5f;
}