using UnityEngine;

public class ProjectileSpawner : EntitySpawner<Projectile>
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Projectile projectilePrefab;

    private ProjectileData projectileData;
    private float timeSinceLastSpawn;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }

    public void SetUp(ProjectileData data, int defaultSize = 5, int maxSize = 15)
    {
        projectileData = data;
        base.SetUp(data.prefab, defaultSize, maxSize);
    }

    public void SpawnProjectile()
    {
        if (timeSinceLastSpawn < projectileData.cooldown) return;

        var projectile = Pool.GetObject(spawnPoint.position, spawnPoint.rotation);
        projectile.Released += ReleaseProjectile;
        projectile.SetUp(projectileData);

        timeSinceLastSpawn = 0;
    }

    private void ReleaseProjectile(Projectile projectile)
    {
        projectile.Released -= ReleaseProjectile;
        projectile.SetFromStart();
        Pool.ReleaseGameObject(projectile);
    }
}