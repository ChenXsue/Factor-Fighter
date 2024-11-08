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
// public class HealthUIController : MonoBehaviour
// {
//     public TextMeshProUGUI healthText;  // UI中的文本组件


//     private void Start()
//     {
//         Debug.Log("HealthUIController Start called"); // 检查 Start 是否被调用
//         UpdateHealthUI(); // 初始化时更新UI显示
//     }


//     public void UpdateHealthUI()
//     {
//         if (HealthManager.Instance != null)
//         {
//             healthText.text = "Health: " + HealthManager.Instance.currentHealth;
//             Debug.Log("Updated UI Health Text to: Health: " + HealthManager.Instance.currentHealth); // 确认UI内容已更新
//         }
//         else
//         {
//             Debug.LogError("HealthManager instance is null.");
//         }
//     }


// }