using UnityEngine;

public class Clickable : MonoBehaviour
{
    public PathNode associatedNode; // The PathNode this object points to

    private void OnMouseDown()
    {
        if (associatedNode == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have an associated PathNode.");
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
            else
            {
                Debug.LogError("Player is missing the PlayerMovement script.");
            }
        }
        else
        {
            Debug.LogError("No GameObject tagged 'Player' found in the scene.");
        }
    }
}