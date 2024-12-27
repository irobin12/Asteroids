using System;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(MovementManager), typeof(Collider2D))]
public class Projectile : MonoBehaviour, IEntity<ProjectileData>, IDestroyable, IPoolable
{
    private ProjectileData data;
    private float lifetime;
    private MovementManager movementManager;
    private ObjectPool<Projectile> pool;
    public Action<Projectile> Released;
    private float timeSinceSpawned;

    private void Update()
    {
        movementManager.Update();

        if (timeSinceSpawned < lifetime)
            timeSinceSpawned += Time.deltaTime;
        else
            Destroy();
    }

    /// <summary>
    ///     Called upon collision or at end of lifetime.
    /// </summary>
    public void Destroy()
    {
        // Release to pool
        Released?.Invoke(this);
    }

    public void SetUp(ProjectileData projectileData)
    {
        data = projectileData;
        lifetime = data.Lifetime;
        movementManager = GetComponent<MovementManager>();
        movementManager.SetUp(true, projectileData.LaunchVelocity);
        movementManager.SetMovement(true);
    }

    public void SetFromStart()
    {
        timeSinceSpawned = 0f;
    }

    public void Release()
    {
        movementManager.ResetVelocity();
        Released?.Invoke(this);
    }
}