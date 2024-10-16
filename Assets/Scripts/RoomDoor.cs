using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    private MathProblem currentProblem;
    public GameObject buttonPrefab;  // Prefab for the operator button to be instantiated
    public GameObject operatorsPanel; // Panel where operator buttons will be displayed

    public static bool isGamePaused = false;

    private bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        Ene_MathProblemManager problemManager = FindObjectOfType<Ene_MathProblemManager>();
        
        currentProblem = problemManager.GetRandomMathProblem();

        Debug.Log("Math problem for this enemy: " + currentProblem.question);

    }

    public string GetMathProblem()
    {
        return currentProblem.question;
    }

    public bool CheckAnswer(int playerAnswer)
    {
        return playerAnswer == currentProblem.answer;
    }

    public void Defeat()
    {
        Debug.LogError("Room Door unlocked");

        int numberToAdd = 0;
        // Check which door was touched to decide which number to add
        if (this.gameObject.CompareTag("Door1"))
        {
            // Add number to list
            // Number to add = 
        }
        else if (this.gameObject.CompareTag("Door2"))
        {
            // Add number to list
            // Number to add =
        }
        
        // Create a new button dynamically
        GameObject newButton = Instantiate(buttonPrefab); // Instantiate the button prefab
        // Set it as a child of the numbers panel

        // Set the button's text to display the number

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding with the door is the player
        if (other.gameObject.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            PauseGame();
            // Check if the player is passing through the door the first time
            if (!firstTime)
            {
                // Go to the next room
                SceneManager.LoadSceneAsync("Room2");   // suppose next room is called "Room2"
            }
            else
            {
                // Show the math problem when the player touches the door
                MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
                mathUI.ShowMathProblem(this);   // Show the math problem UI
                firstTime = false;
                SceneManager.LoadSceneAsync("Room2");   // suppose next room is called "Room2"
            }
        }
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f; // 停止游戏时间
        isGamePaused = true;
        Debug.Log("Game paused");
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f; // 恢复正常游戏时间
        isGamePaused = false;
        Debug.Log("Game resumed");
    }
}
