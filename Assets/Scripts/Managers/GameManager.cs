using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton
{
    private SpawnManager spawnManager;

    new private void Awake()
    {
        base.Awake();
        spawnManager = FindFirstObjectByType<SpawnManager>();

        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager not found in the scene.");
        }
    }

    private void Start()
    {
        if (spawnManager != null)
        {
            // Set the player spawn position
            Vector3 playerSpawnPosition = new Vector3(0, 0, 0);
            GameObject playerSpawnPoint = new GameObject("PlayerSpawnPoint");
            playerSpawnPoint.transform.position = playerSpawnPosition;
            spawnManager.Spawn("Astronaut guy 1", 1, 0, playerSpawnPoint.transform);
        }
    }

    private Transform CreateSpawnPoint(Vector3 position)
    {
        GameObject spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.position = position;
        return spawnPoint.transform;
    }
}
