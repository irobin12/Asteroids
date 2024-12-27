using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "RockData", menuName = "Data/RockData", order = 1)]
public class RockData : EntityData
{
    [SerializeField] private Rock prefab;
    public Rock Prefab
    {
        get
        {
            Assert.IsNotNull(prefab, $"Prefab is null in {name} data, please assign one.");
            return prefab;
        }
    }

    [Tooltip("Does this rock have to be collected rather than shot at? (If true, spawnedRock won't be used.)")]
    [SerializeField] private bool collectable;
    public bool Collectable  => collectable;

    [Tooltip("Score gained for destroying this rock.")]
    [SerializeField] private int score;
    public int Score => score;

    [Header("Rocks spawned upon destroying this rock")]
    
    [Tooltip("Data for the rock that will be spawned when this one gets destroyed.")]
    [SerializeField] private RockData spawnedRock;
    public RockData SpawnedRock => spawnedRock;

    [Tooltip("Number of rocks to spawn.")] 
    [SerializeField] private int spawnedRocksAmount = 3;
    public int SpawnedRocksAmount => spawnedRocksAmount;

    [Tooltip("Min angle at which the spawned rocks will deviate from this rock.")] 
    [Range(0, 360)]
    [SerializeField] private int minSpawnAngleDeviation = 10;
    public int MinSpawnAngleDeviation => minSpawnAngleDeviation;

    [Tooltip("Max angle at which the spawned rocks will deviate from this rock.")] 
    [Range(0, 360)]
    [SerializeField] private int maxSpawnAngleDeviation = 90;
    public int MaxSpawnAngleDeviation => maxSpawnAngleDeviation;

    [Tooltip("Min velocity multiplier with which the spawned rocks will deviate from this rock, 1 being the same speed as the parent rock.")]
    [Range(0.5f, 1f)]
    [SerializeField] private float minVelocityMultiplier = 0.9f;
    public float MinVelocityMultiplier => minVelocityMultiplier;

    [Tooltip("Max velocity multiplier with which the spawned rocks will deviate from this rock, 1 being the same speed as the parent rock.")]
    [Range(1f, 1.5f)]
    [SerializeField] private float maxVelocityMultiplier = 1.2f;
    public float MaxVelocityMultiplier => maxVelocityMultiplier;
}