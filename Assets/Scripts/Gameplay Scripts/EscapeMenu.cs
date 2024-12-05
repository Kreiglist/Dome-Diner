using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeMenuUI;  // Reference to the Escape Menu UI
    public GameObject gameUI;  // Reference to the main game UI (to hide when menu is open)
    public GameObject restartButton;  // Reference to the Restart Level Button
    public GameObject returnToMenuButton;  // Reference to the Return to Menu Button

    private Collider2D restartButtonCollider;
    private Collider2D returnToMenuButtonCollider;

    private bool isGamePaused = false;

    void Start()
    {
        escapeMenuUI.SetActive(false);  // Hide the escape menu at the start
        restartButtonCollider = restartButton.GetComponent<Collider2D>();
        returnToMenuButtonCollider = returnToMenuButton.GetComponent<Collider2D>();

        // Disable button colliders initially
        DisableButtonColliders();
    }

    void Update()
    {
        // Check if the player presses ESC to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    // Toggle the Escape Menu and pause the game
    void TogglePauseMenu()
    {
        if (isGamePaused)
        {
            ResumeGame();  // If the game is paused, resume it
        }
        else
        {
            ShowPauseMenu();  // Show the pause menu
        }
    }

    // Show the Escape Menu UI and pause the game
    void ShowPauseMenu()
    {
        escapeMenuUI.SetActive(true);  // Activate the Escape Menu UI
        gameUI.SetActive(false);  // Deactivate the main game UI

        // Enable the colliders for the buttons (to allow interaction)
        EnableButtonColliders();

        Time.timeScale = 0f;  // Pause the game (stop time progression)
        isGamePaused = true;
    }

    // Resume the game (hide the Escape Menu and restore the game state)
    void ResumeGame()
    {
        escapeMenuUI.SetActive(false);  // Deactivate the Escape Menu UI
        gameUI.SetActive(true);  // Reactivate the main game UI again

        // Disable the colliders for the buttons (to prevent interaction)
        DisableButtonColliders();

        Time.timeScale = 1f;  // Resume the game (normal time progression)
        isGamePaused = false;
    }

    // Restart the current level (scene)
    public void RestartLevel()
    {
        Time.timeScale = 1f;  // Make sure the game resumes before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Restart the current scene
    }

    // Load the main menu (or another scene for the menu)
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;  // Make sure the game resumes before loading the menu
        SceneManager.LoadScene("MainMenu");  // Replace with your main menu scene name
    }

    // Enable the colliders for the menu buttons
    void EnableButtonColliders()
    {
        restartButtonCollider.enabled = true;
        returnToMenuButtonCollider.enabled = true;
    }

    // Disable the colliders for the menu buttons
    void DisableButtonColliders()
    {
        restartButtonCollider.enabled = false;
        returnToMenuButtonCollider.enabled = false;
    }
}
