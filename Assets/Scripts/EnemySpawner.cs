using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies;

    public float spriteSize = 0.5f;

    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(3.4f + spriteSize, 12.8f - spriteSize),
                Random.Range(0.0f + spriteSize, 7.2f - spriteSize),
                0.0f);

            var spawnRotation = Quaternion.Euler(
                0.0f,
                0.0f,
                0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }
}
