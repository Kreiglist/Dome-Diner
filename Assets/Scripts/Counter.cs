using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    public Transform[] spawnPoints; // Array of spawn points for food
    public GameObject foodPrefab; // Prefab for food items

    /// <summary>
    /// Spawns food at an available spawn point with the given table ID.
    /// </summary>
    /// <param name="tableID">The table ID for the food.</param>
    /// <returns>True if food was spawned successfully, false otherwise.</returns>
    public bool SpawnFood(int tableID)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount == 0) // Check if the spawn point is empty
            {
                GameObject food = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);

                // Set the table ID on the food
                TMP_Text idText = food.GetComponentInChildren<TMP_Text>();
                if (idText != null)
                {
                    idText.text = $"{tableID}";
                }

                FoodItem foodItem = food.GetComponent<FoodItem>();
                if (foodItem != null)
                {
                    foodItem.tableID = tableID;
                }

                return true; // Food spawned successfully
            }
        }

        Debug.LogWarning("All spawn points are occupied!");
        return false; // No available spawn point
    }
}
