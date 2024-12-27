using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class PlayerManager : MonoBehaviour
{
    public Action PlayerDeath;
    public Action PlayerStarted;

    private Player player;
    private PlayerData playerData;
    
    /// <summary>
    ///     Used to set a safe area of respawn.
    /// </summary>
    private readonly HashSet<Collider2D> collidingWith = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        collidingWith.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collidingWith.Remove(other);
    }

    public void SetUp(PlayerData data)
    {
        playerData = data;
        CreatePlayer();
        InputManager.TeleportationKeyPressed += Teleport;
    }

    private void Teleport()
    {
        StartCoroutine(MovePlayerAndWait());
    }

    private IEnumerator MovePlayerAndWait()
    {
        player.gameObject.SetActive(false);

        var positionX = Random.Range(ScreenManager.WorldMinCorner.x, ScreenManager.WorldMaxCorner.x);
        var positionY = Random.Range(ScreenManager.WorldMinCorner.y, ScreenManager.WorldMaxCorner.y);
        player.transform.position = new Vector3(positionX, positionY, 0);
        yield return new WaitForSeconds(playerData.TeleportationTime);

        player.gameObject.SetActive(true);
    }

    private void CreatePlayer()
    {
        Assert.IsNotNull(playerData.Prefab, "PlayerData needs a prefab with a Player component attached for the game to run.");
        
        player = Instantiate(playerData.Prefab);
        player.SetUp(playerData);
        player.Death += OnPlayerDeath;
    }

    public void ResetPlayerFromStart()
    {
        player.ProjectileSpawner.ReleaseAll();
        SetFromStart();
    }

    public void SetFromStart()
    {
        PlayerStarted?.Invoke();
        player.SetFromStart();
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(playerData.RespawnTime);

        while (collidingWith.Count > 0) yield return null;

        SetFromStart();
    }

    public Vector2 GetPlayerPosition()
    {
        return player.transform.position;
    }

    private void OnPlayerDeath()
    {
        PlayerDeath?.Invoke();
    }
    
    private void OnDestroy()
    {
        if (player)
        {
            player.Death -= OnPlayerDeath;
        }
    }
}