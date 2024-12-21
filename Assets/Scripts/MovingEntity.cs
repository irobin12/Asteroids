using Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(RigidBodyMovementManager))]
public abstract class MovingEntity: MonoBehaviour
{
    private RigidBodyMovementManager MovementManager;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="thrust">Intensity of the force applied to the entity to move forward.</param>
    /// <param name="torque">Optional. Intensity of the force applied to the entity to rotate left or right.</param>
    public virtual void Initialise(MovingEntityData movingEntityData)
    {
        MovementManager = GetComponent<RigidBodyMovementManager>();
        MovementManager.Initialise(movingEntityData);
    }

    protected virtual void FixedUpdate()
    {
        MovementManager.FixedUpdate();
    }

    protected virtual void Update()
    {
        MovementManager.Update();
    }

    protected void SetMovement(bool forward, bool left, bool right)
    {
        MovementManager.SetMovement(forward, left, right);
    }

    public abstract void Reset();
}