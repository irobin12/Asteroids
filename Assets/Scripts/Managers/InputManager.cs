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

    public static InputData Data { get; private set; }

    public static void SetUp(InputData data)
    {
        Data = data;
    }

    public static void Update()
    {
        foreach (var key in Data.moveForwardKeys)
            if (Input.GetKey(key))
            {
                MoveForwardKeyPressed?.Invoke();
                break;
            }

        foreach (var key in Data.moveLeftKeys)
            if (Input.GetKey(key))
            {
                MoveLeftKeyPressed?.Invoke();
                break;
            }

        foreach (var key in Data.moveRightKeys)
            if (Input.GetKey(key))
            {
                MoveRightKeyPressed?.Invoke();
                break;
            }

        foreach (var key in Data.shootKeys)
        {
            if (Data.continuousFire)
            {
                if (Input.GetKey(key)) ShootKeyPressed?.Invoke();
            }
            else
            {
                if (Input.GetKeyDown(key)) ShootKeyPressed?.Invoke();
            }

            break;
        }

        foreach (var key in Data.teleportationKeys)
            if (Input.GetKeyDown(key))
            {
                TeleportationKeyPressed?.Invoke();
                break;
            }

        foreach (var key in Data.restartKeys)
            if (Input.GetKeyDown(key))
            {
                RestartKeyPressed?.Invoke();
                break;
            }
    }
}