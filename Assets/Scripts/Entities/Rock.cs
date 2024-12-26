using System;
using Data;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(MovementManager))]
    public class Rock : MonoBehaviour, IEntity<RockData>, IDestroyable, IPoolable
    {
        public Action<Rock> Destroyed;
        public Action<Rock> Released;

        public RockData Data { get; private set; }
        private MovementManager movementManager;

        public void SetUp(RockData data)
        {
            Data = data;
            movementManager = GetComponent<MovementManager>();
            movementManager.SetUp(Data.launchVelocity, Data.rotationSpeed);
            movementManager.SetMovement(true, false, false);
        }

        public void SetFromStart() { }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                if (other.gameObject.TryGetComponent(out Projectile projectile))
                {
                    projectile.Destroy();
                }
                
                Destroy();
            }
        }

        public void Destroy()
        {
            Destroyed?.Invoke(this);
        }

        public void Release()
        {
            Released?.Invoke(this);
        }
    }
}