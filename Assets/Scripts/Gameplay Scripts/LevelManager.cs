using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int customersServed = 0;
    private int customersLeft;
    public GameObject endLevelUI;  // UI for the end level screen
    public GameObject customerSpawnerObject;  // Reference to CustomerSpawner GameObject
    public GameObject[] tables;  // Reference to all tables
    private CustomerSpawner customerSpawner;  // Reference to the CustomerSpawner script
    private bool levelEnded = false;
    private LevelManager levelManager;
    void Start()
    {
        Time.timeScale = 1f;  // Resume the game (normal time progression)
        // Get the CustomerSpawner component from the customerSpawnerObject
        customerSpawner = customerSpawnerObject.GetComponent<CustomerSpawner>();
        customersLeft = customerSpawner.maxCustomersToSpawn;  // Get maxCustomersToSpawn from CustomerSpawner
        endLevelUI.SetActive(false);
        levelManager = FindObjectOfType<LevelManager>();
        // Disable button colliders at the start
        endLevelUI.GetComponent<EndLevelUI>().DisableButtons();
    }

    void Update()
    {
        if (!levelEnded)
        {
            CheckLevelCompletion();
        }
    }

    void CheckLevelCompletion()
    {
        // Check if customer spawner has finished spawning all customers and tables are not occupied
        if (customerSpawner.IsFinishedSpawning() && AreAllTablesEmpty())
        {
            // Wait for 2 seconds before ending the level
            Invoke("EndLevel", 2f);
        }
    }

    bool AreAllTablesEmpty()
    {
        foreach (GameObject table in tables)
        {
            if (table.GetComponent<Table>().isTableOccupied)  // Access the field directly
            {
                return false;
            }
        }
        return true;
    }

    void EndLevel()
    {
        levelEnded = true;
        endLevelUI.SetActive(true);

        // Show customer performance
        endLevelUI.GetComponent<EndLevelUI>().UpdateUI(customersServed, customersLeft);

        // Enable buttons after the level ends
        endLevelUI.GetComponent<EndLevelUI>().EnableButtons();
    }

    // This method should be called by another script when a customer is served
    public void CustomerServed()
    {
        customersServed++;
        customersLeft = customerSpawner.maxCustomersToSpawn - customersServed;
    }
}
