/*
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private HealthManager healthManager;  // 改为SerializeField，可以在Inspector中拖拽

    private void Start()
    {
        // 如果没有在Inspector中指定，则尝试查找
        if (healthManager == null)
        {
            healthManager = FindObjectOfType<HealthManager>();
            Debug.LogWarning("HealthManager not assigned, attempting to find in scene");
        }
        
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found!");
            return;
        }
        
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthManager != null)
        {
            healthText.text = $"Health: {healthManager.currentHealth}";
        }
    }
}
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab; // 爱心预制体
    [SerializeField] private Transform healthPanel;  // 放置爱心的 Panel
    private List<GameObject> hearts = new List<GameObject>(); // 存储生成的爱心实例

    public void InitializeHealthUI(int maxHealth)
    {
        // 清理已有的爱心
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // 根据最大生命值生成爱心
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, healthPanel);
            hearts.Add(newHeart);
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        // 根据当前生命值显示或隐藏爱心
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }
}

