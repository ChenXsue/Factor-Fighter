using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // Reference to the "Number" panel under "DoorCanvas"
    public GameObject quizPanel;     // Quiz panel to show when the player reaches the door
    public GameObject operatorsPanel; // Panel where operator buttons will be displayed
    //public GameObject buttonPrefab;  // Prefab for the operator button to be instantiated
    public GameObject input;

    private void Start()
    {
        // Initially hide the quiz and equation panels
        quizPanel.SetActive(false);
        operatorsPanel.SetActive(true);
        input.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding with the door is the player
        if (other.gameObject.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            // Show the quiz panel and equation panel when the player touches the door
            quizPanel.SetActive(true);
            operatorsPanel.SetActive(true);
            input.SetActive(true);
            //Enemy.PauseGame();
            //Time.timeScale = 0f;

            // Call the method to create buttons for each operator in Operators_Bag
            // CreateOperatorButtons();
        }
    }


}