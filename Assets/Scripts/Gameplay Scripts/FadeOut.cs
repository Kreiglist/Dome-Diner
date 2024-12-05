using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration to fade out in seconds
    private Renderer objectRenderer; // Renderer of the object to fade
    private Color originalColor; // The original color of the object

    void Start()
    {
        // Get the Renderer component
        objectRenderer = GetComponent<Renderer>();

        // Store the original color of the object
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        // Start the fade out process
        StartCoroutine(FadeOutCoroutine());
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color currentColor = originalColor;

        // Gradually reduce the alpha value to 0 over the duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            // Set the alpha value of the color
            currentColor.a = alpha;
            objectRenderer.material.color = currentColor;

            yield return null; // Wait until the next frame
        }

        // Ensure the alpha is exactly 0 at the end of the fade
        currentColor.a = 0f;
        objectRenderer.material.color = currentColor;
    }
}
