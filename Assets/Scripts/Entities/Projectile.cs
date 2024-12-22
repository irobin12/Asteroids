using System;
using Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Entities
{
    [RequireComponent(typeof(MovementManager), typeof(Collider2D))]
    public class Projectile: MonoBehaviour, IEntity<ProjectileData>, IDestroyable
    {
        public Action<Projectile> Destroyed;
        private ProjectileData data;
        private MovementManager movementManager;
        private ObjectPool<Projectile> pool;
        private float lifetime;
        private float timeSinceSpawned;

        public void SetUp(ProjectileData projectileData)
        { 
            this.data = projectileData;
            lifetime = this.data.lifetime;
            movementManager = GetComponent<MovementManager>();
            movementManager.SetUp(projectileData);
            movementManager.SetMovement(true, false, false);
        }

        public void Reset()
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
            Destroyed?.Invoke(this);
        }
    }
}