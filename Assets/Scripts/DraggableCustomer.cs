using UnityEngine;

public class DraggableCustomer : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 startPosition;
    private Table currentHoveredTable = null;

    void Update()
    {
        if (isDragging)
        {
            // Follow the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }

    void OnMouseDown()
    {
        // Start dragging the customer
        isDragging = true;
        startPosition = transform.position; // Save the original position in case we need to cancel
    }

    void OnMouseUp()
    {
        // Stop dragging
        isDragging = false;

        if (currentHoveredTable != null)
        {
            // Seat the customer at the current hovered table
            currentHoveredTable.SeatCustomer(this.gameObject);
        }
        else
        {
            // If not dropped on a table, return to the original position
            transform.position = startPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the customer is hovering over a table
        if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>();
            if (table != null)
            {
                currentHoveredTable = table;
                table.ShowSeatingPreview(this.gameObject); // Show seating preview
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Stop showing seating preview when the customer leaves the table area
        if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>();
            if (table != null)
            {
                currentHoveredTable = null;
                table.HideSeatingPreview(); // Hide the seating preview
            }
        }
    }
}
