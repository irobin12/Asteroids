using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player: Entity
{
    private Rigidbody2D rigidbody2D;
    private float thrust;
    private float torque;
    private GameManager.ScreenData screenData;

    private Vector2 worldMinCorner;
    private Vector2 worldMaxCorner;
    
    public void Initialise(float thrust, float torque, GameManager.ScreenData screenData)
    {
        this.thrust = thrust;
        this.torque = torque;
        this.screenData = screenData;

        var minCorner = this.screenData.MainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var maxCorner = this.screenData.MainCamera.ScreenToWorldPoint(new Vector3(screenData.ScreenSize.x, screenData.ScreenSize.y, 0));
        worldMinCorner = new Vector2(minCorner.x, minCorner.y);
        worldMaxCorner = new Vector2(maxCorner.x, maxCorner.y);
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        InputManager.UpKeyPressed += MoveForward;
        InputManager.LeftKeyPressed += TurnLeft;
        InputManager.RightKeyPressed += TurnRight;
    }

    private void MoveForward()
    {
        rigidbody2D.AddRelativeForce(Vector2.up * thrust);
    }

    private void CrossOverScreenBoundaries()
    {
        if (transform.position.x > worldMaxCorner.x)
        {
            transform.position = new Vector2(worldMinCorner.x, transform.position.y);
        }
        else if (transform.position.x < worldMinCorner.x)
        {
            transform.position = new Vector2(worldMaxCorner.x, transform.position.y);
        }

        if (transform.position.y > worldMaxCorner.y)
        {
            transform.position = new Vector2(transform.position.x, worldMinCorner.y);
        }
        else if (transform.position.y < worldMinCorner.y)
        {
            transform.position = new Vector2(transform.position.x, worldMaxCorner.y);
        }
    }

    private void TurnLeft()
    {
        Rotate(torque);
    }

    private void TurnRight()
    {
        Rotate(-torque);
    }

    private void Rotate(float torque)
    {
        transform.Rotate(transform.forward * torque);
    }

    public override void OnUpdate()
    {
        CrossOverScreenBoundaries();
    }
}