using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Projectile projectilePrefab;
    
    [SerializeField] private float projectileThrust = 5f;
    [SerializeField] private float projectileLifetime = 2f;
    
    private MovingEntityPool pool;

    public void Initialise()
    {
        pool = new MovingEntityPool(projectilePrefab, projectileThrust, 10, 50);
    }

    public void SpawnProjectile()
    {
        var projectile = pool.GetEntity(spawnPoint).GetComponent<Projectile>();
        projectile.Death += ReleaseProjectile;
        projectile.SetUp(projectileLifetime, true);
    }

    private void ReleaseProjectile(Projectile projectile)
    {
        projectile.Death -= ReleaseProjectile;
        pool.ReleaseEntity(projectile);
    }
}