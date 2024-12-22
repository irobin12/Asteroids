using Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidBodyMovementManager: MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private float thrust;
    private float torque;
    
    private bool canMoveForward;
    private bool canMoveLeft;
    private bool canMoveRight;

    public void SetUp(MovingEntityData data)
    {
        thrust = data.thrust;
        torque = data.torque;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (canMoveForward)
        {
            MoveForward();
            canMoveForward = false;
        }

        if (canMoveLeft)
        {
            TurnLeft();
            canMoveLeft = false;
        }

        if (canMoveRight)
        {
            TurnRight();
            canMoveRight = false;
        }
    }

    public void Update()
    {
        CrossOverScreenBoundaries();
    }

    public void SetMovement(bool forward, bool left, bool right)
    {
        canMoveForward = forward;
        canMoveLeft = left;
        canMoveRight = right;
    }

    private void MoveForward()
    {
        rigidbody2D.AddForce(transform.up * thrust, ForceMode2D.Force);
    }

    private void TurnLeft()
    {
        Rotate(torque);
    }

    private void TurnRight()
    {
        Rotate(-torque);
    }

    private void Rotate(float rotation)
    {
        transform.Rotate(transform.forward * rotation);
    }

    private void CrossOverScreenBoundaries()
    {
        if (transform.position.x > ScreenManager.WorldMaxCorner.x)
        {
            transform.position = new Vector2(ScreenManager.WorldMinCorner.x, transform.position.y);
        }
        else if (transform.position.x < ScreenManager.WorldMinCorner.x)
        {
            transform.position = new Vector2(ScreenManager.WorldMaxCorner.x, transform.position.y);
        }

        if (transform.position.y > ScreenManager.WorldMaxCorner.y)
        {
            transform.position = new Vector2(transform.position.x, ScreenManager.WorldMinCorner.y);
        }
        else if (transform.position.y < ScreenManager.WorldMinCorner.y)
        {
            transform.position = new Vector2(transform.position.x, ScreenManager.WorldMaxCorner.y);
        }
    }
}