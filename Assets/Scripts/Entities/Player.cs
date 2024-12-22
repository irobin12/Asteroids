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

        private bool upKeyPressed;
        private bool leftKeyPressed;
        private bool rightKeyPressed;
        private bool ctrlKeyPressed;

        private bool lockFire;
        private PlayerData data;
        
        public void SetUp(PlayerData playerData)
        {
            data = playerData;
            movementManager = GetComponent<MovementManager>();
            movementManager.SetUp(data);
            
            projectileSpawner = GetComponent<ProjectileSpawner>();
            projectileSpawner.SetUp(data.projectileData);
        
            lockFire = data.lockFire;
        
            InputManager.UpKeyPressed += OnUpKeyPressed;
            InputManager.LeftKeyPressed += OnLeftKeyPressed;
            InputManager.RightKeyPressed += OnRightKeyPressed;
            InputManager.CtrlKeyPressed += OnCtrlKeyPressed;
        }

        private void OnUpKeyPressed()
        {
            upKeyPressed = true;
        }

        private void OnLeftKeyPressed()
        {
            leftKeyPressed = true;
        }

        private void OnRightKeyPressed()
        {
            rightKeyPressed = true;
        }

        private void OnCtrlKeyPressed()
        {
            if (lockFire)
            {
                ctrlKeyPressed = !ctrlKeyPressed;
            }
            else
            {
                ctrlKeyPressed = true;
            }
        }

        private void FixedUpdate()
        {
            movementManager.FixedUpdate();
            HandleInput();
        }

        private void HandleInput()
        {
            movementManager.SetMovement(upKeyPressed, leftKeyPressed, rightKeyPressed);
            upKeyPressed = false;
            leftKeyPressed = false;
            rightKeyPressed = false;

            if (ctrlKeyPressed)
            {
                Fire();
                if (!lockFire)
                {
                    ctrlKeyPressed = false;
                }
            }
        }

        private void Fire()
        {
            projectileSpawner.SpawnProjectile();
        }

        private void OnDestroy()
        {
            InputManager.UpKeyPressed -= OnUpKeyPressed;
            InputManager.LeftKeyPressed -= OnLeftKeyPressed;
            InputManager.RightKeyPressed -= OnRightKeyPressed;
            InputManager.CtrlKeyPressed -= OnCtrlKeyPressed;
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