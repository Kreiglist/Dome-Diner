using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // The prefab of the customer to spawn
    public Transform spawnPoint; // The starting point for the queue
    public float queueSpacing = 1.5f; // The space between each customer in the line

    private int customerCount = 0; // To keep track of how many customers have been spawned
    private Vector3 lastCustomerPosition; // Tracks the position of the last customer in the queue

    void Start()
    {
        // Initialize the first customer position at the spawn point
        lastCustomerPosition = spawnPoint.position;
    }

    void Update()
    {
        // Check if a button (e.g., space bar) is pressed to spawn a new customer
        if (Input.GetKeyDown(KeyCode.Space)) // You can change this to any key or button
        {
            SpawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        // Calculate the position of the new customer based on the last one
        Vector3 newCustomerPosition = lastCustomerPosition + new Vector3(0, -queueSpacing, 0); // Align vertically

        // Spawn the new customer
        GameObject newCustomer = Instantiate(customerPrefab, newCustomerPosition, Quaternion.identity);

        // Update the position for the next customer in the line
        lastCustomerPosition = newCustomerPosition;

        // Increment the customer count
        customerCount++;
    }
}
