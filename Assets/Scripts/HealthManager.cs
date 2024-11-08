using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour 
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    public bool isGameOver = false;

    private HealthUIController healthUIController;

    void Awake()
    {
        InitializeHealth();
        
        // 查找UI控制器
        healthUIController = FindObjectOfType<HealthUIController>();
        if (healthUIController == null)
        {
            Debug.LogError("HealthUIController not found!");
        }
    }

    private void InitializeHealth()
    {
        currentHealth = maxHealth;
        isGameOver = false;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isGameOver = false;
        UpdateUI();
        Debug.Log("Health reset to " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();
        Debug.Log("Current health after taking damage: " + currentHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateUI()
    {
        if (healthUIController != null)
        {
            healthUIController.UpdateHealthUI();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        isGameOver = true;
        // 可以触发游戏结束事件，而不是直接加载场景
        SceneManager.LoadScene("GameOverScene");
    }
}

// public class HealthManager : MonoBehaviour
// {
//     public static HealthManager Instance;

//     public int maxHealth = 3;  // 最大生命值
//     public int currentHealth;  // 当前生命值
//     public bool isGameOver = false;

//     private HealthUIController healthUIController;
//     private bool isFirstLevelInitialized = false; // 标记是否在第一关初始化过生命值

//     void Awake()
//     {
//         // 检查单例实例
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject); // 保持在场景切换时不销毁
//             InitializeHealth();

//             // 查找场景中的HealthUIController
//             healthUIController = FindObjectOfType<HealthUIController>();
//             if (healthUIController != null)
//             {
//                 DontDestroyOnLoad(healthUIController.gameObject); // 保持HealthUI在场景切换时不销毁
//                 healthUIController.gameObject.SetActive(false); // 初始状态下隐藏UI
//             }
//             else
//             {
//                 Debug.LogError("HealthUIController not found in the initial scene!");
//             }

//             // 订阅场景加载事件
//             SceneManager.sceneLoaded += OnSceneLoaded;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void InitializeHealth()
//     {
//         currentHealth = maxHealth;
//         isGameOver = false;
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         // 控制 HealthUI 的显示状态
//         if (scene.name == "SampleScene" && !isFirstLevelInitialized) // 仅在第一关重置生命值
//         {
//             if (healthUIController != null)
//             {
//                 healthUIController.gameObject.SetActive(true); // 激活UI
//                 ResetHealth(); // 重置生命值
//                 isFirstLevelInitialized = true; // 标记已初始化第一关
//                 healthUIController.UpdateHealthUI();
//                 Debug.Log("HealthUIController activated in SampleScene with health reset");
//             }
//         }
//         else if (scene.name.Contains("Level")) // 第二关中的Level1-Level4，不重置生命值
//         {
//             if (healthUIController != null)
//             {
//                 healthUIController.gameObject.SetActive(true); // 激活UI
//                 healthUIController.UpdateHealthUI();
//                 Debug.Log("HealthUIController activated in " + scene.name);
//             }
//         }
//         else // 主菜单、游戏结束场景，隐藏UI
//         {
//             if (healthUIController != null)
//             {
//                 healthUIController.gameObject.SetActive(false);
//                 Debug.Log("HealthUIController deactivated in " + scene.name);
//             }
//         }
//     }

//     public void ResetHealth()
//     {
//         currentHealth = maxHealth;
//         isGameOver = false;
//         if (healthUIController != null)
//         {
//             healthUIController.UpdateHealthUI();
//         }
//         Debug.Log("Health reset to " + currentHealth);
//     }

//     public void TakeDamage(int damage)
//     {
//         currentHealth -= damage;
//         if (healthUIController != null)
//         {
//             healthUIController.UpdateHealthUI();
//         }
//         Debug.Log("Current health after taking damage: " + currentHealth);

//         if (currentHealth <= 0)
//         {
//             GameOver();
//         }
//     }

//     private void GameOver()
//     {
//         Debug.Log("Game Over");
//         isGameOver = true;
//         SceneManager.LoadScene("GameOverScene");
//     }

//     private void OnDestroy()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded; // 取消事件订阅
//     }
// }
