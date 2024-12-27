public class SmallEnemy : Enemy
{
    private PlayerManager playerManager;

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
    
    protected override void RotateProjectileSpawner()
    {
        var playerPosition = playerManager.GetPlayerPosition();
        ProjectileSpawner.LookAt(playerPosition);
    }
}