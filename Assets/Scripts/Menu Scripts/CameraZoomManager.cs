using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoomManager : MonoBehaviour
{
    public Camera mainCamera; // Reference to the Main Camera
    public CanvasGroup fadeCanvasGroup; // Canvas group for fade to black effect
    public float zoomSpeed = 2f; // Speed of the camera zoom
    public float fadeSpeed = 2f; // Speed of the fade to black
    public string level1SceneName = "Level1"; // Name of the level 1 scene

    private bool isZooming = false;

    private void Update()
    {
        if (isZooming)
        {
            // Zoom in the camera
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 2f, Time.deltaTime * zoomSpeed);

            // Gradually fade to black
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            }

            // Transition to the next scene when the fade is complete
            if (fadeCanvasGroup.alpha >= 1f)
            {
                SceneManager.LoadScene(level1SceneName);
            }
        }
    }

    public void StartZoom()
    {
        isZooming = true;
    }
}
