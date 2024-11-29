using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitToMap : MonoBehaviour
{
    public GameObject mapObject; // Drag your map GameObject here
    public float padding = 1f; // Additional space around the map
    public bool maintainAspectRatio = true; // Maintain the aspect ratio

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (mapObject != null)
        {
            FitCameraToMap();
        }
        else
        {
            Debug.LogError("Map Object is not assigned! Please assign a map GameObject.");
        }
    }

    void FitCameraToMap()
    {
        Renderer mapRenderer = mapObject.GetComponent<Renderer>();

        if (mapRenderer != null)
        {
            // Get the bounds of the map
            Bounds mapBounds = mapRenderer.bounds;

            // Calculate the size of the map
            float mapWidth = mapBounds.size.x;
            float mapHeight = mapBounds.size.y;

            // Calculate the required orthographic size
            float verticalSize = mapHeight / 2f + padding;
            float horizontalSize = (mapWidth / 2f + padding) / cam.aspect;

            cam.orthographic = true; // Ensure the camera is in orthographic mode
            cam.orthographicSize = maintainAspectRatio ? Mathf.Max(verticalSize, horizontalSize) : verticalSize;

            // Center the camera on the map
            cam.transform.position = new Vector3(
                mapBounds.center.x,
                mapBounds.center.y,
                cam.transform.position.z
            );

            Debug.Log($"Camera adjusted to fit the map with bounds: {mapBounds.size}");
        }
        else
        {
            Debug.LogError("The map object does not have a Renderer component!");
        }
    }
}
