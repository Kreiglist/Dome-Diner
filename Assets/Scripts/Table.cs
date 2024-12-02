using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour
{
    public int tableID; // Unique ID for the table
    public Transform chairPosition1; // First chair position (left)
    public Transform chairPosition2; // Second chair position (right)
    public GameObject seatedCustomerPrefab; // Seated customer prefab
    public GameObject orderingCustomerPrefab; // Ordering customer prefab
    public GameObject waitingCustomerPrefab; // Waiting for food customer prefab
    public GameObject eatingCustomerPrefab; // Eating customer prefab
    public GameObject dirtyPlatePrefab; // Dirty plate prefab
    public GameObject moneyPrefab; // Money prefab

    private bool isTableOccupied = false; // Tracks table occupancy
    private GameObject currentCustomer; // Current customer at the table
    private Coroutine tableRoutine; // Reference to the current coroutine

    private void Start()
    {
        // Ensure the dirty plate and money prefabs are hidden initially
        if (dirtyPlatePrefab != null) dirtyPlatePrefab.SetActive(false);
        if (moneyPrefab != null) moneyPrefab.SetActive(false);
    }

    public void HandleCustomerDrop(Customer customer)
    {
        if (customer != null && !isTableOccupied)
        {
            Debug.Log($"Customer dropped on table {tableID}.");
            isTableOccupied = true;

            // Start the table cycle
            tableRoutine = StartCoroutine(TableCycle());

            // Free the customer's spawn node
            customer.FreeSpawnNode();
            HideChairs();

            // Destroy the dropped customer
            Destroy(customer.gameObject);
        }
    }

    private IEnumerator TableCycle()
    {
        // Phase 1: Seated customer
        Debug.Log($"Table {tableID} - Phase 1: Seated customer.");
        currentCustomer = Instantiate(seatedCustomerPrefab, chairPosition1.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        Destroy(currentCustomer);

        // Phase 2: Ordering customer
        Debug.Log($"Table {tableID} - Phase 2: Ordering customer.");
        currentCustomer = Instantiate(orderingCustomerPrefab, chairPosition1.position, Quaternion.identity);

        // Wait for player interaction
        Debug.Log($"Waiting for interaction at Table {tableID}...");
    }

    public void OnPlayerInteraction()
    {
        Debug.Log($"Player interacted with Table {tableID}.");
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddItem($"OrderPaper:{tableID}", inventory.orderPaperSprite, tableID);
            Debug.Log($"Order from Table {tableID} added to inventory.");
        }

        Destroy(currentCustomer);
        StartWaitingForFood();
    }

    private void StartWaitingForFood()
    {
        Debug.Log($"Table {tableID} - Phase 3: Waiting for food.");
        Destroy(currentCustomer);
        currentCustomer = Instantiate(waitingCustomerPrefab, chairPosition1.position, Quaternion.identity);
        StartCoroutine(WaitingForFoodTimer());
    }

    private IEnumerator WaitingForFoodTimer()
    {
        float patience = 30f;
        while (patience > 0)
        {
            patience -= Time.deltaTime;
            yield return null;

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory != null && inventory.HasItem($"Food:{tableID}"))
            {
                int slotIndex = inventory.FindItemSlot($"Food:{tableID}");
                inventory.RemoveItem(slotIndex);
                Debug.Log($"Food delivered to Table {tableID}.");
                Destroy(currentCustomer);
                StartEating();
                yield break;
            }
        }

        Debug.Log($"Table {tableID} - Customer left due to impatience.");
        ResetTable();
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
        Debug.Log($"Table {tableID} - Phase 5: Dirty plate and ordering.");
        currentCustomer = Instantiate(orderingCustomerPrefab, chairPosition1.position, Quaternion.identity);

        if (dirtyPlatePrefab != null)
        {
            dirtyPlatePrefab.SetActive(true);
        }

        StartCoroutine(OrderWithDirtyPlate(15f));
    }

    private IEnumerator OrderWithDirtyPlate(float patience)
    {
        while (patience > 0)
        {
            patience -= Time.deltaTime;
            yield return null;

            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory != null && PlayerInteraction.MoneyCollected(this))
            {
                if (moneyPrefab != null)
                {
                    moneyPrefab.SetActive(true);
                }
                StartResetTable();
                yield break;
            }
        }

        Debug.Log($"Table {tableID} - Customer left due to impatience.");
        ResetTable();
    }

    private void StartResetTable()
    {
        Debug.Log($"Table {tableID} - Resetting table after payment.");
        ResetTable();
    }

    private void ResetTable()
    {
        Debug.Log($"Table {tableID} - Resetting to original state.");

        if (dirtyPlatePrefab != null) dirtyPlatePrefab.SetActive(false);
        if (moneyPrefab != null) moneyPrefab.SetActive(false);

        if (currentCustomer != null) Destroy(currentCustomer);

        isTableOccupied = false;
    }

    private void HideChairs()
    {
        if (chairPosition1 != null) chairPosition1.gameObject.SetActive(false);
        if (chairPosition2 != null) chairPosition2.gameObject.SetActive(false);
    }
}
