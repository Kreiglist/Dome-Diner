using UnityEngine;

public class StartGameUIManager : MonoBehaviour
{
    public GameObject startGameButton; // The Start Game UI button
    public CameraZoomManager cameraZoomManager; // Reference to the zooming script

    public void OnHover()
    {
        // Hide the Start Game UI on hover
        startGameButton.SetActive(false);
    }

    public void OnStartGameClick()
    {
        // Trigger the camera zoom and fade via the CameraZoomManager
        if (cameraZoomManager != null)
        {
            cameraZoomManager.StartZoom();
        }
    }
}
