using Entities;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
    public class PlayerData : EntityData
    {
        public Player prefab;
        public ProjectileData projectileData;
        [Tooltip("Player respawn countdown time in seconds.")]
        public float respawnTime = 2f;
    }
}