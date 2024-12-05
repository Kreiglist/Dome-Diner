using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{ 
    private CameraZoom cameraZoom; // Reference to the camera zoom script

    private void Start()
    {
        
        // Find the CameraZoom script attached to the Main Camera
        cameraZoom = Camera.main.GetComponent<CameraZoom>();

        if (cameraZoom == null)
        {
            Debug.LogError("CameraZoom script not found on the Main Camera!");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Start Game Button Clicked!");
        //SceneManager.LoadScene("Level_1");
        // Trigger the camera zoom effect
        if (cameraZoom != null)
        {
            cameraZoom.StartZoom();
        }
    }

}
