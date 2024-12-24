using Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "RockData", menuName = "Data/RockData", order = 1)]
    public class RockData : EntityData
    {
        public Rock prefab;
        [Tooltip("Does this rock have to be collected rather than shot at? (If true, spawnedRock won't be used.)")]
        public bool collectable;
        
        [Tooltip("Score gained for destroying this rock.")]
        public int score;
        
        [Header("Rocks spawned upon destroying this rock")]
        [Tooltip("Data for the rock that will be spawned when this one gets destroyed.")]
        public RockData spawnedRock;
        
        [Tooltip("Number of rocks to spawn.")]
        public int spawnedRocksAmount = 3;
        
        [Tooltip("Min angle at which the spawned rocks will deviate from this rock.")]
        [Range(0, 360)]
        public int minSpawnAngleDeviation = 10;
        [Tooltip("Max angle at which the spawned rocks will deviate from this rock.")]
        [Range(0, 360)]
        public int maxSpawnAngleDeviation = 90;

        [Tooltip("Min velocity multiplier with which the spawned rocks will deviate from this rock, 1 being the same speed as the parent rock.")] 
        [Range(0.5f, 1f)]
        public float minVelocityMultiplier = 0.9f;
        [Tooltip("Max velocity multiplier with which the spawned rocks will deviate from this rock, 1 being the same speed as the parent rock.")] 
        [Range(1f, 1.5f)]
        public float maxVelocityMultiplier = 1.2f;


    }
}