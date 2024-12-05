using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        // When clicked, load the main menu
        SceneManager.LoadScene("Level_1");  // Calls the RestartLevel method in EscapeMenu
    }

}