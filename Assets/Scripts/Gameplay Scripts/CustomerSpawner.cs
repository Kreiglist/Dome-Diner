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

    private Dictionary<Transform, bool> nodeAvailability = new Dictionary<Transform, bool>(); // Tracks node availability

    private void Start()
    {
        // Initialize all nodes as available
        foreach (Transform node in spawnNodes)
        {
            nodeAvailability[node] = true;
        }

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
            else
            {
                Debug.LogWarning("No available nodes for spawning! Waiting for nodes to free up.");
            }
        }

        Debug.Log("Customer spawning complete.");
    }

    private Transform GetAvailableNode()
    {
        foreach (var node in nodeAvailability)
        {
            if (node.Value) // Check if the node is available
            {
                return node.Key;
            }
        }
        return null; // No available nodes
    }

    private void SpawnCustomer(Transform node)
    {
        GameObject customer = Instantiate(customerPrefab, node.position, Quaternion.identity);
        nodeAvailability[node] = false; // Mark the node as unavailable
        StartCoroutine(FreeNodeAfterTime(node, 16f)); // Free the node after 16 seconds
        spawnedCustomers++;
        Debug.Log($"Customer spawned at node: {node.name}");
    }

    private IEnumerator FreeNodeAfterTime(Transform node, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (!nodeAvailability[node]) // Double-check the node status
        {
            nodeAvailability[node] = true; // Free the node
            Debug.Log($"Node {node.name} is now free.");
        }
    }
}
