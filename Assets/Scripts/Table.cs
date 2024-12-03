using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour
{
    public int tableID; // Unique ID for the table
    public Transform chairPosition1; // First chair position
    public Transform chairPosition2; // Second chair position
    public GameObject seatedCustomerPrefab; // Customer reading menu
    public GameObject orderingCustomerPrefab; // Ordering customer
    public GameObject waitingCustomerPrefab; // Waiting for food customer
    public GameObject eatingCustomerPrefab; // Eating customer
    public GameObject callingCustomerPrefab; // Calling after eating
    public GameObject dirtyPlatePrefab; // Dirty plate prefab
    public GameObject moneyPrefab; // Money prefab
    public Transform patienceUIPosition; // Position for patience UI
    public GameObject patienceUIPrefab; // Prefab for patience UI 
    private bool isTableOccupied = false; // Tracks if the table is occupied
    private GameObject currentCustomer; // Current customer object
    private GameObject currentPatienceUI; // Current patience UI
    private Coroutine tableRoutine; // Tracks table coroutine
    private float patience = 0f; // Patience timer for the current phase
    private int currentPhase = 1; // Tracks current phase of the table

    private void Start()
    {
        // Ensure prefabs are initially hidden
        if (dirtyPlatePrefab != null) dirtyPlatePrefab.SetActive(false);
        if (moneyPrefab != null) moneyPrefab.SetActive(false);
    }

private void Update()
{
    // Handle patience countdown if applicable
    if (patience > 0 && (currentPhase == 3 || currentPhase == 5))
    {
        patience -= Time.deltaTime;
        UpdatePatienceUI();

        if (patience <= 0)
        {
            Debug.Log($"Table {tableID}: Customer left due to impatience.");
            ResetTable();
        }
    }

    // Check for interaction in Phase 2 or Phase 3
    if ((currentPhase == 2 || currentPhase == 3 || currentPhase == 4) && Input.GetMouseButtonDown(0))
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (isTableOccupied != null)
        {
            Debug.Log($"Player clicked to interact with Table {tableID} during Phase {currentPhase}.");
            ProcessInteraction();
        }
    }
}

    public void HandleCustomerDrop(Customer customer)
    {
        if (customer != null && !isTableOccupied)
        {
            Debug.Log($"Customer dropped on Table {tableID}.");
            isTableOccupied = true;
            currentPhase = 1;
            HideChairs();
            Destroy(customer.gameObject);
            tableRoutine = StartCoroutine(TableCycle());
        }
    }

private IEnumerator TableCycle()
{
    // Phase 1: Seated customer reading menu
    Debug.Log($"Table {tableID} - Phase 1: Seated customer.");
    currentCustomer = Instantiate(seatedCustomerPrefab, chairPosition1.position, Quaternion.identity);
    yield return new WaitForSeconds(Random.Range(5f, 10f));
    Destroy(currentCustomer);

    // Phase 2: Ordering customer
    Debug.Log($"Table {tableID} - Phase 2: Ordering customer.");
    currentCustomer = Instantiate(orderingCustomerPrefab, chairPosition1.position, Quaternion.identity);
    currentPhase = 2; // Explicitly set phase

    // Wait for player interaction to proceed to the next phase
    yield return new WaitUntil(() => currentPhase != 2); // Wait for interaction

    // Phase 3: Patience countdown starts after interaction
    Debug.Log($"Table {tableID} - Phase 3: Patience countdown.");
    StartPatience(15f);
    currentPhase = 3;

    // Wait for the player to complete the interaction during patience
    while (patience > 0 && currentPhase == 3)
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null && !player.isMoving) // Ensure player is stationary
        {
            ProcessOrder();
            yield break;
        }
        yield return null;
    }

    // If patience runs out, reset the table
    if (patience <= 0)
    {
        Debug.Log($"Table {tableID} - Customer left due to impatience.");
        ResetTable();
    }
}

    private void ProcessOrder()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddItem($"OrderPaper:{tableID}", inventory.orderPaperSprite, tableID);
            Debug.Log($"Order for Table {tableID} added to inventory.");
        }

        Destroy(currentCustomer);
        DestroyPatienceUI();
        StartWaitingForFood();
    }
