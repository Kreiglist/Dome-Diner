using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasResizer : MonoBehaviour
{
    public Camera targetCamera; // Reference to the camera to fit the canvas to

    private Canvas canvas;

    private void Start()
    {
        // Get the Canvas component
        canvas = GetComponent<Canvas>();

        // Ensure the target camera is assigned, default to main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        // Resize the Canvas to match the camera's viewport
        ResizeCanvasToCamera();
    }

    private void ResizeCanvasToCamera()
    {
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            RectTransform rectTransform = canvas.GetComponent<RectTransform>();

            // Get camera bounds
            float screenHeight = targetCamera.orthographicSize * 2f;
            float screenWidth = screenHeight * targetCamera.aspect;

            // Adjust canvas size
            rectTransform.sizeDelta = new Vector2(screenWidth, screenHeight);

            // Position the canvas in front of the camera (you can adjust the distance if needed)
            Vector3 cameraPosition = targetCamera.transform.position;
            rectTransform.position = cameraPosition + targetCamera.transform.forward * 5f; // 5f is the distance in front of the camera

            // Optionally adjust the canvas rotation to align it with the camera's view (if needed)
            rectTransform.rotation = Quaternion.LookRotation(targetCamera.transform.forward);
        }
        else
        {
            Debug.LogWarning("Canvas is not in World Space render mode. Adjusting the size won't have an effect.");
        }
    }
}
