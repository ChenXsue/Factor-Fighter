using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    private MathProblem currentProblem;
    public GameObject buttonPrefab;  // Prefab for the operator button to be instantiated
    public GameObject operatorsPanel; // Panel where operator buttons will be displayed

    public static bool isGamePaused = false;

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
        Debug.LogError(operator1.OperatorInstance.Operators_Bag.Count);

        char operatorToAdd = ' ';
        // Check which enemy was defeated and decide which operator to add
        if (this.gameObject.CompareTag("Enemy1"))
        {
            operator1.OperatorInstance.Operators_Bag.Add('*');
            operatorToAdd = '*';
        }
        else if (this.gameObject.CompareTag("Enemy2"))
        {
            operator1.OperatorInstance.Operators_Bag.Add('+');
            operatorToAdd = '+';
        }
        
        // Create a new button dynamically
        GameObject newButton = Instantiate(buttonPrefab); // Instantiate the button prefab
        newButton.transform.SetParent(operatorsPanel.transform, false); // Set it as a child of the operator panel

        // Set the button's text to display the operator
        newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = operatorToAdd.ToString();

        // Add the DraggableItem script dynamically to the new button
        newButton.AddComponent<DraggableItem>();
        newButton.name = operatorToAdd.ToString();
        newButton.SetActive(true);
        // Optionally, destroy the defeated enemy object
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched the enemy has the "Player" tag
        if (other.CompareTag("Player"))
        {
            PauseGame();
            // Show the math problem when the player touches the enemy
            MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
            mathUI.ShowMathProblem(this);  // Show the math problem UI
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