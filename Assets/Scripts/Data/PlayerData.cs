using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
    public class PlayerData : MovingEntityData
    {
        public ProjectileData projectileData;
        
        [Header("Accessibility")]
        [Tooltip("Can the projectile fire key be held down to shoot continuously?")]
        public bool continuousFire;

        [Tooltip("Can the projectile firing be locked to avoid having to hold the key down? " +
                 "(This forces continuous fire on.)")]
        public bool lockFire;
    }
}