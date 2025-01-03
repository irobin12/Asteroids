using UnityEngine;

public class BigEnemy : Enemy
{
    private int minNegativeShootingAngle;
    private int maxNegativeShootingAngle;
    private int minPositiveShootingAngle;
    private int maxPositiveShootingAngle;
    
    // Assuming projectile spawner initially faces forward.
    private const int MinShootingAngle = -180;
    private const int MaxShootingAngle = 180;

    public override void SetUp(EnemyData data)
    {
        base.SetUp(data);
        
        minNegativeShootingAngle = - data.ShootingAnglePadding;
        minPositiveShootingAngle = data.ShootingAnglePadding;
        
        maxNegativeShootingAngle = MinShootingAngle - minNegativeShootingAngle;
        maxPositiveShootingAngle = MaxShootingAngle - minPositiveShootingAngle;
    }
    
    protected override void RotateProjectileSpawner()
    {
        CreateAndClampAngle(out var angle);

        var rotation = Quaternion.Euler(0, 0, angle);
        
        ProjectileSpawner.RotateSpawnPoint(rotation);
    }

    private void CreateAndClampAngle(out int angle)
    {
        angle = Random.Range(maxNegativeShootingAngle, maxPositiveShootingAngle);
        if (angle > minNegativeShootingAngle && angle <= 0)
        {
            angle = minNegativeShootingAngle;
        }
        else if (angle < minPositiveShootingAngle && angle >= 0)
        {
            angle = minPositiveShootingAngle;
        }
    }
}