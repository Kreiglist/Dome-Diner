using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        // When clicked, load the main menu
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("Main Menu");
           // Replace with your actual scene name
    }
}
