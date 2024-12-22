using System;
using Data;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(ProjectileSpawner), typeof(MovementManager))]
    public class Player : MonoBehaviour, IEntity<PlayerData>, IDestroyable
    {
        public Action Destroyed;

        private ProjectileSpawner projectileSpawner;
        private MovementManager movementManager;

        private bool moveForward;
        private bool turnLeft;
        private bool turnRight;
        private bool shoot;

        private bool lockFire;
        private PlayerData data;
        
        public void SetUp(PlayerData playerData)
        {
            data = playerData;
            movementManager = GetComponent<MovementManager>();
            movementManager.SetUp(data);
            
            projectileSpawner = GetComponent<ProjectileSpawner>();
            projectileSpawner.SetUp(data.projectileData);
        
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
            projectileSpawner.SpawnProjectile();
        }

        private void OnDestroy()
        {
            InputManager.MoveForwardKeyPressed -= MoveForward;
            InputManager.MoveLeftKeyPressed -= TurnLeft;
            InputManager.MoveRightKeyPressed -= TurnRight;
            InputManager.ShootKeyPressed -= Shoot;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy();
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public void Destroy()
        {
            Destroyed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}