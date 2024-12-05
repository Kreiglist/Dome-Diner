using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private bool isDragging = false; // Whether the customer is being dragged
    private Vector3 offset; // Offset for accurate dragging
    private Camera mainCamera; // Main camera for screen-to-world point conversion
    public Transform spawnNode;
    public float patience = 15f; // Patience timer in seconds
    public Slider patienceSlider; // Optional: Attach a UI slider to display patience visually
    private bool isDespawned = false; // Tracks if the customer is already despawned

    private void Start()
    {
        mainCamera = Camera.main; // Cache the main camera

        // Initialize the patience slider if available
        if (patienceSlider != null)
        {
            patienceSlider.maxValue = patience;
            patienceSlider.value = patience;
        }
    }

    private void Update()
    {
        // Handle patience timer
        if (!isDragging && !isDespawned)
        {
            patience -= Time.deltaTime;

            // Update patience slider if available
            if (patienceSlider != null)
            {
                patienceSlider.value = patience;
            }

            // Despawn the customer if patience reaches zero
            if (patience <= 0)
            {
                DespawnCustomer();
            }
        }

        // Handle dragging logic
        if (isDragging)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x - offset.x, mousePosition.y - offset.y, 0);
        }
    }

    private void OnMouseDown()
    {
        // Start dragging
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - transform.position;
        isDragging = true;
    }

    private void OnMouseUp()
    {
        // Stop dragging
        isDragging = false;

        // Check if the customer was dropped on a table
        HandleDrop();
    }

   private void HandleDrop()
{
    float detectionRadius = 0.5f;
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

    foreach (var hitCollider in hitColliders)
    {
        if (hitCollider.CompareTag("Table"))
        {
            Table table = hitCollider.GetComponent<Table>();
            if (table != null)
            {
                Debug.Log($"Customer dropped on table: {table.name}");
                table.HandleCustomerDrop(this);
                FreeSpawnNode(); // Free the spawn node
                Destroy(gameObject); // Destroy customer after successful drop
                return;
            }
            else
            {
                Debug.LogWarning($"Collider with tag 'Table' does not have a Table component: {hitCollider.name}");
            }
        }
    }

    // If no valid table was found, return to original position
    Debug.Log("Customer not dropped on a valid table. Returning to initial position.");
    if (spawnNode != null)
    {
        transform.position = spawnNode.position; // Return to spawn node
    }
    else
    {
        Debug.LogError("Spawn node is null. Assigning fallback position.");
        transform.position = Vector3.zero; // Fallback position
    }
}


    public void SetSpawnNode(Transform node)
    {
        spawnNode = node;
        MarkNodeAsUnavailable();
    }

    private void MarkNodeAsUnavailable()
    {
        if (spawnNode != null)
        {
            var nodeScript = spawnNode.GetComponent<PathNode>();
            if (nodeScript != null)
            {
                nodeScript.SetOccupied(true);
            }
        }
    }

    public void FreeSpawnNode()
    {
        if (spawnNode != null)
        {
            var nodeScript = spawnNode.GetComponent<PathNode>();
            if (nodeScript != null)
            {
                nodeScript.SetOccupied(false); // Mark the node as free
                Debug.Log($"Node {spawnNode.name} is now free.");
            }
        }
    }

    private void DespawnCustomer()
    {
        isDespawned = true; // Mark as despawned
        Debug.Log("Customer despawned due to patience timeout.");
        FreeSpawnNode(); // Free the spawn node
        Destroy(gameObject); // Remove the customer
    }
}