private void ProcessInteraction()
{
    if (currentPhase == 2)
    {
        Debug.Log($"Player interacted with Table {tableID} during Phase 2. Proceeding to Phase 3.");
        currentPhase = 3; // Progress to Phase 3
        StartPatience(15f); // Start the patience timer for Phase 3
    }
    else if (currentPhase == 3)
    {
        Debug.Log($"Player interacted with Table {tableID} during Phase 3. Processing order.");
        ProcessOrder(); // Proceed to process the order
    }else if (currentPhase == 4)
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
    if (inventory != null && !player.isMoving)
    {
        string foodItem = $"Food:{tableID}";

        // Check if the inventory contains the correct food item
        if (inventory.HasItem(foodItem))
        {
            Debug.Log($"Player delivered correct food for Table {tableID}. Starting eating phase.");

            // Remove the food from the inventory
            int foodSlot = inventory.FindItemSlot(foodItem);
            if (foodSlot != -1)
            {
                inventory.RemoveItem(foodSlot);
            }

            StartEating(); // Start the eating phase
        }
        else
        {
            Debug.LogWarning($"Player does not have the correct food for Table {tableID}. Cannot proceed.");
        }
    }
}
}

    private void StartWaitingForFood()
    {
        Debug.Log($"Table {tableID} - Phase 3: Waiting for food.");
        currentCustomer = Instantiate(waitingCustomerPrefab, chairPosition1.position, Quaternion.identity);
        currentPhase = 4;
    }

    private void StartEating()
    {
        Debug.Log($"Table {tableID} - Phase 4: Eating.");
        Destroy(currentCustomer);
        currentCustomer = Instantiate(eatingCustomerPrefab, chairPosition1.position, Quaternion.identity);
        StartCoroutine(EatingTimer());
    }

    private IEnumerator EatingTimer()
    {
        yield return new WaitForSeconds(15f);
        Destroy(currentCustomer);
        StartOrderPhaseWithDirtyPlate();
    }

    private void StartOrderPhaseWithDirtyPlate()
    {
        Debug.Log($"Table {tableID} - Phase 5: Dirty plate.");
        currentCustomer = Instantiate(callingCustomerPrefab, chairPosition1.position, Quaternion.identity);
        if (dirtyPlatePrefab != null) dirtyPlatePrefab.SetActive(true);
        StartPatience(30f);
        currentPhase = 5;
    }

    private void ResetTable()
    {
        Debug.Log($"Table {tableID} - Resetting to original state.");
        if (dirtyPlatePrefab != null) dirtyPlatePrefab.SetActive(false);
        if (moneyPrefab != null) moneyPrefab.SetActive(false);
        if (currentCustomer != null) Destroy(currentCustomer);
        DestroyPatienceUI();
        isTableOccupied = false;
        currentPhase = 1;
    }

    private void StartPatience(float duration)
    {
        patience = duration;
        if (currentPatienceUI != null) Destroy(currentPatienceUI);

        if (patienceUIPrefab != null)
        {
            currentPatienceUI = Instantiate(patienceUIPrefab, patienceUIPosition.position, Quaternion.identity);
        }
    }

    private void UpdatePatienceUI()
    {
        // Logic to update the patience UI (e.g., filling a bar)
        if (currentPatienceUI != null)
        {
            // Add UI update logic here if needed
        }
    }
    private void DestroyPatienceUI()
    {
        if (currentPatienceUI != null)
        {
            Destroy(currentPatienceUI);
            currentPatienceUI = null;
        }
    }

    private void HideChairs()
    {
        if (chairPosition1 != null) chairPosition1.gameObject.SetActive(false);
        if (chairPosition2 != null) chairPosition2.gameObject.SetActive(false);
    }
} 