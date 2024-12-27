using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData", order = 1)]
public class ProjectileData : EntityData
{
    [SerializeField] private Projectile prefab;
    public Projectile Prefab
    {
        get
        {
            Assert.IsNotNull(prefab, $"Prefab is null in {name} data, please assign one.");
            return prefab;
        }
    }

    [Range(0f, 5f)] [Tooltip("Time to wait the projectile disappears after being launched?")]
    [SerializeField] private float lifetime = 0.5f;
    public float Lifetime => lifetime;

    [Range(0f, 5f)] [Tooltip("Time to wait before a new projectile can be launched after the last one.")]
    [SerializeField] private float cooldown = 0.5f;
    public float Cooldown => cooldown;
}