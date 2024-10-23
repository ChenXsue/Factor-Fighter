using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SideMenuController : MonoBehaviour
{
    // Assign the Panel (side menu) in the Inspector
    public GameObject sideMenu;

    // To keep track of the menu's visibility state
    private bool isMenuVisible = false;

    // Call this function to toggle the side menu
    public void ToggleSideMenu()
    {
        isMenuVisible = !isMenuVisible; // Switch the visibility state
        sideMenu.SetActive(isMenuVisible); // Set the panel active or inactive

        // If the menu is visible, freeze the game
        if (isMenuVisible){
            Time.timeScale = 0;
        }
        // If the menu is not visible, resume the game
        if (!isMenuVisible){
            Time.timeScale = 1;
        }
    }

    // Function to resume the game
    public void ResumeGame()
    {
        ToggleSideMenu(); // Hide the menu and resume the game
    }

    // Function to go back to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reset the time scale in case the game was paused
        // SceneManager.LoadScene("MainMenu"); // Load the main menu scene (replace with your main menu scene name)
    }

    // Function to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1; // Reset the time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    
}
