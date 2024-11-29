using UnityEngine;

public class FoodState : MonoBehaviour
{
    public GameObject fullFoodPrefab; // Prefab for full food
    public GameObject twoThirdsFoodPrefab; // Prefab for 2/3 eaten food
    public GameObject oneThirdFoodPrefab; // Prefab for 1/3 eaten food

    private GameObject currentFoodInstance; // The currently active food instance

    private float totalTime = 15f; // Total time for eating
    private float elapsedTime = 0f;

    private void Start()
    {
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float remainingTime = totalTime - elapsedTime;

        if (remainingTime > 9f && currentFoodInstance != fullFoodPrefab)
        {
            UpdateFoodPrefab(fullFoodPrefab);
        }
        else if (remainingTime <= 9f && remainingTime > 4f && currentFoodInstance != twoThirdsFoodPrefab)
        {
            UpdateFoodPrefab(twoThirdsFoodPrefab);
        }
        else if (remainingTime <= 4f && currentFoodInstance != oneThirdFoodPrefab)
        {
            UpdateFoodPrefab(oneThirdFoodPrefab);
        }

        if (elapsedTime >= totalTime)
        {
            Destroy(currentFoodInstance); // Clean up the final prefab
            Destroy(this.gameObject); // Remove the script's object
        }
    }

    private void SpawnFood(GameObject foodPrefab)
    {
        if (foodPrefab != null)
        {
            currentFoodInstance = Instantiate(foodPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    private void UpdateFoodPrefab(GameObject newPrefab)
    {
        if (currentFoodInstance != null)
        {
            Destroy(currentFoodInstance);
        }
        SpawnFood(newPrefab);
    }
}
