using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MathGameManager : MonoBehaviour
{
    public GameObject equationPanel; // Reference to the 'equation' panel
    public TMPro.TextMeshProUGUI resultText; // Reference to the target result (e.g., "= 12")
    public TMPro.TextMeshProUGUI userInputText; // Displays the current expression
    private string currentInput = ""; // Stores the user's input expression
    private List<string> expressionElements = new List<string>(); // List to hold numbers and operators
    public GameObject quizPanel;
    public GameObject passPanel;

        // This method is called when a number or operator is dropped onto the equation panel
    public void AddToEquation(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Attempted to add an empty or null value to equation!");
            return;
        }

        expressionElements.Add(value); // Add the dragged number or operator to the list
        currentInput += value; // Update the expression string
        userInputText.text = currentInput; // Update the displayed expression in the UI

        // Log the current state of expressionElements and currentInput
        Debug.Log("Added value: " + value);
        Debug.Log("Current expressionElements: " + string.Join(" ", expressionElements)); // Ensure this is not empty
        Debug.Log("Current input string: " + currentInput);
    }

        // This method is called when the user clicks the "Calculate" button
    public void CalculateResult()
    {
        // Before joining the elements into a single expression, print the list
        Debug.Log("expressionElements before joining: " + string.Join(" ", expressionElements));

        string expression = string.Join("", expressionElements); // Join all elements into a single expression

        // Debug to check the expression being evaluated
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
            expression = expression.Replace("×", "*");  // Just in case there's a symbol issue

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