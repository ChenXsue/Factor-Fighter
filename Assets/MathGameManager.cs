using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MathGameManager : MonoBehaviour
{
    public TMPro.TMP_InputField userInputField; // Input field for user expression
    public TMPro.TextMeshProUGUI resultText; // Target result text (e.g., "= 12")
    public GameObject quizPanel;
    public GameObject passPanel;
    


    void Start()
    {
        // Get the current level name dynamically
        string levelName = SceneManager.GetActiveScene().name;
        
        // Start the timer for the current level    
        GameTimer.Instance.StartTimer(levelName);

    }

    void Update()
    {
        if (quizPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            CalculateResult();
        }
    }

    public void CalculateResult()
    {
        string expression = userInputField.text;

        Debug.Log("Evaluating expression: '" + expression + "'");

        if (string.IsNullOrEmpty(expression))
        {
            Debug.LogError("Expression is empty or invalid!");
            return;
        }

        float result = EvaluateExpression(expression);
        string cleanedResultText = resultText.text.Replace("=", "").Trim();

        if (!string.IsNullOrEmpty(cleanedResultText))
        {
            float targetNumber;

            bool isValidNumber = float.TryParse(cleanedResultText, out targetNumber);

            if (isValidNumber)
            {
                if (Mathf.Approximately(result, targetNumber))
                {
                    quizPanel.SetActive(false);
                    passPanel.SetActive(true);

                    Debug.Log("Success! You have succeeded.");

                    // Stop the timer at the end of the level
                    GameTimer.Instance.StopTimer();
                }
                else
                {
                    Debug.Log("Try again! The result is incorrect.");

                    GameTimer.Instance.AddTimePoint();
                }
            }
            else
            {
                Debug.LogError("Invalid number format in resultText: " + cleanedResultText);
            }
        }
        else
        {
            Debug.LogError("resultText is null or empty!");
        }
    }

    private float EvaluateExpression(string expression)
    {
        try
        {
            expression = expression.Replace("×", "*");
            expression = expression.Replace("÷", "/");
            System.Data.DataTable table = new System.Data.DataTable();
            var result = table.Compute(expression, null).ToString();

            Debug.Log("Evaluated result: " + result);
            return float.Parse(result);
        }
        catch (Exception e)
        {
            Debug.LogError("Error in evaluating expression: " + expression + " - " + e.Message);
            return 0f;
        }
    }
}