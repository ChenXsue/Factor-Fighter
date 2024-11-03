using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuFunctionManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image menuPanel; // 新添加的面板引用

    [Header("Inventory References")]
    [SerializeField] private NumberInventorySO numberInventory;
    [SerializeField] private OperatorInventorySO operatorInventory;

    private void Start()
    {
        SetupButtons();
        ValidateReferences();
        InitializeUI();
    }

    private void InitializeUI()
    {
        // 设置面板样式
        if (menuPanel != null)
        {
            menuPanel.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        }
    }

    private void SetupButtons()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void ValidateReferences()
    {
        if (numberInventory == null || operatorInventory == null)
        {
            Debug.LogError("Inventory references not set in MenuFunctionManager!");
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        GameTimer.Instance.ResetTimer();

        // 重置所有游戏状态
        ResetAllGameState();
        
        // 恢复时间缩放
        Time.timeScale = 1f;
        
        // 获取当前场景名称
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // 根据当前场景决定要加载的场景
        string sceneToLoad;
        
        if (currentSceneName.StartsWith("Level"))
        {
            // 如果当前在Level1-4中的任意一关，都重新加载Level1
            sceneToLoad = "Level1";
        }
        else if (currentSceneName == "SampleScene")
        {
            // 如果在第一关，重新加载SampleScene
            sceneToLoad = "SampleScene";
        }
        else
        {
            // 如果在其他场景（比如主菜单），默认加载第一关
            sceneToLoad = "SampleScene";
            Debug.LogWarning($"Unexpected scene name: {currentSceneName}, loading SampleScene as default.");
        }
        
        // 加载选定的场景
        SceneManager.LoadScene(sceneToLoad);
        StartCoroutine(StartTimerAfterLoad(sceneToLoad));

        
        Debug.Log($"Restarting game: Loading scene {sceneToLoad} from {currentSceneName}");
    }

    public void GoToMainMenu()
    {
        GameTimer.Instance.ResetTimer();
        ResetAllGameState();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");  // 确保有这个场景名称
    }
    // This coroutine will start the timer after the new scene has loaded
    private IEnumerator StartTimerAfterLoad(string levelName)
    {
        yield return new WaitForSeconds(0.1f); // Delay to ensure the scene is loaded
        GameTimer.Instance.StartTimer(levelName);
    }
    private void ResetAllGameState()
    {
        // 重置房间状态
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.ResetGame();
            Debug.Log("Reset room state: doors and collected numbers cleared");
        }

        // 重置敌人状态
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.ResetEnemies();
            Debug.Log("Reset enemy state: all enemies restored");
        }

        // 重置方块状态
        if (CubeManager.Instance != null)
        {
            CubeManager.Instance.ResetCubes();
            Debug.Log("Reset cube state: all cubes restored");
        }

        // 清空背包
        ClearInventories();

        // 刷新UI
        RefreshInventoryUI();
    }

    private void ClearInventories()
    {
        if (numberInventory != null)
        {
            numberInventory.Clear();
            Debug.Log("Number inventory cleared");
        }

        if (operatorInventory != null)
        {
            operatorInventory.Clear();
            Debug.Log("Operator inventory cleared");
        }
    }

    private void RefreshInventoryUI()
    {
        NumberInventoryManager numberManager = FindObjectOfType<NumberInventoryManager>();
        if (numberManager != null)
        {
            numberManager.RefreshNumberInventory();
        }

        OperatorInventoryManager operatorManager = FindObjectOfType<OperatorInventoryManager>();
        if (operatorManager != null)
        {
            operatorManager.RefreshOperatorInventory();
        }
    }

    private void OnDestroy()
    {
        if (resumeButton != null)
            resumeButton.onClick.RemoveListener(ResumeGame);
        
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(GoToMainMenu);
    }
}