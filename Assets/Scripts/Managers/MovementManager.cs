using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementManager : MonoBehaviour
{
    public Action ScreenBoundaryCrossed;
    
    private bool canMoveForward;
    private bool canMoveLeft;
    private bool canMoveRight;
    private new Rigidbody2D rigidbody2D;
    private float thrust;
    private float torque;
    private bool allowScreenBoundaryCrossing; // Mostly to ensure enemies spawn correctly

    public void Update()
    {
        CrossOverScreenBoundaries();
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

    public void ResetVelocity()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;
    }

    public void SetUp(bool allowScreenBoundaryCrossing, float thrust, float torque = 0f)
    {
        this.allowScreenBoundaryCrossing = allowScreenBoundaryCrossing;
        this.thrust = thrust;
        this.torque = torque;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.angularVelocity = 0f;
        rigidbody2D.velocity = Vector2.zero;
    }

    public void SetMovement(bool forward, bool left = false, bool right = false)
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
        var reachedScreenBoundary = false;
        
        if (transform.position.x > ScreenManager.WorldMaxCorner.x)
        {
            CrossScreenBoundary(ScreenManager.WorldMinCorner.x, transform.position.y, out reachedScreenBoundary);
        }
        else if (transform.position.x < ScreenManager.WorldMinCorner.x)
        {
            CrossScreenBoundary(ScreenManager.WorldMaxCorner.x, transform.position.y, out reachedScreenBoundary);
        }

        if (transform.position.y > ScreenManager.WorldMaxCorner.y)
        {
            CrossScreenBoundary(transform.position.x, ScreenManager.WorldMinCorner.y, out reachedScreenBoundary);

        }
        else if (transform.position.y < ScreenManager.WorldMinCorner.y)
        {
            CrossScreenBoundary(transform.position.x, ScreenManager.WorldMaxCorner.y, out reachedScreenBoundary);
        }

        if (reachedScreenBoundary)
        {
            ScreenBoundaryCrossed?.Invoke();
        }
    }

    private void CrossScreenBoundary(float x, float y, out bool reachedScreenBoundary)
    {
        if (allowScreenBoundaryCrossing)
        {
            transform.position = new Vector2(x, y);
        }
        reachedScreenBoundary = true;
    }
}