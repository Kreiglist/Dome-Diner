using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public Animator animator; // Animator for character animations
    public PathNode initialSpawnNode; // The initial spawn node for the player
    private PathNode currentNode; // Current node the player is at
    private Queue<PathNode> movementQueue = new Queue<PathNode>(); // Queue to store the path to follow
    private PathNode currentTarget; // Current node the player is moving towards
    private bool isMoving = false; // Whether the player is currently moving
    private bool moveHorizontally = true; // Control whether to move horizontally first

    private void Start()
    {
        if (initialSpawnNode != null)
        {
            currentNode = initialSpawnNode;
            transform.position = currentNode.transform.position; // Set player position to the initial node
            Debug.Log($"Player spawned at: {currentNode.name} at position {currentNode.transform.position}");
        }
        else
        {
            Debug.LogError("Initial spawn node is not set!");
        }
    }

    private void Update()
    {
        if (isMoving && currentTarget != null)
        {
            MoveTowardsTarget();
        }
        else if (!isMoving)
        {
            ResetAnimations();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = currentTarget.transform.position;
        Vector3 currentPosition = transform.position;

        if (moveHorizontally)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = new Vector3(
                Mathf.MoveTowards(currentPosition.x, targetPosition.x, step),
                currentPosition.y,
                currentPosition.z
            );

            HandleHorizontalAnimation(currentPosition, targetPosition);

            if (Mathf.Abs(currentPosition.x - targetPosition.x) < 0.1f)
            {
                moveHorizontally = false;
                animator.SetBool("isWalkingSide", false);
            }
        }
        else
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = new Vector3(
                currentPosition.x,
                Mathf.MoveTowards(currentPosition.y, targetPosition.y, step),
                currentPosition.z
            );

            if (Mathf.Abs(currentPosition.y - targetPosition.y) < 0.1f)
            {
                FinishMovement();
            }
        }

        animator.SetFloat("moveSpeed", moveSpeed);
    }

    private void FinishMovement()
    {
        Debug.Log($"Arrived at target: {currentTarget.name}");
        isMoving = false;
        currentNode = currentTarget; // Update the current node to the target node
        currentTarget = null;

        if (movementQueue.Count > 0)
        {
            MoveToNextNode();
        }
        else
        {
            Debug.Log("No more nodes in the queue.");
        }
    }

    private void HandleHorizontalAnimation(Vector3 currentPosition, Vector3 targetPosition)
    {
        if (targetPosition.x < currentPosition.x)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1; // Flip the x-axis while preserving size
            transform.localScale = scale;
            animator.SetBool("isWalkingSide", true);
        }
        else if (targetPosition.x > currentPosition.x)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // Reset the x-axis to default while preserving size
            transform.localScale = scale;
            animator.SetBool("isWalkingSide", true);
        }
    }

    private void ResetAnimations()
    {
        animator.SetFloat("moveSpeed", 0f);
        animator.SetBool("isMovingForward", false);
        animator.SetBool("isWalkingSide", false);
    }

    public void QueueMovement(PathNode targetNode)
    {
        // Perform pathfinding to generate the path
        List<PathNode> path = FindPath(currentNode, targetNode);

        if (path != null)
        {
            foreach (PathNode node in path)
            {
                movementQueue.Enqueue(node);
            }

            if (!isMoving)
            {
                MoveToNextNode();
            }
        }
        else
        {
            Debug.LogError($"No path found from {currentNode.name} to {targetNode.name}");
        }
    }

    private void MoveToNextNode()
    {
        if (movementQueue.Count > 0)
        {
            currentTarget = movementQueue.Dequeue();
            isMoving = true;
            moveHorizontally = true;
            Debug.Log($"Moving to next node: {currentTarget.name}");
        }
    }

   private List<PathNode> FindPath(PathNode startNode, PathNode targetNode)
{
    List<PathNode> openSet = new List<PathNode>(); // Nodes to evaluate
    HashSet<PathNode> closedSet = new HashSet<PathNode>(); // Nodes already evaluated
    openSet.Add(startNode);

    // Initialize the start node costs
    startNode.gCost = 0;
    startNode.hCost = GetDistance(startNode, targetNode);

    while (openSet.Count > 0)
    {
        PathNode currentNode = openSet[0];
        foreach (PathNode node in openSet)
        {
            if (node.fCost < currentNode.fCost || 
                (node.fCost == currentNode.fCost && node.hCost < currentNode.hCost))
            {
                currentNode = node;
            }
        }

        openSet.Remove(currentNode);
        closedSet.Add(currentNode);

        if (currentNode == targetNode)
        {
            return RetracePath(startNode, targetNode);
        }

        foreach (PathNode neighbor in currentNode.connectedNodes)
        {
            if (neighbor.isOccupied || closedSet.Contains(neighbor))
                continue;

            // Include node-specific movement cost
            int movementCost = neighbor.gCost > 0 ? neighbor.gCost : 1; // Use preset gCost or default to 1
            int newCostToNeighbor = currentNode.gCost + movementCost;

            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
            {
                neighbor.gCost = newCostToNeighbor;
                neighbor.hCost = GetDistance(neighbor, targetNode);
                neighbor.parent = currentNode;

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }
    }

    return null; // No path found
}

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

private int GetDistance(PathNode nodeA, PathNode nodeB)
{
    // Use grid-based Manhattan distance
    int dx = Mathf.Abs(nodeA.gridX - nodeB.gridX);
    int dy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

    return dx + dy;
}
}
