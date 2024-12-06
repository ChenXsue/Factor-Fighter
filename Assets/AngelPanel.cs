using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AngelPanel : MonoBehaviour
{
    public GameObject AngelGameObject;
    public int currAngelID;
    public TMP_InputField inputNumber;
    public Button Operation1;
    public Button Operation2;
    // Start is called before the first frame update
    void Start()
    {
        currAngelID = 0;
        // Operation1.GetComponentInChildren<TextMeshProUGUI>().text = "";
        // Operation2.GetComponentInChildren<TextMeshProUGUI>().text = "";
        if (inputNumber != null)
        {
            inputNumber.interactable = false;
        }
    }

    public void SetCurrentAngelID(int angelID, string operation1, string operation2)
    {
        currAngelID = angelID;
        Debug.Log($"Current angelInput reference status: {(inputNumber != null ? "Valid" : "Null")}");
        Debug.Log("Current Angel ID: " + currAngelID);
        Operation1.GetComponentInChildren<TextMeshProUGUI>().text = operation1;
        Debug.Log("Operation1: " + operation1);
        Operation2.GetComponentInChildren<TextMeshProUGUI>().text = operation2;
        Debug.Log("Operation2: " + operation2);
    }

    public int CalculateOperation(string operationNumber)
    {
        if (inputNumber.text == "")
        {
            Debug.LogError("Invalid input number: " + inputNumber.text);
            return 0;
        }

        int numberToAdd = 0;
        int baseNum = int.Parse(inputNumber.text);
        string text = "";

        if (operationNumber == "Operation1")
        {
            text = Operation1.GetComponentInChildren<TextMeshProUGUI>().text.Trim();
        }
        else if (operationNumber == "Operation2")
        {
            text = Operation2.GetComponentInChildren<TextMeshProUGUI>().text.Trim();
        }
        else
        {
            Debug.LogError("Invalid operation number: " + operationNumber);
        }

        if (text.Length < 2)
        {
            Debug.LogError("Invalid input: " + text);
            return baseNum;
        }

        char operation = text[0];
        int number = int.Parse(text.Substring(1));

        switch(operation)
        {
            case '+':
                numberToAdd = baseNum + number;
                break;
            case '-':
                numberToAdd = baseNum - number;
                break;
            case '*':
                numberToAdd = baseNum * number;
                break;
            case '/':
                numberToAdd = baseNum / number;
                break;
            default:
                Debug.LogError("Invalid operation: " + operation);
                break;
        }

        Debug.Log("Angel Number to add: " + numberToAdd);

        if (numberToAdd > 0)
        {
            return numberToAdd;
        }
        else
        {
            return baseNum;
        }
    }

    public void updateAngel()
    {
        AngelGameObject.SetActive(false);
        AngelGameObject.GetComponent<Angel>().ResumeGame();
    }
}
