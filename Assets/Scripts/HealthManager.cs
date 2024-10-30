using UnityEngine;
using UnityEngine.SceneManagement;


public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public int maxHealth = 3;  // 最大生命值
    public int currentHealth;  // 当前生命值
    public bool isGameOver = false;

    void Awake()
    {
        // 检查单例实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持在场景切换时不销毁
            InitializeHealth();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeHealth()
    {
        currentHealth = maxHealth; // 设置每一关开始时的初始生命值
        isGameOver = false; // 重置游戏结束状态
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; // 重新开始时重置生命值
        isGameOver = false; // 重置游戏结束状态
        Debug.Log("Health reset to " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        isGameOver = true;
        SceneManager.LoadScene("GameOverScene"); // 切换到游戏结束场景
    }

}
