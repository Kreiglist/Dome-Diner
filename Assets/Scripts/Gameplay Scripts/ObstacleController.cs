using UnityEngine;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour
{
    public List<Node> obstacles;  // All obstacle nodes in the scene

    public void ActivateObstacle(Node obstacleNode, bool active)
    {
        if (obstacleNode.nodeType == NodeType.ObstacleNode)
        {
            obstacleNode.SetObstacle(active);  // Set obstacle state for the node
        }
    }

    // Example method to toggle an obstacle state
    public void ToggleObstacle(Node obstacleNode)
    {
        if (obstacleNode.nodeType == NodeType.ObstacleNode)
        {
            bool isActive = obstacleNode.IsActive();
            obstacleNode.SetObstacle(!isActive);  // Toggle obstacle status
        }
    }
}
