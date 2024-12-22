using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "RockData", menuName = "Data/RockData", order = 1)]
    public class RockData : MovingEntityData
    {
        [Tooltip("Data for the rock that will be spawned when this one gets destroyed.")]
        public RockData spawnedRock;

        [Tooltip("Score gained for destroying this rock.")]
        public int score;
    }
}