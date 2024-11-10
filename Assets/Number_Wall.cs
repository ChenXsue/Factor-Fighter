using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Number_Wall : MonoBehaviour 
{
    public static bool isNumberWallActive = false;
    public int areaSize;
    public int givenSide;
    public string answer;
    public GameObject mathProblemPanel;
    public GameObject operatorsPanel;
    public GameObject input;
    public TMP_InputField inputField;
    public static bool isGamePaused = false;
    //private string mathProblem;

    void Start()
    {
        //mathProblem = "What is ?";
        mathProblemPanel.SetActive(false);
        int answer_int = areaSize / givenSide;
        answer = answer_int.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNumberWallActive = true;
            PauseGame();
            Debug.Log("Player has reached the number wall!");

            mathProblemPanel.SetActive(true);
            operatorsPanel.SetActive(true);
            input.SetActive(true);
        }
    }

    public void CheckAnswer()
    {
        if (inputField.text == answer)
        {
            Debug.Log("Correct answer!");
            ResumeGame();
            mathProblemPanel.SetActive(false);
            operatorsPanel.SetActive(false);
            input.SetActive(false);
            isNumberWallActive = false;
        }
        else
        {
            Debug.Log("Incorrect answer!");
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