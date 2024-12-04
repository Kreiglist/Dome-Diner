using UnityEngine;

public class DirtyPlateRemover : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.LogWarning("clicked");
        // Find the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found. Make sure the Player GameObject is tagged 'Player'.");
            return;
        }

        // Get the PlayerMovement script to check if the player is not moving
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogWarning("PlayerMovement script not found on Player.");
            return;
        }

        // Check if the player is not moving
        if (playerMovement.isMoving)
        {
            Debug.Log("Player is moving. Cannot delete dirty plates yet.");
            return;
        }

        // Check if the player is within the collider's bounds
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D thisCollider = GetComponent<Collider2D>();

        if (playerCollider != null && thisCollider != null && playerCollider.IsTouching(thisCollider))
        {
            // Get the PlayerInventory script to manage the inventory
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
            if (playerInventory == null)
            {
                Debug.LogWarning("PlayerInventory script not found on Player.");
                return;
            }

            // Try to remove a dirty plate
            if (playerInventory.RemoveItemByType("DirtyPlate"))
            {
                Debug.Log("Dirty plate removed from inventory.");
            }
            else
            {
                Debug.Log("No dirty plates in inventory to remove.");
            }
        }
        else
        {
            Debug.Log("Player is not within the collider.");
        }
    }
}
