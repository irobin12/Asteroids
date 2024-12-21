using System;
using UnityEngine;

public static class InputManager
{
    public static Action UpKeyPressed;
    public static Action LeftKeyPressed;
    public static Action RightKeyPressed;
        
    public static void Refresh()
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
    }
}
