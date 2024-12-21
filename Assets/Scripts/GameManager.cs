using Data;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager: MonoBehaviour
{
    [FormerlySerializedAs("gameMovingEntityData")] [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;

    private Player player;
    private RockSpawner rockSpawner;

    private void Awake()
    {
        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
        InputManager.Initialize(gameData.player);
        
        if (!TryGetComponent(out rockSpawner))
        {
            rockSpawner = gameObject.AddComponent<RockSpawner>();
        }
    }

    private void Start()
    {
        CreatePlayer();
        CreateRocks();
    }

    private void CreatePlayer()
    {
        var playerInstance = Instantiate(gameData.player.prefab);
        if (playerInstance.TryGetComponent(out player))
        {
            player.Initialize(gameData.player, gameData.player.projectileData);
        }
        else
        {
            Debug.LogError($"The MovingEntity assigned to the {nameof(MovingEntityData.prefab)} field of " +
                           $"{nameof(gameData.player)} in {nameof(gameData)} must have a {nameof(Player)} " +
                           $"component attached for the game to run. ");
        }
    }

    private void CreateRocks()
    {
        rockSpawner.Initialize(gameData.rock);
        rockSpawner.SpawnFirstRocks(gameData.levels[0].startingRocksToSpawn);
    }

    private void Update()
    {
        InputManager.Update();
    }
}