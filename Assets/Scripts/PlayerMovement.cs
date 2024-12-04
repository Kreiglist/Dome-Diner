using UnityEngine;
using System;
using System.Collections; // Required for IEnumerator
using System.Collections.Generic; // If you're using Lists or similar
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public Animator animator; // Animator for character animations
    public PathNode initialSpawnNode; // The initial spawn node for the player
    private PathNode currentNode; // Current node the player is at
    private Queue<PathNode> movementQueue = new Queue<PathNode>(); // Queue to store the path to follow
    private PathNode currentTarget; // Current node the player is moving towards
    public bool isMoving = false; // Whether the player is currently moving
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
        if (currentTarget != null)
        {
            MoveTowardsTarget();
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

            if (targetPosition.y > currentPosition.y)
            {
                // Upward movement
                animator.Play("astro_walk_back");
            }
            else if (targetPosition.y < currentPosition.y)
            {
                // Downward movement
                animator.Play("astro_walk_front");
            }
            else
            {
                animator.SetBool("isMovingUp", false);
            }

            if (Mathf.Abs(currentPosition.y - targetPosition.y) < 0.1f)
            {
                FinishMovement();
                animator.Play("astro_idle");
            }
        }

        animator.SetFloat("moveSpeed", moveSpeed);
    }

private void FinishMovement()
{
    //Debug.Log($"Arrived at target: {currentTarget.name}");
    isMoving = false;
    currentNode = currentTarget; // Update the current node to the target node
    currentTarget = null;
    Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // Check nearby colliders
    foreach (Collider collider in colliders)
    {
        Table table = collider.GetComponent<Table>();
        if (table != null)
        {
            // table.ProcessInteraction();
            Debug.Log($"Player reached and interacting with Table {table.tableID}");
        }
    }

    if (movementQueue.Count > 0)
    {
        MoveToNextNode();
    }
    //else
    //{
    //    Debug.Log("No more nodes in the queue.");
    //}

}
    private IEnumerator isMovingCooldown()
    {
        yield return new WaitForSeconds(1f);
        
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
            //Debug.LogError($"No path found from {currentNode.name} to {targetNode.name}");
        }
    }

    private void MoveToNextNode()
    {
        if (movementQueue.Count > 0)
        {
            currentTarget = movementQueue.Dequeue();
            isMoving = true;
            moveHorizontally = true;
            //Debug.Log($"Moving to next node: {currentTarget.name}");
        }
    }

    private List<PathNode> FindPath(PathNode startNode, PathNode targetNode)
{
    if (startNode == null || targetNode == null)
    {
        //Debug.LogError($"FindPath called with invalid nodes: startNode = {startNode}, targetNode = {targetNode}");
        return null;
    }

    // Initialize the data structures
    Dictionary<PathNode, int> distances = new Dictionary<PathNode, int>();
    Dictionary<PathNode, PathNode> previousNodes = new Dictionary<PathNode, PathNode>();
    HashSet<PathNode> visitedNodes = new HashSet<PathNode>();
    PriorityQueue<PathNode, int> priorityQueue = new PriorityQueue<PathNode, int>();

    // Set all distances to infinity, except for the start node
    foreach (PathNode node in GetAllNodes())
    {
        distances[node] = int.MaxValue;
        previousNodes[node] = null;
    }
    distances[startNode] = 0;

    // Enqueue the start node
    priorityQueue.Enqueue(startNode, 0);

    // Main loop
    while (priorityQueue.Count > 0)
    {
        PathNode currentNode = priorityQueue.Dequeue();

        // Skip if the node has already been visited
        if (visitedNodes.Contains(currentNode))
        {
            continue;
        }
        visitedNodes.Add(currentNode);

        // Stop if we reached the target node
        if (currentNode == targetNode)
        {
            return RetracePath(startNode, targetNode, previousNodes);
        }

        // Process neighbors
        foreach (PathNode neighbor in currentNode.connectedNodes)
        {
            if (visitedNodes.Contains(neighbor) || neighbor.isOccupied)
            {
                continue;
            }

            int newDistance = distances[currentNode] + GetDistance(currentNode, neighbor);

            if (newDistance < distances[neighbor])
            {
                distances[neighbor] = newDistance;
                previousNodes[neighbor] = currentNode;
                priorityQueue.Enqueue(neighbor, newDistance);
            }
        }
    }

    // If we reach here, no path was found
    //Debug.LogWarning($"No path found from {startNode.name} to {targetNode.name}");
    return null;
}
public class PriorityQueue<TItem, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TItem Item, TPriority Priority)> elements = new List<(TItem, TPriority)>();

    public int Count => elements.Count;

    public void Enqueue(TItem item, TPriority priority)
    {
        elements.Add((item, priority));
        elements.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }

    public TItem Dequeue()
    {
        var bestItem = elements[0].Item;
        elements.RemoveAt(0);
        return bestItem;
    }
}

private IEnumerable<PathNode> GetAllNodes()
{
    return FindObjectsOfType<PathNode>();
}

    private List<PathNode> RetracePath(PathNode startNode, PathNode targetNode, Dictionary<PathNode, PathNode> previousNodes)
{
    List<PathNode> path = new List<PathNode>();
    PathNode currentNode = targetNode;

    while (currentNode != null && currentNode != startNode)
    {
        path.Add(currentNode);
        currentNode = previousNodes[currentNode];
    }

    path.Add(startNode);
    path.Reverse();
    return path;
}


    private int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dx = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dy = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dx + dy;
    }}

