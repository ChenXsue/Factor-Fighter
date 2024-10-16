using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private NumberInventorySO numberInventory;
    [SerializeField] private OperatorInventorySO operatorInventory;
    [SerializeField] private string nextLevelName = "Level1";

    public void OnNextLevelButtonClick()
    {
        // 清空数字背包
        if (numberInventory != null)
        {
            numberInventory.Clear();
            Debug.Log("Number inventory cleared.");
        }
        else
        {
            Debug.LogWarning("Number inventory is not assigned!");
        }

        // 清空运算符背包
        if (operatorInventory != null)
        {
            operatorInventory.Clear();
            Debug.Log("Operator inventory cleared.");
        }
        else
        {
            Debug.LogWarning("Operator inventory is not assigned!");
        }

        // 加载下一关
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        Debug.Log($"Loading next level: {nextLevelName}");
        SceneManager.LoadScene(nextLevelName);
    }
}