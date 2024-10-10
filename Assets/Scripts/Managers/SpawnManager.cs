using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;

    internal void Spawn(string prefabName, int count, float interval, Transform transformInfo)
    {
        if (!IsExist(prefabName))
        {
            Debug.Log("Prefab's name not found");
            return;
        }

        StartCoroutine(SpawnPrefab(prefabName, count, interval, transformInfo));
    }

    internal void SpawnEndlessly(string prefabName, float interval, List<Transform> spawnPoints)
    {
        if (!IsExist(prefabName))
        {
            Debug.Log("Prefab's name not found");
            return;
        }

        StartCoroutine(SpawnPrefabEndlessly(prefabName, interval, spawnPoints));
    }

    private IEnumerator SpawnPrefab(string prefabName, int count, float interval, Transform transformInfo)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(GetPrefab(prefabName), transformInfo.position, transformInfo.rotation);
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator SpawnPrefabEndlessly(string prefabName, float interval, List<Transform> spawnPoints)
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(GetPrefab(prefabName), spawnPoint.position, spawnPoint.rotation);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private bool IsExist(string prefabName)
    {
        foreach (GameObject go in prefabs)
        {
            if (go.name == prefabName)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject GetPrefab(string prefabName)
    {
        foreach (GameObject go in prefabs)
        {
            if (go.name == prefabName) return go;
        }
        return null;
    }
}