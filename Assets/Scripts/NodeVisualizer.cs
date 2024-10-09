using UnityEngine;

public class NodeVisualizer : MonoBehaviour
{
    public Transform[] connectedNodes; // Array of nodes this node is connected to
    public Color lineColor = Color.green;


    void OnDrawGizmos()
    {
        // Draw a line from this node to all connected nodes
        if (connectedNodes != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform node in connectedNodes)
            {
                if (node != null)
                {
                    Gizmos.DrawLine(transform.position, node.position);
                }
            }
        }
    }
}