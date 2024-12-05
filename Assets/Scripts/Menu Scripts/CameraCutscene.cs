using UnityEngine;

public class CameraCutscene : MonoBehaviour
{
    public Transform startPoint; // The camera's starting position (on the right)
    public Transform endPoint;   // The camera's ending position (on the left)
    public float panDuration = 5f; // Time it takes to pan from start to end

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.position = startPoint.position;
        StartCoroutine(PanCamera());
    }

    private System.Collections.IEnumerator PanCamera()
    {
        float elapsedTime = 0f;

        while (elapsedTime < panDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, elapsedTime / panDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = endPoint.position; // Ensure final position is correct

        // Call method to show the UI after the pan
        //ShowUI();
    }

    private void ShowUI()
    {
        // Activate your UI elements (e.g., "Start Game" button)
        GameObject.Find("StartGameUI").SetActive(true);
    }
}
