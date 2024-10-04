using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // Reference to the "Number" panel under "DoorCanvas"
    public GameObject quizPanel;     // Quiz panel to show when the player reaches the door
    public GameObject equation;      // The equation panel to display the equation
    public GameObject operatorsShown;
    public GameObject operatorsPanel; // Panel where operator buttons will be displayed
    public GameObject buttonPrefab;  // Prefab for the operator button to be instantiated

    private void Start()
    {
        // Initially hide the quiz and equation panels
        quizPanel.SetActive(false);
        equation.SetActive(false);
        operatorsPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding with the door is the player
        if (other.gameObject.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            // Show the quiz panel and equation panel when the player touches the door
            quizPanel.SetActive(true);
            equation.SetActive(true);
            operatorsShown.SetActive(false);
            operatorsPanel.SetActive(true);

            // Call the method to create buttons for each operator in Operators_Bag
            // CreateOperatorButtons();
        }
    }

    // Method to dynamically create buttons for each operator in Operators_Bag
    private void CreateOperatorButtons()
    {

        // Define a starting position and spacing for the buttons
        Vector2 startingPosition = new Vector2(0, 0); // Start from the top left corner or any other position
        float spacing = 50f; // Set the spacing between buttons

        // Loop through all stored operators in the Operators_Bag and create a button for each one
        for (int i = 0; i < operator1.OperatorInstance.Operators_Bag.Count; i++)
        {
            char op = operator1.OperatorInstance.Operators_Bag[i];
            Debug.Log("i:" + i);
            // Instantiate a new button using the button prefab
            GameObject newButton = Instantiate(buttonPrefab);

            // Set the button as a child of the operatorsPanel
            newButton.transform.SetParent(operatorsPanel.transform, false); // SetParent, and 'false' to keep the local scale and position

            // Set the button's text to the operator value
            newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = op.ToString();

            // Add the DraggableItem script dynamically so the button can be dragged
            newButton.AddComponent<DraggableItem>();

            // Ensure the button's scale and size are correct within the panel
            newButton.transform.localScale = Vector3.one;  // Make sure the scale is 1,1,1
            newButton.GetComponent<RectTransform>().localPosition = Vector3.zero; // Reset position inside the panel
            newButton.name = op.ToString();
            newButton.SetActive(true);
            // Set the button's anchoredPosition based on its index
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(startingPosition.x + (i * spacing), startingPosition.y); // Horizontal layout with spacing
        }
    }
}