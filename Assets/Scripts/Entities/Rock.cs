using System;
using UnityEngine;

[RequireComponent(typeof(MovementManager))]
public class Rock : MonoBehaviour, IEntity<RockData>, IDestroyable, IPoolable
{
    public Action<Rock> Destroyed;
    private MovementManager movementManager;
    public Action<Rock> Released;

    public RockData Data { get; private set; }

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

    public void SetUp(RockData data)
    {
        Data = data;
        movementManager = GetComponent<MovementManager>();
        movementManager.SetUp(Data.launchVelocity);
        movementManager.SetMovement(true, false, false);
    }

    public void SetFromStart()
    {
    }

    public void Release()
    {
        Released?.Invoke(this);
    }
}