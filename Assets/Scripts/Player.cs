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

    public void Initialise(PlayerData playerData, ProjectileData projectileData)
    {
        base.Initialise(playerData);

        projectileSpawner = GetComponent<ProjectileSpawner>();
        projectileSpawner.Initialise(projectileData);
        
        InputManager.UpKeyPressed += () => upKeyPressed = true;
        InputManager.LeftKeyPressed += () => leftKeyPressed = true;
        InputManager.RightKeyPressed += () => rightKeyPressed = true;
        InputManager.CtrlKeyPressed += () => ctrlKeyPressed = true;
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
            ctrlKeyPressed = false;
        }
    }

    private void Fire()
    {
        projectileSpawner.SpawnProjectile();
    }
}