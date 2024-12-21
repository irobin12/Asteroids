using System;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile: MovingEntity
{
    private ObjectPool<Projectile> pool;
    private float lifetime;
    private float timeSinceSpawned;
    
    public void SetUp(float projectileLifetime, bool forward)
    {
        lifetime = projectileLifetime;
        SetMovement(forward, false, false);
    }

    public override void Reset()
    {
        timeSinceSpawned = 0f;
    }

    protected override void Update()
    {
        base.Update();
         
        if (timeSinceSpawned < lifetime)
        {
            timeSinceSpawned += Time.deltaTime;
        }
        else
        {
            Die();
        }
    }
}