using Entities;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "RockData", menuName = "Data/RockData", order = 1)]
    public class RockData : EntityData
    {
        public Rock prefab;
        [Tooltip("Does this rock have to be collected rather than shot at? (If true, spawnedRock won't be used.)")]
        public bool collectable;
        
        [Tooltip("Data for the rock that will be spawned when this one gets destroyed.")]
        public RockData spawnedRock;

        [Tooltip("Score gained for destroying this rock.")]
        public int score;
    }
}