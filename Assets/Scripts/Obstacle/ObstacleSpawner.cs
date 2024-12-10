using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public List<Transform> spawnPoints; // List of spawn points for obstacles
    public GameObject[] obstaclePrefabs; // Array to hold 3 different obstacle prefabs
    public int maxActiveObstacles = 5; // Maximum number of obstacles at a time
    public float obstacleLifespan = 5f; // Fixed lifespan for each obstacle (5 seconds)

    [Header("Spawn Interval Settings")]
    public float minSpawnInterval = 1f; // Minimum time between spawns
    public float maxSpawnInterval = 3f; // Maximum time between spawns

    private List<GameObject> activeObstacles = new List<GameObject>();

    private void Start()
    {
        if (spawnPoints.Count == 0 || obstaclePrefabs.Length == 0)
        {
            Debug.LogError("Spawn points or obstacle prefabs are not assigned.");
            return;
        }

        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            if (activeObstacles.Count < maxActiveObstacles)
            {
                // Randomly pick a spawn point and obstacle prefab
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

                // Spawn the obstacle
                GameObject spawnedObstacle = Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);
                activeObstacles.Add(spawnedObstacle);

                Debug.Log($"Spawned obstacle at {spawnPoint.name}");

                // Remove the obstacle after its fixed lifespan
                StartCoroutine(RemoveObstacleAfterTime(spawnedObstacle, obstacleLifespan));
            }

            // Randomized spawn interval
            float spawnDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator RemoveObstacleAfterTime(GameObject obstacle, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (obstacle != null)
        {
            Debug.Log($"Obstacle {obstacle.name} destroyed after {duration} seconds.");
            activeObstacles.Remove(obstacle);
            Destroy(obstacle);
        }
    }
}
