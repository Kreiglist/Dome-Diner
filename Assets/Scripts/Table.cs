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
    private bool isPlayerNearby = false; // Tracks if the player is in range
    private bool isInteracting = false; // Tracks if player is interacting
    private int currentPhase = 1; // Tracks current phase of the table

    private void Start()
    {
        // Ensure prefabs are initially hidden
        if (dirtyPlatePrefab != null) dirtyPlatePrefab.SetActive(false);
        if (moneyPrefab != null) moneyPrefab.SetActive(false);
    }

    private void Update()
    {   
        // Update patience timer if applicable
        if ((currentPhase == 3 || currentPhase == 4 || currentPhase == 5) && patience > 0)
        {
            patience -= Time.deltaTime;
            if (patience <= 0)
            {
                Debug.Log($"Table {tableID}: Customer left due to impatience.");
                ResetTable();
            }
        }
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // Adjust radius
    foreach (Collider collider in colliders)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log($"Player detected near Table {tableID} in Update");
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
        currentPhase = 3;

        // Start patience UI
        StartPatience(15f);

        // Wait for player interaction
        while (patience > 0)
        {
            if (isPlayerNearby && !isInteracting)
            {
                Debug.Log($"Player interacted with Table {tableID}.");
                PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
                if (inventory != null)
                {
                    inventory.AddItem($"OrderPaper:{tableID}", inventory.orderPaperSprite, tableID);
                    Debug.Log($"Order for Table {tableID} added to inventory.");
                }
                Destroy(currentCustomer);
                DestroyPatienceUI();
                StartWaitingForFood();
                yield break;
            }
            yield return null;
        }
    }

    private void StartWaitingForFood()
    {
        // Phase 3: Waiting for food
        Debug.Log($"Table {tableID} - Phase 3: Waiting for food.");
        currentCustomer = Instantiate(waitingCustomerPrefab, chairPosition1.position, Quaternion.identity);
        currentPhase = 4;
    }

    private void StartEating()
    {
        // Phase 4: Eating customer
        Debug.Log($"Table {tableID} - Phase 4: Eating.");
        Destroy(currentCustomer);
        currentCustomer = Instantiate(eatingCustomerPrefab, chairPosition1.position, Quaternion.identity);
        StartPatience(30f);
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
        // Phase 5: Dirty plate and calling
        Debug.Log($"Table {tableID} - Phase 5: Dirty plate.");
        currentCustomer = Instantiate(callingCustomerPrefab, chairPosition1.position, Quaternion.identity);
        if (dirtyPlatePrefab != null) dirtyPlatePrefab.SetActive(true);
        StartPatience(30f);
        currentPhase = 5;
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
        DestroyPatienceUI();
        isTableOccupied = false;
        currentPhase = 1;
    }

    private void StartPatience(float duration)
    {
        patience = duration;
        if (currentPatienceUI != null) Destroy(currentPatienceUI); // Destroy old UI if any
        if (patienceUIPrefab != null)
        {
            currentPatienceUI = Instantiate(patienceUIPrefab, patienceUIPosition.position, Quaternion.identity);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Something entered Table {tableID}'s trigger: {other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player reached Table {tableID}.");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player left Table {tableID}.");
            isPlayerNearby = false;
        }
    }

    private void HideChairs()
    {
        if (chairPosition1 != null) chairPosition1.gameObject.SetActive(false);
        if (chairPosition2 != null) chairPosition2.gameObject.SetActive(false);
    }
}
