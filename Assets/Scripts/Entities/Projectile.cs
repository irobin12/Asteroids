using System;
using Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Entities
{
    [RequireComponent(typeof(MovementManager), typeof(Collider2D))]
    public class Projectile: MonoBehaviour, IEntity<ProjectileData>, IDestroyable, IPoolable
    {
        public Action<Projectile> Released;
        
        private ProjectileData data;
        private MovementManager movementManager;
        private ObjectPool<Projectile> pool;
        private float lifetime;
        private float timeSinceSpawned;

        public void SetUp(ProjectileData projectileData)
        { 
            data = projectileData;
            lifetime = data.lifetime;
            movementManager = GetComponent<MovementManager>();
            movementManager.SetUp(projectileData.launchVelocity, projectileData.rotationSpeed);
            movementManager.SetMovement(true, false, false);
        }

        public void SetFromStart()
        {
            timeSinceSpawned = 0f;
        }

        private void Update()
        {
            movementManager.Update();
         
            if (timeSinceSpawned < lifetime)
            {
                timeSinceSpawned += Time.deltaTime;
            }
            else
            {
                Destroy();
            }
        }

        /// <summary>
        /// Called upon collision or at end of lifetime.
        /// </summary>
        public void Destroy()
        {
            // Release to pool
            Released?.Invoke(this);
        }

        public void Release()
        {
            Released?.Invoke(this);
        }
    }
}