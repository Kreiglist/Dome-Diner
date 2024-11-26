using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public NodeType nodeType;  // Define if this is a movement or obstacle node
    public List<Node> connectedNodes;  // Direct neighbors of this node

    private bool isActive = true;  // If the node is accessible
    private bool hasObstacle = false;  // If there's an obstacle blocking the node

    void Start()
    {
        if (nodeType == NodeType.ObstacleNode)
        {
            SetObstacle(true);  // Initially set obstacle nodes to blocked
        }
    }

    public void SetObstacle(bool state)
    {
        hasObstacle = state;
        isActive = !hasObstacle;

        foreach (Node neighbor in connectedNodes)
        {
            if (neighbor.nodeType == NodeType.MovementNode)
            {
                // Block or unblock connections to neighbors based on obstacle state
                if (hasObstacle)
                {
                    neighbor.connectedNodes.Remove(this);
                }
                else if (!neighbor.connectedNodes.Contains(this))
                {
                    neighbor.connectedNodes.Add(this);
                }
            }
        }
    }

    public bool IsActive()
    {
        return isActive && nodeType == NodeType.MovementNode;
    }
}
