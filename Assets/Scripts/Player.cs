using Data;
using UnityEngine;

[RequireComponent(typeof(ProjectileSpawner))]
public class Player: MovingEntity
{
    private ProjectileSpawner projectileSpawner;

    private bool upKeyPressed;
    private bool leftKeyPressed;
    private bool rightKeyPressed;
    private bool ctrlKeyPressed;

    private bool lockFire;

    public void Initialize(PlayerData playerData, ProjectileData projectileData)
    {
        base.Initialize(playerData);
        lockFire = playerData.lockFire;

        projectileSpawner = GetComponent<ProjectileSpawner>();
        projectileSpawner.Initialize(projectileData);
        
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleInput();
    }

    public override void Reset() { }

    private void HandleInput()
    {
        SetMovement(upKeyPressed, leftKeyPressed, rightKeyPressed);
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
}