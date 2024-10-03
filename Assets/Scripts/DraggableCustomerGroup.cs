using UnityEngine;
using System.Collections;

public class DraggableCustomerGroup : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    private Table currentHoveredTable = null;  // Reference to the table being hovered over

    private Collider2D customerCollider;  // Customer group's collider

    private void Start()
    {
        mainCamera = Camera.main;
        customerCollider = GetComponent<Collider2D>();

        // Enable the collider right after the game starts
        customerCollider.enabled = true;
    }

    void Update()
    {
        if (isDragging)
        {
            // Move the group to follow the mouse position while dragging
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x - offset.x, mousePosition.y - offset.y, 0);
        }
    }

    void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the group's current position
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - transform.position;

        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Snap the group to the seating position if hovering over a table
        if (currentHoveredTable != null)
        {
            SnapToTable();
            currentHoveredTable.SeatCustomerGroup(this.gameObject);  // Seat the group
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Detect when the customer group is hovering over the table
        if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>();
            if (table != null)
            {
                currentHoveredTable = table;
                currentHoveredTable.ShowSeatingPreview(this.gameObject);  // Show seating preview
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Detect when the customer group is no longer hovering over the table
        if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>();
            if (table != null)
            {
                currentHoveredTable = null;
                table.HideSeatingPreview();  // Hide seating preview
            }
        }
    }

    // Snap the group to the table's seating position
    private void SnapToTable()
    {
        if (currentHoveredTable != null)
        {
            // Move the customer group to the table's seating position
            transform.position = currentHoveredTable.GetSeatingPosition().position;

            // Hide seating preview after snapping
            currentHoveredTable.HideSeatingPreview();
        }
    }
}
