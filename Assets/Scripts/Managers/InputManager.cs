using System;
using UnityEngine;

public static class InputManager
{
    public static Action MoveForwardKeyPressed;
    public static Action MoveLeftKeyPressed;
    public static Action MoveRightKeyPressed;
    public static Action ShootKeyPressed;
    public static Action TeleportationKeyPressed;
    public static Action RestartKeyPressed;

    private static KeyCode[] moveForwardKeys;
    private static KeyCode[] moveLeftKeys;
    private static KeyCode[] moveRightKeys;
    private static KeyCode[] shootKeys;
    private static KeyCode[] teleportationKeys;
    public static KeyCode[] RestartKeys {get; private set;}

    private static bool continuousFire;
    public static bool LockFire { get; private set; }

    public static void SetUp(InputData data)
    {
        moveForwardKeys = data.MoveForwardKeys;
        moveLeftKeys = data.MoveLeftKeys;
        moveRightKeys = data.MoveRightKeys;
        shootKeys = data.ShootKeys;
        teleportationKeys = data.TeleportationKeys;
        RestartKeys = data.RestartKeys;
        continuousFire = data.ContinuousFire;
        LockFire = data.LockFire;
    }

    public static void Update()
    {
        Debug.Log("Updating InputManger");
        foreach (var key in moveForwardKeys)
            if (Input.GetKey(key))
            {
                MoveForwardKeyPressed?.Invoke();
                break;
            }

        foreach (var key in moveLeftKeys)
            if (Input.GetKey(key))
            {
                Debug.Log("MoveLeftKeyPressed");
                MoveLeftKeyPressed?.Invoke();
                break;
            }

        foreach (var key in moveRightKeys)
            if (Input.GetKey(key))
            {
                MoveRightKeyPressed?.Invoke();
                break;
            }

        foreach (var key in shootKeys)
        {
            if (continuousFire)
            {
                if (Input.GetKey(key))
                {
                    Debug.Log("ShootKeyPressed");
                    ShootKeyPressed?.Invoke();
                }
            }
            else
            {
                if (Input.GetKeyDown(key))
                {
                    ShootKeyPressed?.Invoke();
                }
            }

            break;
        }

        foreach (var key in teleportationKeys)
            if (Input.GetKeyDown(key))
            {
                TeleportationKeyPressed?.Invoke();
                break;
            }

        foreach (var key in RestartKeys)
            if (Input.GetKeyDown(key))
            {
                RestartKeyPressed?.Invoke();
                break;
            }
    }
}