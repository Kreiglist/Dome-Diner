using UnityEngine;

public abstract class PlayerPathfindingAlgorithm : ScriptableObject
{
    public abstract void MovePlayer(GameObject player, Vector3 targetPosition);
}
