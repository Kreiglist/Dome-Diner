using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Transform nodeToMoveTo; // The node the player should move to when this object is clicked

    void OnMouseDown()
    {
        // Find the player GameObject and queue the movement to the target node
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.QueueMovement(nodeToMoveTo); // Queue the movement
            }
        }
    }
}