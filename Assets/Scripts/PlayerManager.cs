using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Entities;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerManager: MonoBehaviour
{
    public Action PlayerDeath;
    private Player player;
    private PlayerData playerData;
    /// <summary>
    /// Used to set a safe area of respawn.
    /// </summary>
    private readonly HashSet<Collider2D> collidingWith = new();
    
    public void SetUp(PlayerData data)
    {
        playerData = data;
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        player = Instantiate(playerData.prefab);
        if (player != null)
        {
            player.SetUp(playerData);
            player.Death += OnPlayerDeath;
        }
        else
        {
            Debug.LogWarning($"PlayerData needs a prefab with a Player component attached for the game to run!");
        }
    }
    
    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(playerData.respawnTime);
        
        while (collidingWith.Count > 0)
        {
            yield return null;
        }
        
        player.Reset();
    }

    private void OnPlayerDeath()
    {
        PlayerDeath?.Invoke();
    }
    
    private void OnDestroy()
    {
        player.Death -= OnPlayerDeath;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collidingWith.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collidingWith.Remove(other);
    }
}