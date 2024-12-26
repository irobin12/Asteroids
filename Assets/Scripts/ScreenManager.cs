using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Assuming that the window cannot get resized once the game is launched (as is currently set in the Project Settings/PLayer).
/// </summary>
public static class ScreenManager
{
    public static Vector2 WorldMaxCorner { get; private set; }
    public static Vector2 WorldMinCorner { get; private set; }

    public static void SetBoundariesInWorldPoint(Vector2 screenSize, Camera mainCamera)
    {
        var minCorner = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var maxCorner = mainCamera.ScreenToWorldPoint(new Vector3(screenSize.x, screenSize.y, 0));
        WorldMinCorner = new Vector2(minCorner.x, minCorner.y);
        WorldMaxCorner = new Vector2(maxCorner.x, maxCorner.y);
    }
}