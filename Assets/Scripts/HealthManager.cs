/*
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthManager : MonoBehaviour 
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    public bool isGameOver = false;

    private HealthUIController healthUIController;
    [SerializeField] public GameObject gameoverCanvas;

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
        Time.timeScale = 0f;
        gameoverCanvas.SetActive(true);
        gameoverCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "You have no health left!";
    }
}
*/
using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    public bool isGameOver = false;

    private HealthUIController healthUIController;
    [SerializeField] public GameObject gameoverCanvas;

    void Awake()
    {
        InitializeHealth();

        // 查找UI控制器
        healthUIController = FindObjectOfType<HealthUIController>();
        if (healthUIController == null)
        {
            Debug.LogError("HealthUIController not found!");
        }
        else
        {
            healthUIController.InitializeHealthUI(maxHealth); // 初始化爱心 UI
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
            healthUIController.UpdateHealthUI(currentHealth);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        isGameOver = true;
        Time.timeScale = 0f;
        gameoverCanvas.SetActive(true);
        gameoverCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "You have no health left!";
    }
}

