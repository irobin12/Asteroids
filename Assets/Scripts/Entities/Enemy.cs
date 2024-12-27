using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ProjectileSpawner), typeof(MovementManager))]
public abstract class Enemy : MonoBehaviour, IEntity<EnemyData>, IDestroyable
{
    public Action<Enemy> Destroyed;
    public Action<Enemy> ReachedEndOfScreen;
    
    public EnemyData Data {get; private set;}
    protected ProjectileSpawner ProjectileSpawner;
    private MovementManager movementManager;
    
    /// <summary>
    /// Enemies spawn either on the left or the right side of the screen.
    /// </summary>
    private bool spawnedOnLeftSide;
    private float horizontalMidPoint;
    private bool crossedHorizontalMidPoint;
    private float timeSinceLastShot;
    
    private const int AngleFacingRight = 270;
    private const int AngleFacingLeft = 90;

    public virtual void SetUp(EnemyData data)
    {
        Data = data;
        horizontalMidPoint = (ScreenManager.WorldMaxCorner.x - math.abs(ScreenManager.WorldMinCorner.x)) / 2f;
        
        movementManager = GetComponent<MovementManager>();
        movementManager.ScreenBoundaryCrossed += OnScreenBoundaryCrossed;
        movementManager.SetUp(false, Data.LaunchVelocity);
        
        ProjectileSpawner = GetComponent<ProjectileSpawner>();
        ProjectileSpawner.SetUp(data.ProjectileData);
    }

    private void OnScreenBoundaryCrossed()
    {
        // Bit of a hack because of execution order issue, can be used to make enemy go diagonally once midway anyway
        if (crossedHorizontalMidPoint)
        {
            ReachedEndOfScreen?.Invoke(this);
        }
    }

    public void SetActive(bool active, bool removeProjectiles = true)
    {
        gameObject.SetActive(active);
        if (!active && removeProjectiles)
        {
            ProjectileSpawner.ReleaseAll();
        }
    }

    public void SetFromStart()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            if (other.gameObject.TryGetComponent(out Projectile projectile)) projectile.Destroy();

            Destroy();
        }
    }
    
    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    private void FixedUpdate()
    {
        movementManager.FixedUpdate();
        
        if ((spawnedOnLeftSide && transform.position.x >= horizontalMidPoint) || (!spawnedOnLeftSide && transform.position.x <= horizontalMidPoint))
        {
            crossedHorizontalMidPoint = true;
        }
        
        TryShooting();
    }

    private void TryShooting()
    {
        timeSinceLastShot += Time.fixedTime;
        
        if (timeSinceLastShot >= Data.ProjectileData.Cooldown)
        {
            RotateProjectileSpawner();
            ProjectileSpawner.SpawnProjectile();
            timeSinceLastShot = 0f;
        }
    }

    protected abstract void RotateProjectileSpawner();

    private void OnDestroy()
    {
        if (movementManager)
        {
            movementManager.ScreenBoundaryCrossed -= OnScreenBoundaryCrossed;
        }
    }
}