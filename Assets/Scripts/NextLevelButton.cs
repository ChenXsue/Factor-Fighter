using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevelButton : MonoBehaviour 
{
    [Header("Inventories")]
    [SerializeField] private NumberInventorySO numberInventory;
    [SerializeField] private OperatorInventorySO operatorInventory;
    
    [Header("References")]
    [SerializeField] private HealthManager healthManager;  // 添加HealthManager引用
    
    [Header("Level Settings")]
    [SerializeField] private string nextLevelName = "Level1";

    private void Awake()
    {
        // 如果没有在Inspector中指定，尝试查找
        if (healthManager == null)
        {
            healthManager = FindObjectOfType<HealthManager>();
            if (healthManager == null)
            {
                Debug.LogError("HealthManager not found! Please assign in inspector.");
            }
        }
    }

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

        // 重置生命值
        if (healthManager != null)
        {
            healthManager.ResetHealth();
            Debug.Log("Health reset to maximum.");
        }
        else
        {
            Debug.LogWarning("HealthManager is not assigned!");
        }

        // 加载下一关
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // 根据当前场景名确定下一个场景
        nextLevelName = currentSceneName switch
        {
            "Tutorial" => "Level1",
            "Level1" => "Level2",
            "Level2" => "Level3",
            "Level3" => "Level4",
            _ => currentSceneName // 其他未知场景保持不变
        };
        
        Debug.Log($"Loading next level: {nextLevelName}");
        SceneManager.LoadScene(nextLevelName);
    }
}