using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    private MathProblem currentProblem;
    //public GameObject buttonPrefab;  // Prefab for the operator button to be instantiated
    public OperatorSO operatorData;
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
        if (operatorData != null)
        {
            OperatorInventoryManager.instance.AddOperator(operatorData);
            Debug.Log($"Added operator {operatorData.operatorChar} to inventory");
        }
        else
        {
            Debug.LogWarning("No operator set for this enemy!");
        }

        // 销毁敌人对象
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
        Time.timeScale = 0f; 
        isGamePaused = true;
        Debug.Log("Game paused");
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f; 
        isGamePaused = false;
        Debug.Log("Game resumed");
    }

}