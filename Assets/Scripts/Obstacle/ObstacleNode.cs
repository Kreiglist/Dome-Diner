using UnityEngine;

public class ObstacleNode : MonoBehaviour
{
    public bool isBlocked = false; // Determines if this node is blocking the path

    public void SetBlocked(bool state)
    {
        isBlocked = state;
        Debug.Log($"{gameObject.name} is now {(isBlocked ? "BLOCKED" : "UNBLOCKED")}");
    }
}
