using UnityEngine;

namespace Data
{
    public class EntityData : ScriptableObject
    {
        [Range(0f, 20f)][Tooltip("Force (speed) at which the entity is launched.")] 
        public float thrust = 10f;
        [Range(0f, 20f)][Tooltip("Optional. Force (speed) at which the entity starts rotating.")] 
        public float torque = 5f;
    }
}