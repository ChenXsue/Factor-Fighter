using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Number_Wall : MonoBehaviour 
{
    public static Number_Wall instance;
    public static bool isNumberWallActive = false;
    public int areaSize;
    public int givenSide;
    public int answer;
    public GameObject mathProblemPanel;
    public GameObject operatorsPanel;
    public GameObject input;
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
        mathProblemPanel.SetActive(false);
        answer = areaSize / givenSide;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNumberWallActive = true;
            PauseGame();
            Debug.Log("Player has reached the number wall!");

            inputField.text = "";
            isColliding = true;
            mathProblemPanel.SetActive(true);
            operatorsPanel.SetActive(true);
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
            
            ResumeGame();
            return true;
        }
        else
        {
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