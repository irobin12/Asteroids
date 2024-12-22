using System;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

public static class InputManager
{
    public static Action MoveForwardKeyPressed;
    public static Action MoveLeftKeyPressed;
    public static Action MoveRightKeyPressed;
    public static Action ShootKeyPressed;
    public static Action TeleportationKeyPressed;
    
    public static InputData Data {get; private set;}
    
    public static void SetUp(InputData data)
    {
        Data = data;
    }
    
    public static void Update()
    {
        foreach (KeyCode keyCode in Data.moveForwardKeys)
        {
            if (Input.GetKey(keyCode))
            {
                MoveForwardKeyPressed?.Invoke();
                break;
            }
        }

        foreach (KeyCode keyCode in Data.moveLeftKeys)
        {
            if (Input.GetKey(keyCode))
            {
                MoveLeftKeyPressed?.Invoke();
                break;
            }
        }

        foreach (KeyCode keyCode in Data.moveRightKeys)
        {
            if (Input.GetKey(keyCode))
            {
                MoveRightKeyPressed?.Invoke();
                break;
            }
        }
        
        foreach (KeyCode keyCode in Data.shootKeys)
        {
            if (Data.continuousFire)
            {
                if (Input.GetKey(keyCode))
                {
                    ShootKeyPressed?.Invoke();
                }
            }
            else
            {
                if (Input.GetKeyDown(keyCode))
                {
                    ShootKeyPressed?.Invoke();
                }
            }
            
            break;
        }

        // if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)
        //         || (Data.continuousFire && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        // {
        //     ShootKeyPressed?.Invoke();
        // }

        foreach (KeyCode keyCode in Data.teleportationKeys)
        {
            if (Input.GetKey(keyCode))
            {
                TeleportationKeyPressed?.Invoke();
                break;
            }
        }
    }
}
