using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // Customer prefab to spawn
    public List<Transform> spawnNodes; // List of node positions where customers can spawn
    public int maxCustomersToSpawn = 10; // Maximum number of customers to spawn
    public float minSpawnCooldown = 2f; // Minimum cooldown between spawns
    public float maxSpawnCooldown = 5f; // Maximum cooldown between spawns

    private int spawnedCustomers = 0; // Track the number of spawned customers

    private void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (spawnedCustomers < maxCustomersToSpawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnCooldown, maxSpawnCooldown));

            Transform availableNode = GetAvailableNode();
            if (availableNode != null)
            {
                SpawnCustomer(availableNode);
            }
        }

        Debug.Log("Customer spawning complete.");
    }

    private Transform GetAvailableNode()
    {
        foreach (Transform node in spawnNodes)
        {
            if (node.childCount == 0) // Check if the node is available
            {
                return node;
            }
        }
        return null; // No available nodes
    }

    private void SpawnCustomer(Transform node)
    {
        GameObject customer = Instantiate(customerPrefab, node.position, Quaternion.identity);
        spawnedCustomers++;
        Debug.Log($"Customer spawned at node: {node.name}");
    }
}
