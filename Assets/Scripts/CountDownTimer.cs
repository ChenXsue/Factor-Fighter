using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for displaying time
    public float defaultTime = 90f;
    public float timeRemaining; // Start with 90 seconds (1:30)
    private bool timerIsRunning = true; // Timer state
    private bool isPaused = false;

    public static CountDownTimer instance; // Singleton instance


    // Manage timer across multiple scenes
    void Awake()
    {
       // Singleton pattern: If an instance already exists, destroy the new one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign the singleton instance and prevent this object from being destroyed on scene load
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // void Start()
    // {
    //     // DontDestroyOnLoad(gameObject); // Keeps the Timer alive across scenes
    //     // UpdateTimerDisplay(timeRemaining);

    //     // if (!timerInitialized)
    //     // {
    //     //     timeRemaining = 90f; // Start with 90 seconds
    //     //     timerInitialized = true;
    //     // }

    //     UpdateTimerDisplay(timeRemaining);
    // }

    void Update()
    {
        if (timerIsRunning && !isPaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Reduce time
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay(timeRemaining);
                // Freeze the game
                Time.timeScale = 0;
                // Display a message on the screen
                timerText.text = "Time's up!";
                // Change the color of the timer text to red
                timerText.color = Color.red;

                

            }
        }
    }

    

    // This function updates the displayed timer
    void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay = Mathf.Clamp(timeToDisplay, 0, Mathf.Infinity);

        int minutes = Mathf.FloorToInt(timeToDisplay / 60); // Calculate minutes
        int seconds = Mathf.FloorToInt(timeToDisplay % 60); // Calculate seconds

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds); // Display as minutes:seconds
    }

    // Function to reassign the TimerText UI to a new Canvas in a different scene
    public void ReassignToNewCanvas()
    {
        Canvas newCanvas = FindObjectOfType<Canvas>(); // Find the Canvas in the new scene
        if (newCanvas != null)
        {
            // Set the TimerText under the new Canvas
            timerText.transform.SetParent(newCanvas.transform, false);
        }
    }


    // Call this function when the Main Menu is clicked
    public void PauseTimer()
    {
        isPaused = true;
    }

    // Call this function when the game is resumed
    public void ResumeTimer()
    {
        isPaused = false;
    }

    public void ResetTimer()
    {
        timeRemaining = defaultTime;
        timerIsRunning = true;
        isPaused = false;
        UpdateTimerDisplay(timeRemaining);
    }

    // Function to restart the timer in the current level (used for the Restart button)
    public void RestartTimer()
    {
        timeRemaining = defaultTime;
        timerIsRunning = true;
        isPaused = false;
        UpdateTimerDisplay(timeRemaining);
    }

}
