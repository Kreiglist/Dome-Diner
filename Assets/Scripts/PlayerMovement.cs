using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    private Queue<Transform> movementQueue = new Queue<Transform>(); // Queue to store movement targets
    private Transform currentTarget; // The current node the player is moving towards
    private bool isMoving = false; // Whether the player is currently moving
    private bool moveHorizontally = true; // Control whether to move horizontally first
    public Animator animator;

    void Update()
    {
        if (isMoving && currentTarget != null)
        {
            if (moveHorizontally)
            {
                // Move horizontally towards the target's X position
                float step = moveSpeed * Time.deltaTime;
                Vector3 newPos = new Vector3(
                    Mathf.MoveTowards(transform.position.x, currentTarget.position.x, step),
                    transform.position.y,
                    transform.position.z
                );
                transform.position = newPos;

                // If moving forward, enable the forward animation
                if (currentTarget.position.y > transform.position.y)
                {
                    animator.SetBool("isMovingForward", true); // Play forward movement animation
                }
                else
                {
                    animator.SetBool("isMovingForward", false); // Stop forward movement animation
                }

                // Flip the sprite based on the direction of movement
                if (currentTarget.position.x < transform.position.x)
                {
                    // Moving left, flip the sprite
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (currentTarget.position.x > transform.position.x)
                {
                    // Moving right, set sprite to default scale
                    transform.localScale = new Vector3(1, 1, 1);
                }

                    // If horizontal movement is complete, switch to vertical movement
                    if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.1f)
                {
                    moveHorizontally = false;
                }
            }
            else
            {
                // Move vertically towards the target's Y position after horizontal movement is done
                float step = moveSpeed * Time.deltaTime;
                Vector3 newPos = new Vector3(
                    transform.position.x,
                    Mathf.MoveTowards(transform.position.y, currentTarget.position.y, step),
                    transform.position.z
                );
                transform.position = newPos;

                // If vertical movement is complete, stop moving and dequeue the next target
                if (Mathf.Abs(transform.position.y - currentTarget.position.y) < 0.1f)
                {
                    isMoving = false;
                    currentTarget = null;

                    // Check if there are more nodes in the queue to move to
                    if (movementQueue.Count > 0)
                    {
                        MoveToNextNode();
                    }
                }
            }
            // Set animator's moveSpeed parameter based on movement speed
            animator.SetFloat("moveSpeed", moveSpeed);
        }
        else
        {
            // If the player is not moving, set the moveSpeed to 0
            animator.SetFloat("moveSpeed", 0f);
            animator.SetBool("isMovingForward", false);
        }
    }

    // Call this method when a new object is clicked
    public void QueueMovement(Transform targetNode)
    {
        // Add the clicked node to the movement queue
        movementQueue.Enqueue(targetNode);

        // If the player is not currently moving, start moving towards the first node
        if (!isMoving)
        {
            MoveToNextNode();
        }
    }

    // Move to the next node in the queue
    private void MoveToNextNode()
    {
        if (movementQueue.Count > 0)
        {
            currentTarget = movementQueue.Dequeue();
            isMoving = true;
            moveHorizontally = true; // Start by moving horizontally first
        }
    }
}
