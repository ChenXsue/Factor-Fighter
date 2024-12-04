using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Number_Wall : MonoBehaviour 
{
    public static Number_Wall instance;
    public static bool isNumberWallActive = false;
    public int areaSize;
    public int givenSide;
    public int answer;
    public GameObject mathProblemPanel;
    public GameObject numberGridPlane;
    public GameObject operatorsPanel;
    public GameObject input;
    public GameObject bluebox;
    public TMP_InputField inputField;
    public GameObject areaText;
    public GameObject sideObject;
    public GameObject areaObject;
    public static bool isGamePaused = false;
    public bool isColliding = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //mathProblem = "What is ?";
        mathProblemPanel.SetActive(true);
        answer = areaSize / givenSide;
        bluebox.transform.SetParent(mathProblemPanel.transform, false); // 将 bluebox 添加为 mathProblemPanel 的子对象
        bluebox.GetComponent<Canvas>().sortingOrder = 1; // 设置 bluebox 的渲染顺序高于 panel
        if (inputField.placeholder != null)
            {
                TMP_Text placeholderText = inputField.placeholder.GetComponent<TMP_Text>();
                placeholderText.text = "W"; // 设置占位符内容
                placeholderText.color = Color.gray; // 设置占位符颜色
            }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNumberWallActive = true;
            PauseGame();
            Debug.Log("Player has reached the number wall!");

            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                numberGridPlane.GetComponent<TutorialGlow>().StartFlashing();
            }

            inputField.text = "";
            isColliding = true;
            mathProblemPanel.SetActive(true);
            // operatorsPanel.SetActive(true);
        }
        
    }

    public bool CheckAnswer()
    {
        inputField.text = inputField.text.Trim();
        if (inputField.text == answer.ToString())
        {
            Debug.Log("Number Wall Correct answer!");
            isColliding = false;
            mathProblemPanel.SetActive(false);
            areaText.SetActive(false);
            sideObject.SetActive(false);
            SpriteRenderer areaSprite = areaObject.GetComponent<SpriteRenderer>();
            areaSprite.color = Color.white;
            WebGLDataLogger.numberUsed ++;
            WebGLDataLogger.numberSum ++;
            WebGLDataLogger.answerSum++;

            ResumeGame();
            return true;
        }
        else
        {
            WebGLDataLogger.wrongNum++;
            Debug.Log("Number Wall Incorrect answer!");
            return false;
        }
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        Debug.Log("Game paused");
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        isNumberWallActive = false;
        Debug.Log("Game resumed");
    }
}