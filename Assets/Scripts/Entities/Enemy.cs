using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ProjectileSpawner), typeof(MovementManager))]
public class Enemy : MonoBehaviour, IEntity<EnemyData>, IDestroyable
{
    private const int AngleFacingRight = 270;
    private const int AngleFacingLeft = 90;
    public Action<Enemy> Destroyed;
    public Action<Enemy> ReachedEndOfScreen;
    
    /// <summary>
    /// Enemies spawn either on the left or the right side of the screen.
    /// </summary>
    private bool spawnedOnLeftSide;

    public EnemyData Data {get; private set;}
    private MovementManager movementManager;

    private float horizontalMidPoint;
    private bool crossedHorizontalMidPoint = false;

    public void SetUp(EnemyData data)
    {
        Data = data;
        horizontalMidPoint = (ScreenManager.WorldMaxCorner.x - math.abs(ScreenManager.WorldMinCorner.x)) / 2f;
        movementManager = GetComponent<MovementManager>();
        movementManager.ScreenBoundaryCrossed += OnScreenBoundaryCrossed;
        movementManager.SetUp(false, Data.launchVelocity);
    }

    private void OnScreenBoundaryCrossed()
    {
        // Bit of a hack because of execution order issue, can be used to make enemy go diagonally once midway anyway
        if (crossedHorizontalMidPoint)
        {
            ReachedEndOfScreen?.Invoke(this);
        }
    }

    public void SetSpawnPosition()
    {
        crossedHorizontalMidPoint = false;
        var yPosition = Random.Range(ScreenManager.WorldMinCorner.y, ScreenManager.WorldMaxCorner.y);
        
        spawnedOnLeftSide = Random.Range(0, 2) == 0;
        var xPosition = spawnedOnLeftSide ? ScreenManager.WorldMinCorner.x : ScreenManager.WorldMaxCorner.x;
        
        transform.position = new Vector3(xPosition, yPosition, 0);
        var rotationZ = spawnedOnLeftSide ? AngleFacingRight : AngleFacingLeft;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        
        movementManager.SetMovement(true);
    }

    public void SetFromStart()
    {
    }
    
    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    private void FixedUpdate()
    {
        if ((spawnedOnLeftSide && transform.position.x >= horizontalMidPoint) || (!spawnedOnLeftSide && transform.position.x <= horizontalMidPoint))
        {
            crossedHorizontalMidPoint = true;
        }
    }

    private void OnDestroy()
    {
        movementManager.ScreenBoundaryCrossed -= OnScreenBoundaryCrossed;
    }
}