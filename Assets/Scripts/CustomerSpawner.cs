
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] customerPrefabs;  // Array of different customer prefabs
    public Transform spawnPoint;  // Spawn point for customers

    public void SpawnCustomer()
    {
        int randomIndex = Random.Range(0, customerPrefabs.Length);
        Instantiate(customerPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
    }
}
