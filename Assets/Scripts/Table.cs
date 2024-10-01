using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform seatingPosition;     // Position where the customer group should sit
    public GameObject seatingPreview;     // A visual preview that shows where the customers will sit
    public GameObject chairWithCustomerPrefab;  // Prefab for the chair with customer seated

    private GameObject instantiatedChair; // Reference to the instantiated chair prefab

    private void Start()
    {
        // Ensure the seating preview is hidden at the start
        if (seatingPreview != null)
        {
            seatingPreview.SetActive(false);
        }
    }

    // Show seating preview when hovering over the table
    public void ShowSeatingPreview(GameObject customerGroup)
    {
        Debug.Log("Showing seating preview...");

        // Activate the seating preview if available
        if (seatingPreview != null)
        {
            seatingPreview.SetActive(true);  // Show seating preview
        }
    }

    // Hide seating preview when the group leaves the table area
    public void HideSeatingPreview()
    {
        Debug.Log("Hiding seating preview...");

        // Deactivate the seating preview
        if (seatingPreview != null)
        {
            seatingPreview.SetActive(false);  // Hide seating preview
        }
    }

    // Return the seating position for the group to snap to
    public Transform GetSeatingPosition()
    {
        return seatingPosition;
    }

    // Seat the customer group and instantiate the seating prefab
    public void SeatCustomerGroup(GameObject customerGroup)
    {
        Debug.Log("Seating the customer group...");

        // Instantiate the chairWithCustomer prefab at the seating position (if applicable)
        if (instantiatedChair == null && chairWithCustomerPrefab != null)
        {
            instantiatedChair = Instantiate(chairWithCustomerPrefab, seatingPosition.position, Quaternion.identity);
            instantiatedChair.transform.SetParent(this.transform);  // Parent the instantiated object to the table
        }
        else
        {
            Debug.LogWarning("Chair has already been instantiated.");
        }

        // No disabling of prefabs or hiding necessary here, just seating the group
    }
}
