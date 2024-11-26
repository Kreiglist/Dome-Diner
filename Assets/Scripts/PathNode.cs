using UnityEngine;
using System.Collections.Generic;

public class PathNode : MonoBehaviour
{
    public List<PathNode> connectedNodes = new List<PathNode>(); // Neighboring nodes
    public bool isOccupied = false; // Whether it's occupied by an obstacle or table

    // Pathfinding-related properties
    public int gridX, gridY; // Optional if using a grid
    public int gCost, hCost; // Cost values for pathfinding
    public PathNode parent; // Parent node for retracing paths

    public int fCost
    {
        get { return gCost + hCost; }
    }

    // Method to get the position of the node (same as world position)
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // Method to check if the node is occupied (by an obstacle or table)
    public bool IsOccupied()
    {
        return isOccupied;
    }

    // Set node as occupied or available
    public void SetOccupied(bool state)
    {
        isOccupied = state;
    }
}
