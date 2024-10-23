using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // 如果您想在Inspector中设置这些初始值，可以将它们设为public
    [SerializeField] private int[] initialNumbers = { 3, 2, 4 };

    private void Start()
    {
        InitializeNumberInventory();
    }

    private void InitializeNumberInventory()
    {
        // 确保NumberManager和NumberInventoryManager实例存在
        if (NumberManager.instance == null || NumberInventoryManager.instance == null)
        {
            Debug.LogError("NumberManager or NumberInventoryManager instance is missing!");
            return;
        }

        
        foreach (int number in initialNumbers)
        {
            NumberSO numberSO = NumberManager.instance.GetNumber(number);
            if (numberSO != null)
            {
                NumberInventoryManager.instance.AddNumber(numberSO);
                Debug.Log($"Added initial number to inventory: {number}");
            }
            else
            {
                Debug.LogWarning($"Failed to get NumberSO for value: {number}");
            }
        }
    }
}