using UnityEngine;

public class DraggableCustomerGroup : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Table currentHoveredTable = null;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDragging)
        {
            if (currentHoveredTable != null)
            {
                // Snap to the seating position of the hovered table
                //transform.position = currentHoveredTable.seatingPosition.position;
                ChangeToSeatedPose(); // Change the customer group to a seated pose
            }
            else
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePosition.x - offset.x, mousePosition.y - offset.y, 0);
            }
        }
    }

    void OnMouseDown()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - transform.position;
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        if (currentHoveredTable != null)
        {
            // Finalize seating when the mouse is released over a table
            CustomerGroup customerGroup = GetComponent<CustomerGroup>();
            if (customerGroup != null)
            {
                customerGroup.SitGroupAtTable(currentHoveredTable); // Seat the group at the table
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>();
            if (table != null)
            {
                currentHoveredTable = table;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Table"))
        {
            Table table = other.GetComponent<Table>();
            if (table != null && currentHoveredTable == table)
            {
                currentHoveredTable = null;
                ChangeToStandingPose(); // Revert to the standing pose when exiting
            }
        }
    }

    private void ChangeToSeatedPose()
    {
        // Logic to change customer group to seated pose
        // This could involve enabling a different animation or changing the sprite
    }

    private void ChangeToStandingPose()
    {
        // Logic to change customer group back to standing pose
        // This could involve enabling the original animation or sprite
    }
}