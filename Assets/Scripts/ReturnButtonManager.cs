using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // 用于 Button 类
using TMPro;                    // 用于 TMP_InputField 类
using System.Text;     
using System;  

public class ReturnButtonManager : MonoBehaviour
{
    [SerializeField] private Button returnButton;
    [SerializeField] private GameObject invisibleObject;
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(OnReturnButtonClicked);
            Debug.Log("Return button listener added");
        }
        else
        {
            Debug.LogError("Return button reference is missing!");
        }
    }

    private void OnReturnButtonClicked()
    {
        Debug.Log("Return button clicked");

        if (!string.IsNullOrEmpty(inputField.text))
        {
            ReturnAllItemsToBags();
        }

        if (invisibleObject != null)
        {
            invisibleObject.SetActive(false);
        }

        if (inputField != null)
        {
            inputField.text = "";
        }

        //Enemy.ResumeGame();
        Time.timeScale = 1f;

        // 刷新UI状态
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
        {
            inputManager.OnInvisibleObjectStateChanged();
        }
    }

    private void ReturnAllItemsToBags()
    {
        string currentInput = inputField.text;
        Debug.Log($"Processing return of items from input: {currentInput}");

        // 分割输入字符串，使用空格作为分隔符
        string[] tokens = currentInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string token in tokens)
        {
            if (int.TryParse(token, out int number))
            {
                ReturnNumberToBag(number);
            }
            else if (token.Length == 1 && IsOperator(token[0]))
            {
                ReturnOperatorToBag(token[0]);
            }
        }

        NumberInventoryManager.instance.RefreshNumberInventory();
        OperatorInventoryManager.instance.RefreshOperatorInventory();
    }

    private void ReturnNumberToBag(int number)
    {
        Debug.Log($"Returning number {number} to bag");
        NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
        if (numberToAdd != null)
        {
            NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
            Debug.Log($"Successfully added number {number} back to inventory");
        }
    }

    private void ReturnOperatorToBag(char operatorChar)
    {
        Debug.Log($"Returning operator {operatorChar} to bag");
        
        // 通过InputManager获取操作符预设
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
        {
            OperatorSO operatorToAdd = inputManager.GetOperatorPreset(operatorChar);
            if (operatorToAdd != null)
            {
                OperatorInventoryManager.instance.myOperatorBag.AddItem(operatorToAdd);
                Debug.Log($"Successfully added operator {operatorChar} back to inventory");
            }
        }
    }

    private bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/';
    }

    private void OnDestroy()
    {
        if (returnButton != null)
        {
            returnButton.onClick.RemoveListener(OnReturnButtonClicked);
        }
    }
}