using System;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public struct ScreenData
    {
        // public readonly Player Player;
        // public readonly Rock Rock;
        // public readonly Enemy Enemy;
        public readonly Vector2 ScreenSize;
        public readonly int ScreenPadding;
        public readonly Camera MainCamera;

        public ScreenData(Vector2 screenSize, int screenPadding, Camera mainCamera)
        {
            // Player = data.playerPrefab;
            // Rock = data.rock;
            // Enemy = data.enemy;
            ScreenSize = screenSize;
            ScreenPadding = screenPadding;
            MainCamera = mainCamera;
        }
    }

    /// <summary>
    /// Only working when window is not resizable (it's currently turned off in Player Preferences).
    /// </summary>
    public static Vector2 ScreenSize {get; private set;}
    // public float ScreenWidth {get; private set;}
    // public float ScreenHeight {get; private set; }
    
    [SerializeField] private GameData gameData;
    [SerializeField] private Camera mainCamera;

    // private Data data;
    private Player player;
    private ScreenData screenData;

    private void Awake()
    {
        ScreenSize = new Vector2(Screen.width, Screen.height);
        // ScreenWidth = Screen.width;
        // ScreenHeight = Screen.height;
        // data = new Data(gameData);
        screenData = new ScreenData(ScreenSize, gameData.screenPadding, mainCamera);
        Debug.Log("screen size is" + ScreenSize);
    }

    private void Start()
    {
        player = Instantiate(gameData.playerPrefab);
        player.Initialise(gameData.playerThrust, gameData.playerTorque, screenData);
    }

    private void FixedUpdate()
    {
        InputManager.Refresh();
    }

    private void Update()
    {
        player.OnUpdate();
    }
}