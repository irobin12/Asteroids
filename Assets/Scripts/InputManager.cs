using System;
using Data;
using UnityEngine;

public static class InputManager
{
    public static Action UpKeyPressed;
    public static Action LeftKeyPressed;
    public static Action RightKeyPressed;
    public static Action CtrlKeyPressed;
    public static Action SpaceKeyPressed;
    
    private static PlayerData playerData;

    public static void SetUp(PlayerData playerData)
    {
        InputManager.playerData = playerData;
    }
    
    public static void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            UpKeyPressed?.Invoke();
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            LeftKeyPressed?.Invoke();
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            RightKeyPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)
                || (playerData.continuousFire && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            CtrlKeyPressed?.Invoke();
        }
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceKeyPressed?.Invoke();
        }
    }
}
