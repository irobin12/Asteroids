using UnityEngine;

public static class Utils
{
    public static void TryAddComponent<T>(this GameObject gameObject, out T component) where T : Component
    {
        if (!gameObject.TryGetComponent(out component))
        {
            component = gameObject.AddComponent<T>();
        }
    }
}