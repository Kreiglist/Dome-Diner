using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class CameraCutscene : MonoBehaviour
{
    public Transform startPoint; // Camera's starting position
    public Transform endPoint;   // Camera's ending position
    public float panDuration = 5f; // Time to pan from start to end
    public Graphic[] uiElements; // Array of UI elements (Image, Text, etc.) to fade in
    public SpriteRenderer[] spriteRenderers; // Array of SpriteRenderers to fade in
    public float fadeDuration = 2f; // Time for fading in

    private Camera mainCamera;

    void Start()
    {
                                                                                           
        Time.timeScale = 1f;  // Resume the game (normal time574632  progression)
        mainCamera = Camera.main;
        mainCamera.transform.position = startPoint.position;

        // Set UI elements' and SpriteRenderers' alpha to 0 (invisible) at the start
        foreach (Graphic uiElement in uiElements)
        {
            if (uiElement != null)
            {
                Color color = uiElement.color;
                color.a = 0;
                uiElement.color = color;
            }
        }

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 0;
                spriteRenderer.color = color;
            }
        }

        StartCoroutine(PanCamera());
    }

    private IEnumerator PanCamera()
    {
        float elapsedTime = 0f;

        while (elapsedTime < panDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, elapsedTime / panDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = endPoint.position; // Ensure the camera is in the final position

        // Start fading in the UI elements and SpriteRenderers
        StartCoroutine(FadeInElements());
    }

    private IEnumerator FadeInElements()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Fade in UI elements
            foreach (Graphic uiElement in uiElements)
            {
                if (uiElement != null)
                {
                    Color color = uiElement.color;
                    color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration); // Smoothly increase alpha
                    uiElement.color = color;
                }
            }

            // Fade in SpriteRenderers
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration); // Smoothly increase alpha
                    spriteRenderer.color = color;
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure all UI elements and SpriteRenderers are fully visible
        foreach (Graphic uiElement in uiElements)
        {
            if (uiElement != null)
            {
                Color color = uiElement.color;
                color.a = 1;
                uiElement.color = color;
            }
        }

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1;
                spriteRenderer.color = color;
            }
        }
    }
}
