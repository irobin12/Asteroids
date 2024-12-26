using System;
using UnityEngine;

[RequireComponent(typeof(ProjectileSpawner), typeof(MovementManager))]
public class Player : MonoBehaviour, IEntity<PlayerData>, IDestroyable
{
    [SerializeField] private Animation animationWaitingForRespawn;
    private PlayerData data;

    public Action Death;
    private bool isAlreadyDestroyed; // To avoid calling Destroyed twice if hit by two enemies simultaneously

    private bool lockFire;

    private bool moveForward;

    private MovementManager movementManager;
    private bool shoot;
    private bool turnLeft;
    private bool turnRight;
    public ProjectileSpawner ProjectileSpawner { get; private set; }

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
        Destroy();
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
        movementManager.SetUp(data.launchVelocity, data.rotationSpeed);

        ProjectileSpawner = GetComponent<ProjectileSpawner>();
        ProjectileSpawner.SetUp(data.projectileData);

        lockFire = InputManager.Data.lockFire;

        InputManager.MoveForwardKeyPressed += MoveForward;
        InputManager.MoveLeftKeyPressed += TurnLeft;
        InputManager.MoveRightKeyPressed += TurnRight;
        InputManager.ShootKeyPressed += Shoot;
    }

    public void SetFromStart()
    {
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

    public void PrepareForRespawn()
    {
        gameObject.SetActive(true);
        if (animationWaitingForRespawn != null) animationWaitingForRespawn.Play();
    }
}