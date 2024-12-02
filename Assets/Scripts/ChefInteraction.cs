using UnityEngine;

public class ChefInteraction : MonoBehaviour
{
    public PathNode associatedNode; // The PathNode this object points to (for player movement)
    private Chef chef; // Reference to the Chef script

    private void Start()
    {
        // Ensure the Chef script is attached to this GameObject
        chef = GetComponent<Chef>();
        if (chef == null)
        {
            Debug.LogError("Chef script is missing on this GameObject.");
        }
    }

    private void OnMouseDown()
    {
        // Step 1: Check if there's an associated node for movement
        if (associatedNode == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have an associated PathNode.");
            return;
        }

        // Step 2: Move the player to the associated node
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.QueueMovement(associatedNode);

                // Step 3: Start a coroutine to wait until the player finishes movement
                StartCoroutine(WaitForPlayerToReachNode(playerMovement));
            }
            else
            {
                Debug.LogError("Player is missing the PlayerMovement script.");
            }
        }
        else
        {
            Debug.LogError("No GameObject tagged 'Player' found in the scene.");
        }
    }

    private System.Collections.IEnumerator WaitForPlayerToReachNode(PlayerMovement playerMovement)
    {
        // Wait until the player stops moving
        while (playerMovement.IsMoving())
        {
            yield return null; // Wait for the next frame
        }

        // Step 4: Once the player has reached the node, process the interaction
        HandlePlayerInteraction();
    }

    private void HandlePlayerInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                // Check if the player has an order paper
                int orderSlot = inventory.FindItemSlot("OrderPaper:");
                if (orderSlot != -1)
                {
                    // Extract the table ID from the order paper
                    string order = inventory.GetItem(orderSlot);
                    int tableID = int.Parse(order.Split(':')[1]);

                    // Pass the order to the chef and remove it from inventory
                    if (chef != null)
                    {
                        chef.ReceiveOrder(tableID);
                        inventory.RemoveItem(orderSlot);
                        Debug.Log($"Order for Table {tableID} given to the chef.");
                    }
                }
                else
                {
                    Debug.LogWarning("No order paper in the player's inventory.");
                }
            }
            else
            {
                Debug.LogError("Player is missing the PlayerInventory script.");
            }
        }
    }
}
