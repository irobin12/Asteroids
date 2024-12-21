using System;
using Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(RigidBodyMovementManager))]
public abstract class MovingEntity: MonoBehaviour
{
    public Action<MovingEntity> Death;
    private RigidBodyMovementManager movementManager;
    
    public void Initialize(MovingEntityData movingEntityData)
    {
        movementManager = GetComponent<RigidBodyMovementManager>();
        movementManager.Initialize(movingEntityData);
    }

    protected virtual void FixedUpdate()
    {
        movementManager.FixedUpdate();
    }

    protected virtual void Update()
    {
        movementManager.Update();
    }

    public void SetMovement(bool forward, bool left, bool right)
    {
        movementManager.SetMovement(forward, left, right);
    }

    public abstract void Reset();

    protected void Die()
    {
        Death.Invoke(this);
    }
}