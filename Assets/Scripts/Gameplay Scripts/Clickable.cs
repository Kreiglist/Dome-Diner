using UnityEngine;

public class Clickable : MonoBehaviour
{
    public PathNode associatedNode; // The PathNode this object points to

    private void OnMouseDown()
    {
        if (associatedNode == null)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.QueueMovement(associatedNode);
            }
        }
    }
}