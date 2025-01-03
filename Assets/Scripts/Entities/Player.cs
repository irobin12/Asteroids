using System;
using UnityEngine;

[RequireComponent(typeof(ProjectileSpawner), typeof(MovementManager))]
public class Player : MonoBehaviour, IEntity<PlayerData>, IDestroyable
{
    public Action Death;
    
    public ProjectileSpawner ProjectileSpawner { get; private set; }
    private MovementManager movementManager;

    private PlayerData data;
    private bool isAlreadyDestroyed; // To avoid calling Destroyed twice if hit by two enemies simultaneously
    private bool lockFire;
    private bool moveForward;
    private bool shoot;
    private bool turnLeft;
    private bool turnRight;

    private void FixedUpdate()
    {
        movementManager.FixedUpdate();
        HandleInput();
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
        if (other.gameObject.CompareTag("Collectible"))
        {
            if (other.gameObject.TryGetComponent(out Rock rock))
            {
                rock.Collect();
            }
        }
        else
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        if (isAlreadyDestroyed) return;

        isAlreadyDestroyed = true;
        Death?.Invoke();
        gameObject.SetActive(false);
    }

    public void SetUp(PlayerData playerData)
    {
        data = playerData;
        movementManager = GetComponent<MovementManager>();
        movementManager.SetUp(true, data.LaunchVelocity, data.RotationSpeed);

        ProjectileSpawner = GetComponent<ProjectileSpawner>();
        ProjectileSpawner.SetUp(data.ProjectileData);

        lockFire = InputManager.LockFire;

        InputManager.MoveForwardKeyPressed += MoveForward;
        InputManager.MoveLeftKeyPressed += TurnLeft;
        InputManager.MoveRightKeyPressed += TurnRight;
        InputManager.ShootKeyPressed += Shoot;
    }

    public void SetFromStart()
    {
        movementManager.ResetVelocity();
        isAlreadyDestroyed = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
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
            shoot = !shoot;
        else
            shoot = true;
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
            if (!lockFire) shoot = false;
        }
    }

    private void Fire()
    {
        ProjectileSpawner.SpawnProjectile();
    }
}