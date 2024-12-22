using Data;
using UnityEngine;

public class Rock: MovingEntity
{
    public RockData Data { get; private set; }
    
    public void SetUp(RockData rockData, bool forward)
    {
        Data = rockData;
        SetMovement(forward, false, false);
    }
    
    public override void Reset()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Die();
        }
    }
}