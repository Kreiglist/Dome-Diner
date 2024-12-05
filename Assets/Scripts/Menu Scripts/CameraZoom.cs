using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CameraZoom : MonoBehaviour
{
    public float zoomDuration = 2f; // Duration of the zoom in seconds
    public float targetZoomSize = 2f; // Final orthographic size for the zoom
    public SpriteRenderer fadeSprite; // Sprite Renderer for the fade-to-black effect
    public float fadeDuration = 1f; // Duration of the fade
    public Transform targetPrefab; // The prefab or object to zoom toward
    public float moveDuration = 2f; // Duration to move toward the prefab in seconds
    public string levelToLoadName = "Level_1";
    private Camera mainCamera;
    private bool isZooming = false;

    private void Start()
    {
        mainCamera = Camera.main;

        if (fadeSprite != null)
        {
            // Set the sprite to fully transparent at the start
            Color color = fadeSprite.color;
            color.a = 0;
            fadeSprite.color = color;
        }
    }

    public void StartZoom()
    {
        if (!isZooming)
        {
            isZooming = true;
            StartCoroutine(ZoomAndMoveToTargetRoutine());
        }
    }

    private System.Collections.IEnumerator ZoomAndMoveToTargetRoutine()
    {
        if (targetPrefab == null)
        {
            Debug.LogError("Target prefab is not assigned!");
            yield break;
        }


        float elapsedTime = 0f;
        float initialZoomSize = mainCamera.orthographicSize;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = new Vector3(targetPrefab.position.x, targetPrefab.position.y, transform.position.z);

        bool fadeStarted = false;

        while (elapsedTime < Mathf.Max(zoomDuration, moveDuration))
        {
            if (elapsedTime < zoomDuration)
            {
                mainCamera.orthographicSize = Mathf.Lerp(initialZoomSize, targetZoomSize, elapsedTime / zoomDuration);
            }

            if (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            }

            if (!fadeStarted && elapsedTime >= Mathf.Max(zoomDuration, moveDuration) - fadeDuration)
            {
                fadeStarted = true;
                StartCoroutine(FadeToBlack());
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetZoomSize;
        transform.position = targetPosition;

        if (fadeStarted)
        {
            yield return new WaitForSeconds(fadeDuration);
        }

                SceneManager.LoadScene(levelToLoadName);
    }

    private System.Collections.IEnumerator FadeToBlack()
    {
        if (fadeSprite != null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                Color color = fadeSprite.color;
                color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                fadeSprite.color = color;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Color finalColor = fadeSprite.color;
            finalColor.a = 1;
            fadeSprite.color = finalColor;
        }
    }
}
