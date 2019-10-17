using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public const int maxHealth = 3;
    public bool destroyOnDeath;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;

    private PlayerController shooter;
    public float spriteSize = 0.5f;
    
    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int amount, Transform shooter)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Vector2 newPosition = (new Vector2(Random.Range(3.4f + spriteSize, 12.8f - spriteSize),
                    Random.Range(0.0f + spriteSize, 7.2f - spriteSize)));

                transform.position = newPosition;
                currentHealth = maxHealth;

            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();

                this.shooter = shooter.GetComponent<PlayerController>();
                CmdUpdateScore();             
            }
        }
    }

    void OnChangeHealth(int health)
    {
        RectTransform parentBar = (RectTransform)healthBar.parent;
        float barScale = parentBar.rect.width / maxHealth;
        healthBar.sizeDelta = new Vector2(health * barScale, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;

            currentHealth = maxHealth;
        }
    }

    [Command]
    void CmdUpdateScore()
    {
        if (this.transform.GetComponent<PlayerController>() == shooter)
        {
            shooter.score--;
        } else
        {
            shooter.score++;  
        }
        
        StartCoroutine(DelayScoreboardUpdate());
    }

    IEnumerator DelayScoreboardUpdate()
    {
        yield return new WaitForSeconds(0.15f);
        RpcUpdateScoreboard();
    }

    [ClientRpc]
    void RpcUpdateScoreboard()
    {
        GameObject.FindObjectOfType<NetworkManager>().GetComponent<ScoreManager>().UpdateScoreboard();
    }
}