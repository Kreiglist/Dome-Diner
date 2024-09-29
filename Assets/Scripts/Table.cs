using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform seatingPosition; // The seating position where the customer will sit
    public GameObject seatingPreviewPrefab; // Prefab or image to display as seating preview

    private GameObject currentSeatingPreview = null;

    // Show the seating preview when the customer hovers over the table
    public void ShowSeatingPreview(GameObject customer)
    {
        if (currentSeatingPreview == null)
        {
            // Create the seating preview at the seating position
            currentSeatingPreview = Instantiate(seatingPreviewPrefab, seatingPosition.position, Quaternion.identity);
        }
    }

    // Hide the seating preview when the customer leaves the table
    public void HideSeatingPreview()
    {
        if (currentSeatingPreview != null)
        {
            Destroy(currentSeatingPreview);
            currentSeatingPreview = null;
        }
    }

    // Seat the customer at the table
    public void SeatCustomer(GameObject customer)
    {
        // Move the customer to the seating position
        customer.transform.position = seatingPosition.position;

        // Optionally, you can disable dragging or mark the customer as seated
        DraggableCustomer draggableCustomer = customer.GetComponent<DraggableCustomer>();
        if (draggableCustomer != null)
        {
            Destroy(draggableCustomer); // Optional: Remove dragging ability after seating
        }

        // Hide the seating preview
        HideSeatingPreview();
    }
}
