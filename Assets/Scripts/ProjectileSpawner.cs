using Data;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Projectile projectilePrefab;
    
    private ProjectileData projectileData;
    private MovingEntityPool pool;

    public void Initialise(ProjectileData data)
    {
        projectileData = data;
        pool = new MovingEntityPool(projectileData, 10, 50);
    }

    public void SpawnProjectile()
    {
        var projectile = pool.GetEntity(spawnPoint).GetComponent<Projectile>();
        projectile.Death += ReleaseProjectile;
        projectile.SetUp(projectileData.lifetime, true);
    }

    private void ReleaseProjectile(Projectile projectile)
    {
        projectile.Death -= ReleaseProjectile;
        pool.ReleaseEntity(projectile);
    }
}