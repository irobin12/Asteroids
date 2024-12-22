using System;
using Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class MovingEntity: MonoBehaviour
{
    public Action<MovingEntity> Death;
    private RigidBodyMovementManager movementManager;
    
    public void SetUp(MovingEntityData movingEntityData)
    {
        if (!TryGetComponent(out movementManager))
        {
            movementManager = gameObject.AddComponent<RigidBodyMovementManager>();
        }
        movementManager.SetUp(movingEntityData);
    }

    protected virtual void FixedUpdate()
    {
        movementManager.FixedUpdate();
    }

    protected virtual void Update()
    {
        movementManager.Update();
    }

    public virtual void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
    
    protected void SetMovement(bool forward, bool left, bool right)
    {
        movementManager.SetMovement(forward, left, right);
    }

    protected virtual void Die()
    {
        Death?.Invoke(this);
    }
}