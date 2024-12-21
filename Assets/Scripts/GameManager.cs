using Data;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager: MonoBehaviour
{
    [FormerlySerializedAs("gameMovingEntityData")] [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;

    private Player player;

    private void Awake()
    {
        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
    }

    private void Start()
    {
        var playerInstance = Instantiate(gameData.player.prefab);
        
        if (playerInstance.TryGetComponent(out player))
        {
            player.Initialise(gameData.player, gameData.player.projectileData);
        }
        else
        {
            Debug.LogError($"The MovingEntity assigned to the {nameof(MovingEntityData.prefab)} field of " +
                           $"{nameof(gameData.player)} in {nameof(gameData)} must have a {nameof(Player)} " +
                           $"component attached for the game to run. ");
        }
    }

    private void Update()
    {
        InputManager.Update();
    }
}