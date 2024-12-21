using UnityEngine;

public class GameManager: MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;

    private Player player;

    private void Awake()
    {
        ScreenManager.SetBoundariesInWorldPoint(new Vector2(Screen.width, Screen.height), mainCamera);
    }

    private void Start()
    {
        player = Instantiate(gameData.playerPrefab);
        player.Initialise(gameData.playerThrust, gameData.playerTorque);
    }

    private void Update()
    {
        InputManager.Update();
    }
}