using System;
using UnityEngine;

public static class InputManager
{
    public static Action UpKeyPressed;
    public static Action LeftKeyPressed;
    public static Action RightKeyPressed;
    public static Action CtrlKeyPressed;
    public static Action SpaceKeyPressed;
        
    public static void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            UpKeyPressed?.Invoke();
            Debug.Log("Up Pressed");
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            LeftKeyPressed?.Invoke();
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            RightKeyPressed?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            CtrlKeyPressed?.Invoke();
            Debug.Log("Ctrl Key pressed");
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceKeyPressed?.Invoke();
        }
    }
}
