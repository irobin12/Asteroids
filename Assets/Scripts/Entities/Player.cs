using System;
using Data;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(ProjectileSpawner), typeof(MovementManager))]
    public class Player : MonoBehaviour, IEntity<PlayerData>, IDestroyable
    {
        [SerializeField] private Animation animationWaitingForRespawn;
        
        public Action Death;
        public ProjectileSpawner ProjectileSpawner {get; private set;}
        
        private MovementManager movementManager;

        private bool lockFire;
        private PlayerData data;

        private bool moveForward;
        private bool turnLeft;
        private bool turnRight;
        private bool shoot;
        private bool isAlreadyDestroyed; // To avoid calling Destroyed twice if hit by two enemies simultaneously
        
        public void SetUp(PlayerData playerData)
        {
            data = playerData;
            movementManager = GetComponent<MovementManager>();
            movementManager.SetUp(data.launchVelocity, data.rotationSpeed);
            
            ProjectileSpawner = GetComponent<ProjectileSpawner>();
            ProjectileSpawner.SetUp(data.projectileData);
        
            lockFire = InputManager.Data.lockFire;
        
            InputManager.MoveForwardKeyPressed += MoveForward;
            InputManager.MoveLeftKeyPressed += TurnLeft;
            InputManager.MoveRightKeyPressed += TurnRight;
            InputManager.ShootKeyPressed += Shoot;
        }

        private void MoveForward()
        {
            moveForward = true;
        }

        private void TurnLeft()
        {
            turnLeft = true;
        }

        private void TurnRight()
        {
            turnRight = true;
        }

        private void Shoot()
        {
            if (lockFire)
            {
                shoot = !shoot;
            }
            else
            {
                shoot = true;
            }
        }

        private void FixedUpdate()
        {
            movementManager.FixedUpdate();
            HandleInput();
        }

        private void HandleInput()
        {
            movementManager.SetMovement(moveForward, turnLeft, turnRight);
            moveForward = false;
            turnLeft = false;
            turnRight = false;

            if (shoot)
            {
                Fire();
                if (!lockFire)
                {
                    shoot = false;
                }
            }
        }

        private void Fire()
        {
            ProjectileSpawner.SpawnProjectile();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy();
        }

        public void Reset()
        {
            isAlreadyDestroyed = false;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(true);
        }

        public void Destroy()
        {
            if(isAlreadyDestroyed) return;
            
            isAlreadyDestroyed = true;
            Death?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            InputManager.MoveForwardKeyPressed -= MoveForward;
            InputManager.MoveLeftKeyPressed -= TurnLeft;
            InputManager.MoveRightKeyPressed -= TurnRight;
            InputManager.ShootKeyPressed -= Shoot;
        }

        public void PrepareForRespawn()
        {
            gameObject.SetActive(true);
            if (animationWaitingForRespawn != null)
            {
                animationWaitingForRespawn.Play();
            }
        }
        
    }
}