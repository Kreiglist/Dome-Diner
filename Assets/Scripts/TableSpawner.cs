using System.Collections.Generic;
using UnityEngine;

public class TableSpawner : MonoBehaviour
{
    public GameObject tablePrefab; // Prefab for the table
    public List<TableNode> tableNodes; // List of TableNode references in the scene
    public int numberOfTablesToSpawn = 5; // Number of tables to spawn

    private void Start()
    {
        if (tablePrefab == null)
        {
            Debug.LogError("Table prefab is not assigned.");
            return;
        }

        if (tableNodes == null || tableNodes.Count == 0)
        {
            Debug.LogError("TableNodes list is empty or unassigned.");
            return;
        }

        SpawnTables();
    }

    private void SpawnTables()
    {
        List<TableNode> availableNodes = new List<TableNode>(tableNodes);

        for (int i = 0; i < numberOfTablesToSpawn && availableNodes.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableNodes.Count);
            TableNode selectedNode = availableNodes[randomIndex];

            if (selectedNode != null && !selectedNode.isOccupied)
            {
                selectedNode.SpawnTable(tablePrefab);
                availableNodes.RemoveAt(randomIndex); // Remove the used node
            }
        }
    }
}
