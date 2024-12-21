using Data;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Projectile projectilePrefab;
    
    private ProjectileData data;
    private MovingEntityPool pool;
    private float timeSinceLastSpawn;

    public void Initialize(ProjectileData projectileData)
    {
        data = projectileData;
        pool = new MovingEntityPool(projectileData, 5, 15);
    }

    public void SpawnProjectile()
    {
        if (timeSinceLastSpawn < data.cooldown) return;
        
        var projectile = pool.GetEntity(spawnPoint.position, spawnPoint.rotation).GetComponent<Projectile>();
        projectile.Death += ReleaseProjectile;
        projectile.Initialize(data.lifetime, true);
        
        timeSinceLastSpawn = 0;
    }

    private void ReleaseProjectile(MovingEntity projectile)
    {
        projectile.Death -= ReleaseProjectile;
        pool.ReleaseEntity(projectile);
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }
}