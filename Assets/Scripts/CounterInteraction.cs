using UnityEngine;

public class CounterInteraction : MonoBehaviour
{
    public PathNode associatedNode; // PathNode for player movement
    public Transform[] spawnPoints; // Points where food can be placed on the counter
    public GameObject foodPrefab; // The prefab for the food
    private Chef chef; // Reference to the Chef

    private void Start()
    {
        chef = FindObjectOfType<Chef>();
        if (chef == null)
        {
            Debug.LogError("Chef not found in the scene.");
        }
    }

    private void OnMouseDown()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Move the player to the associated node
                playerMovement.QueueMovement(associatedNode);

                // Start a coroutine to wait until the player reaches the node
                StartCoroutine(WaitForPlayerToReachNode(player, playerMovement));
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

    private System.Collections.IEnumerator WaitForPlayerToReachNode(GameObject player, PlayerMovement playerMovement)
    {
        // Wait until the player stops moving
        while (playerMovement.IsMoving())
        {
            yield return null; // Wait for the next frame
        }

        // Once the player has reached the node, process the interaction
        HandlePlayerInteraction(player);
    }

    private void HandlePlayerInteraction(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            // Check for food at the counter
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint.childCount > 0)
                {
                    GameObject food = spawnPoint.GetChild(0).gameObject;
                    FoodItem foodItem = food.GetComponent<FoodItem>();

                    // Add to inventory only if there's space
                    if (inventory.AddItem($"Food:{foodItem.GetTableID()}", inventory.foodSprite, foodItem.GetTableID()))
                    {
                        Destroy(food); // Remove food from the counter
                        Debug.Log("Food added to inventory.");
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Inventory is full. Cannot add more food.");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Player is missing the PlayerInventory script.");
        }
    }
}
