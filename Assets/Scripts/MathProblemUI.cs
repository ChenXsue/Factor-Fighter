using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathProblemUI : MonoBehaviour
{
    public TextMeshProUGUI problemText;  // TextMeshPro component for displaying the math problem
    public TMP_InputField answerInputField;  // InputField for player input
    public GameObject mathProblemPanel;  // Panel to show and hide the math problem UI
    private Enemy currentEnemy;
    private RoomDoor currentRoomDoor;

    // Display the math problem
    public void ShowMathProblem(Enemy enemy)
    {
        currentEnemy = enemy;
        problemText.text = enemy.GetMathProblem();  // Display the math problem
        answerInputField.text = "";  // Clear the input field
        answerInputField.ActivateInputField();  // Activate the input field for player input
        mathProblemPanel.SetActive(true);  // Show the math problem panel
    }
    public void ShowMathProblem(RoomDoor roomDoor)
    {
        currentRoomDoor = roomDoor;
        problemText.text = roomDoor.GetMathProblem();  // Display the math problem
        answerInputField.text = "";  // Clear the input field
        answerInputField.ActivateInputField();  // Activate the input field for player input
        mathProblemPanel.SetActive(true);  // Show the math problem panel
    }

    // Check the player's input answer
    public void CheckAnswer()
    {
        int playerAnswer;

        // Safely parse player's input and handle non-numeric input
        if (int.TryParse(answerInputField.text, out playerAnswer))
        {
            if (currentEnemy != null)
            {
                if (currentEnemy.CheckAnswer(playerAnswer))  // Correct answer
                {
                    currentEnemy.Defeat();  // Defeat the enemy
                    mathProblemPanel.SetActive(false);  // Hide the math problem panel
                    Debug.Log("correct");  // Output correct message
                }
                else  // Incorrect answer
                {
                    Debug.Log("wrong");  // Output incorrect message
                    
                }
            }
            if (currentRoomDoor != null)
            {
                if (currentRoomDoor.CheckAnswer(playerAnswer))  // Correct answer
                {
                    currentRoomDoor.Defeat();  // Defeat the enemy
                    mathProblemPanel.SetActive(false);  // Hide the math problem panel
                    Debug.Log("correct");  // Output correct message
                }
                else  // Incorrect answer
                {
                    Debug.Log("wrong");  // Output incorrect message
                    
                }
            }
            
            mathProblemPanel.SetActive(false);
            Enemy.ResumeGame();
        }
        else  // Invalid (non-numeric) input
        {
            Debug.Log("Invalid input!");  // Output invalid input message
            
        }
    }
}