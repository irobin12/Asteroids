using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    public class EntityData : ScriptableObject
    {
        [FormerlySerializedAs("velocity")] [FormerlySerializedAs("thrust")] [Range(0f, 20f)][Tooltip("Force (speed) at which the entity is launched. (Thrust)")] 
        public float launchVelocity = 10f;
        [FormerlySerializedAs("torque")] [Range(0f, 20f)][Tooltip("Optional. Force (speed) at which the entity starts rotating. (Torque)")] 
        public float rotationSpeed = 5f;
    }
}