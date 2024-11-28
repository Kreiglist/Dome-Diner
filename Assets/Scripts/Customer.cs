using UnityEngine;

public class Customer : MonoBehaviour
{
    private bool isDragging = false; // Whether the customer is being dragged
    private Vector3 offset; // Offset for accurate dragging
    private Camera mainCamera; // Main camera for screen-to-world point conversion
    private Vector3 initialPosition; // Original spawn position of the customer
    private Transform spawnNode; // The node where the customer was spawned

    private void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
        initialPosition = transform.position; // Store the original position
    }

    private void Update()
    {
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
                    MarkNodeAsAvailable(); // Free the spawn node
                    Destroy(gameObject); // Destroy customer after successful drop
                    return;
                }
            }
        }

        // If no valid table was found, return to original position
        Debug.Log("Customer not dropped on a valid table. Returning to initial position.");
        transform.position = initialPosition;
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

    private void MarkNodeAsAvailable()
    {
        if (spawnNode != null)
        {
            var nodeScript = spawnNode.GetComponent<PathNode>();
            if (nodeScript != null)
            {
                nodeScript.SetOccupied(false);
            }
        }
    }
}
