using UnityEngine;

public class AspectRatioFitter : MonoBehaviour
{
    public Camera mainCamera;

    // The target aspect ratio (16:10)
    private float targetAspect = 16f / 10f;

    void Start()
    {
        // If you don't assign a camera in the Inspector, it defaults to the main camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Adjust the camera to fit the 16:10 aspect ratio
        AdjustAspectRatio();
    }

    void AdjustAspectRatio()
    {
        // Get the current screen aspect ratio
        float screenAspect = (float)Screen.width / Screen.height;

        // Calculate the difference in aspect ratios
        float scaleHeight = screenAspect / targetAspect;

        // If the current aspect ratio is wider than 16:10, add letterboxing (black bars)
        if (scaleHeight < 1.0f)
        {
            // Change the viewport to letterbox
            Rect rect = mainCamera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = rect;
        }
        // If the current aspect ratio is taller than 16:10, add pillarboxing (black bars on sides)
        else
        {
            // Change the viewport to pillarbox
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCamera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCamera.rect = rect;
        }
    }
}
