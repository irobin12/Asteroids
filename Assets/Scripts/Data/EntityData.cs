using UnityEngine;

public class EntityData : ScriptableObject
{
    [Range(0f, 100f)] [Tooltip("Force (speed) at which the entity is launched. (Thrust)")]
    [SerializeField] private float launchVelocity = 10f;
    public float LaunchVelocity
    {
        get => launchVelocity;
        set => launchVelocity = value;
    }
}