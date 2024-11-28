using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform chairPosition1; // First chair position (left)
    public Transform chairPosition2; // Second chair position (right)
    public GameObject seatedChairPrefab; // Prefab for the seated customer model

    private bool isTableOccupied = false; // Tracks table occupancy

    public void HandleCustomerDrop(Customer customer)
    {
        if (customer != null)
        {
            Debug.Log("Customer group dropped on table.");

            if (!isTableOccupied)
            {
                // Spawn seated customers at both chairs
                SpawnCustomerAtChair(chairPosition1, false); // Left chair (no flip)
                SpawnCustomerAtChair(chairPosition2, true);  // Right chair (flipped)

                // Hide the chairs
                HideChairs();

                // Mark the table as occupied
                isTableOccupied = true;

                // Remove or deactivate the original customer
                customer.gameObject.SetActive(false); // Deactivate the customer
            }
            else
            {
                Debug.LogWarning("Table is already occupied!");
            }
        }
    }

    private void SpawnCustomerAtChair(Transform chairPosition, bool flip)
    {
        GameObject seatedCustomer = Instantiate(seatedChairPrefab, chairPosition.position, Quaternion.identity);

        // Flip the prefab if required
        if (flip)
        {
            FlipObject(seatedCustomer);
        }
    }

    private void FlipObject(GameObject obj)
    {
        Vector3 scale = obj.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -1; // Ensure consistent flipping
        obj.transform.localScale = scale;
    }

    private void HideChairs()
    {
        if (chairPosition1 != null)
        {
            chairPosition1.gameObject.SetActive(false);
        }

        if (chairPosition2 != null)
        {
            chairPosition2.gameObject.SetActive(false);
        }
    }
}
