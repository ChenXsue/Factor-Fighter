using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    private MathProblem currentProblem;
    public TMPro.TextMeshProUGUI operators;
    public GameObject buttonPrefab;  // Prefab for the operator button to be instantiated
    public GameObject operatorsPanel; // Panel where operator buttons will be displayed


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
        UpdatePanelAText();
        // Optionally, destroy the defeated enemy object
        Destroy(gameObject);
    }
    private void UpdatePanelAText()
    {
        string operatorsText = "";  // String to store all operators
        foreach (char op in operator1.OperatorInstance.Operators_Bag)
        {
            operatorsText += op + " ";  // Append each operator to the string
        }

        // Update the TextMeshProUGUI component with the concatenated operators string
        if (operators != null)
        {
            operators.text = operatorsText;  // Update the text field in panelA
        }
        else
        {
            Debug.LogError("Operators is not assigned!");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
        mathUI.ShowMathProblem(this);
    }

}