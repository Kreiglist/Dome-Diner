using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        // When clicked, load the next level
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        // Load the next level in the build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
