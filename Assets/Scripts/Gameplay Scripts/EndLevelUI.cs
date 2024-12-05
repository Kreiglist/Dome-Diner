using UnityEngine;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    public TMP_Text customersServedText;
    public TMP_Text customersLeftText;
    
    public GameObject nextLevelButton;  // Reference to the Next Level Button GameObject
    public GameObject returnToMenuButton;  // Reference to the Return to Menu Button GameObject
    
    public GameObject endLevelUI;  // Reference to the End Level UI GameObject

    private Collider2D nextLevelButtonCollider;
    private Collider2D returnToMenuButtonCollider;

    void Start()
    {
        // Hide the EndLevelUI at the start
        endLevelUI.SetActive(false);

        // Get colliders from button GameObjects
        nextLevelButtonCollider = nextLevelButton.GetComponent<Collider2D>();
        returnToMenuButtonCollider = returnToMenuButton.GetComponent<Collider2D>();

        // Disable button colliders initially
        DisableButtons();
    }

    // Call this method from LevelManager when the level finishes
    public void ShowEndLevelUI(int customersServed, int customersLeft)
    {
        // Show the EndLevelUI
        endLevelUI.SetActive(true);

        // Update the UI with customer stats
        customersServedText.text = "Customers Served: " + customersServed;
        customersLeftText.text = "Customers Left: " + customersLeft;

        // Enable the buttons (reactivate colliders)
        EnableButtons();
    }

    // Disable button colliders to prevent clicks
    public void DisableButtons()
    {
        nextLevelButtonCollider.enabled = false;
        returnToMenuButtonCollider.enabled = false;
    }

    // Enable button colliders when the level finishes
    public void EnableButtons()
    {
        nextLevelButtonCollider.enabled = true;
        returnToMenuButtonCollider.enabled = true;
    }

    // Method to update UI (e.g., when you need to show customers served/left)
    public void UpdateUI(int customersServed, int customersLeft)
    {
        customersServedText.text = "" + customersServed;
        customersLeftText.text = "" + customersLeft;
    }
}
