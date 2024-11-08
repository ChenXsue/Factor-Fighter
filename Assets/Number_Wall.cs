using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number_Wall : MonoBehaviour
{
    public int areaSize;
    public int givenSide;
    public GameObject mathProblemPanel;
    public GameObject operatorsPanel;
    public GameObject input;
    public static bool isGamePaused = false;
    private string mathProblem;

    // Start is called before the first frame update
    void Start()
    {
        mathProblem = "What is ?";
        mathProblemPanel.SetActive(false);
        // mathProblem = givenSide.ToString() + " * ? = " + areaSize.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PauseGame();
            Debug.Log("Player has reached the number wall!");

            mathProblemPanel.SetActive(true);
            operatorsPanel.SetActive(true);
            input.SetActive(true);
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
        Debug.Log("Game resumed");
    }
}
