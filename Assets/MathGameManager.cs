using System;
using UnityEngine;


//通关门的文本解析与判断
public class MathGameManager : MonoBehaviour
{
    public TMPro.TMP_InputField userInputField; // Input field for user expression
    public TMPro.TextMeshProUGUI resultText; // Target result (e.g., "= 12")
    public GameObject quizPanel;
    public GameObject passPanel;

    private string expression;
    


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

    // This method is called when the user clicks the "Calculate" button
    public void CalculateResult()
    {
        string expression = userInputField.text; // Get the expression from the input field

        // Debug to check the input expression
        string expression = userInputField.text;
        this.expression = expression;
        Debug.Log("Evaluating expression: '" + expression + "'");

        if (string.IsNullOrEmpty(expression))
        {
            Debug.LogError("Expression is empty or invalid!");
            return;
        }

        float result = EvaluateExpression(expression); // Evaluate the mathematical expression

        // Clean up the resultText to remove any non-numeric characters (such as '=')
        string cleanedResultText = resultText.text.Replace("=", "").Trim();  // Remove '=' and any extra spaces

        if (!string.IsNullOrEmpty(cleanedResultText))
        {
            float targetNumber;

            // Try to parse the cleaned resultText into a float
            bool isValidNumber = float.TryParse(cleanedResultText, out targetNumber);

            if (isValidNumber)
            {
                // Compare the calculated result with the target number
                if (Mathf.Approximately(result, targetNumber))
                {
                    quizPanel.SetActive(false);
                    passPanel.SetActive(true);

                    Debug.Log("Success! You have succeeded.");


                    // Stop the timer at the end of the level
                    GameTimer.Instance.StopTimer(expression);
                }
                else
                {
                    Debug.Log("Try again! The result is incorrect.");
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

    // Function to evaluate a mathematical expression
    private float EvaluateExpression(string expression)
    {
        try
        {
            // Ensure that '*' is used for multiplication and other operators are correct
            expression = expression.Replace("×", "*");  // Handle potential symbol issues

            System.Data.DataTable table = new System.Data.DataTable();
            var result = table.Compute(expression, null).ToString();

            Debug.Log("Evaluated result: " + result);  // Log the evaluated result
            return float.Parse(result);
        }
        catch (Exception e)
        {
            Debug.LogError("Error in evaluating expression: " + expression + " - " + e.Message);
            return 0f;
        }
    }
}