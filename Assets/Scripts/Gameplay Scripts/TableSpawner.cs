using UnityEngine;

public class TableSpawner : MonoBehaviour
{
    public GameObject tablePrefab;
    public Transform[] tableNodes; // Pre-placed table nodes
    public int numberOfTablesToSpawn = 5; // Number of tables to spawn

    private void Start()
    {
        SpawnTables();
    }

    private void SpawnTables()
    {
        System.Collections.Generic.List<int> usedIndices = new System.Collections.Generic.List<int>();

        for (int i = 0; i < numberOfTablesToSpawn; i++)
        {
            int randomIndex;

            // Ensure a unique node index is selected
            do
            {
                randomIndex = Random.Range(0, tableNodes.Length);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            // Instantiate table at the selected node
            Instantiate(tablePrefab, tableNodes[randomIndex].position, Quaternion.identity);
        }
    }
}
