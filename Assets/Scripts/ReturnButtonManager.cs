using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

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
        Enemy.ResumeGame();
    }

    private void ReturnAllItemsToBags()
    {
        string currentInput = inputField.text;
        Debug.Log($"Processing return of items from input: {currentInput}");
        
        StringBuilder currentNumber = new StringBuilder();
        
        for (int i = 0; i < currentInput.Length; i++)
        {
            char c = currentInput[i];
            
            if (char.IsDigit(c))
            {
                // 累积数字字符
                currentNumber.Append(c);
                
                // 如果是最后一个字符或者下一个字符不是数字，则处理当前数字
                if (i == currentInput.Length - 1 || !char.IsDigit(currentInput[i + 1]))
                {
                    if (currentNumber.Length > 0)
                    {
                        int number = int.Parse(currentNumber.ToString());
                        ReturnNumberToBag(number);
                        currentNumber.Clear();
                    }
                }
            }
            else if (IsOperator(c))
            {
                // 确保之前的数字已经处理完
                if (currentNumber.Length > 0)
                {
                    int number = int.Parse(currentNumber.ToString());
                    ReturnNumberToBag(number);
                    currentNumber.Clear();
                }
                
                ReturnOperatorToBag(c);
            }
        }

        // 刷新两个背包的显示
        NumberInventoryManager.instance.RefreshNumberInventory();
        OperatorInventoryManager.instance.RefreshOperatorInventory();
    }

    private void ReturnNumberToBag(int number)
    {
        Debug.Log($"Returning number {number} to bag");

        // 使用 NumberManager 获取正确的 NumberSO 实例
        NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
        if (numberToAdd != null)
        {
            numberToAdd.count = 1;
            NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
            Debug.Log($"Successfully added number {number} back to inventory");
        }
        else
        {
            Debug.LogError($"Failed to get NumberSO instance for number {number}");
        }
    }

    private void ReturnOperatorToBag(char operatorChar)
    {
        Debug.Log($"Returning operator {operatorChar} to bag");

        OperatorSO operatorToAdd = ScriptableObject.CreateInstance<OperatorSO>();
        operatorToAdd.operatorChar = operatorChar;
        operatorToAdd.itemName = operatorChar.ToString();
        operatorToAdd.count = 1;

        OperatorInventoryManager.instance.myOperatorBag.AddItem(operatorToAdd);
        Debug.Log($"Successfully added operator {operatorChar} back to inventory");
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