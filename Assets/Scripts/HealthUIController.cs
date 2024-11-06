using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public TextMeshProUGUI healthText;  // UI中的文本组件


    private void Start()
    {
        Debug.Log("HealthUIController Start called"); // 检查 Start 是否被调用
        UpdateHealthUI(); // 初始化时更新UI显示
    }


    public void UpdateHealthUI()
    {
        if (HealthManager.Instance != null)
        {
            healthText.text = "Health: " + HealthManager.Instance.currentHealth;
            Debug.Log("Updated UI Health Text to: Health: " + HealthManager.Instance.currentHealth); // 确认UI内容已更新
        }
        else
        {
            Debug.LogError("HealthManager instance is null.");
        }
    }


}