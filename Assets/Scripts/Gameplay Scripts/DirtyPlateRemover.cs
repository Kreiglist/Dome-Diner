using UnityEngine;

public class DirtyPlateRemover : MonoBehaviour
{
    private bool isProcessing = false; // To prevent multiple coroutine executions

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player entered the Dirty Plate Remover zone.");

        if (!isProcessing)
        {
            StartCoroutine(WaitForPlayerToStop(other.gameObject));
            StartCoroutine(WaitForPlayerToStop(other.gameObject));
        }
    }

    private System.Collections.IEnumerator WaitForPlayerToStop(GameObject player)
    {
        isProcessing = true;

        // Get the PlayerMovement script
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogWarning("PlayerMovement script not found on Player.");
            isProcessing = false;
            yield break;
        }

        // Wait until the player stops moving
        while (playerMovement.isMoving)
        {
            Debug.Log("Player is moving. Waiting for them to stop...");
            yield return null; // Wait for the next frame
        }

        // Ensure the player is still within the collider after stopping
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D thisCollider = GetComponent<Collider2D>();

        if (playerCollider != null && thisCollider != null && playerCollider.IsTouching(thisCollider))
        {
            Debug.Log("Player is within the Dirty Plate Remover collider.");

            // Get the PlayerInventory script
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
            if (playerInventory == null)
            {
                Debug.LogWarning("PlayerInventory script not found on Player.");
                isProcessing = false;
                yield break;
            }

            // Try to remove a dirty plate
            if (playerInventory.RemoveItemByType("EmptyPlate"))
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
            Debug.Log("Player is no longer within the collider.");
        }

        isProcessing = false;
    }
}
