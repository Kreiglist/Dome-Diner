using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TableSpawner tableSpawner;
    public int numberOfTablesToSpawn = 5;

    private void Start()
    {
        tableSpawner.numberOfTablesToSpawn = numberOfTablesToSpawn;
    }
}
