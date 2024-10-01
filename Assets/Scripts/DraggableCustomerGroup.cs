using UnityEngine;

public class DraggableCustomerGroup : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 startPosition;

    void Update()
    {
        if (isDragging)
        {
            // Follow the mouse position (applies to the whole group)
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);  // Update position of the group
        }
    }

    void OnMouseDown()
    {
        // Start dragging the entire customer group
        isDragging = true;
        startPosition = transform.position; // Save the original position in case we need to cancel
    }

    void OnMouseUp()
    {
        // Stop dragging the group
        isDragging = false;

        // Optionally, you can add logic to "snap" the group to a table or cancel movement
        // E.g., check if over a table, then snap into place
    }
}
