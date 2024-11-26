using UnityEngine;

public class TableNode : MonoBehaviour
{
    public bool isOccupied = false; // Tracks if a table is already on this node
    public GameObject spawnedTable; // Reference to the spawned table (if any)

    public void SpawnTable(GameObject tablePrefab)
    {
        if (isOccupied || tablePrefab == null)
            return;

        spawnedTable = Instantiate(tablePrefab, transform.position, Quaternion.identity);
        isOccupied = true;

        // Set the spawned table as a child of this node for organization
        spawnedTable.transform.SetParent(transform);
    }

    public void RemoveTable()
    {
        if (spawnedTable != null)
        {
            Destroy(spawnedTable);
            isOccupied = false;
        }
    }
}
