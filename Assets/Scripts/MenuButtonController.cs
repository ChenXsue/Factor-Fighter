using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private Image panelBackground; // 新添加的灰色面板引用
    private Button menuButton;
    private bool isMenuVisible = false;

    private void Start()
    {
        menuButton = GetComponent<Button>();
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ToggleMenu);
        }

        // 确保菜单在开始时是隐藏的
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }

        // 设置面板的初始状态
        if (panelBackground != null)
        {
            Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f); // RGBA值，半透明灰色
            panelBackground.color = backgroundColor;
            panelBackground.gameObject.SetActive(false);
        }
    }

    private void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        menuCanvas.SetActive(isMenuVisible);
        
        if (panelBackground != null)
        {
            panelBackground.gameObject.SetActive(isMenuVisible);
        }

        // 暂停/恢复游戏
        Time.timeScale = isMenuVisible ? 0f : 1f;
    }

    private void OnDestroy()
    {
        if (menuButton != null)
        {
            menuButton.onClick.RemoveListener(ToggleMenu);
        }
        Time.timeScale = 1f;
    }
}