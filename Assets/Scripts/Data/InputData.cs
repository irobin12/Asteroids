using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "InputData", menuName = "Data/InputData", order = 1)]
    public class InputData : ScriptableObject
    {
        public KeyCode[] moveForwardKeys;
        public KeyCode[] moveLeftKeys;
        public KeyCode[] moveRightKeys;
        public KeyCode[] shootKeys;
        public KeyCode[] teleportationKeys;
        public KeyCode[] restartKeys;
        
        [Header("Accessibility")]
        [Tooltip("Can the projectile fire key be held down to shoot continuously?")]
        public bool continuousFire;

        [Tooltip("Can the projectile firing be locked to avoid having to hold the key down? " +
                 "(This forces continuous fire on.)")]
        public bool lockFire;
    }
}