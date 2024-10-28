using UnityEngine;
using System.Collections.Generic;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance; // Singleton instance
    private float startTime;
    private bool isTiming = false; // Check if timer is currently running
    private bool levelStarted = false; // Check if the level timer has started
    private string currentLevel; // Store the current level name

    // List to store any custom time points for logging purposes
    private List<float> timePoints = new List<float>();

    private WebGLDataLogger dataLogger; // Reference to WebGLDataLogger

    void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Preserve GameTimer across all scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        // 在 Awake 中查找 WebGLDataLogger 实例，避免重复查找
        dataLogger = FindObjectOfType<WebGLDataLogger>();
        
        // Debug output to verify the logger is found
        if (dataLogger == null)
        {
            Debug.LogWarning("WebGLDataLogger not found in the scene!");
        }
        else
        {
            Debug.Log("WebGLDataLogger found and ready to log data.");
        }
    }

    // Method to start the timer, accepting a level name
    public void StartTimer(string levelName)
    {
        // Only start if not already timing and level hasn't started
        if (!isTiming && !levelStarted)
        {
            startTime = Time.time; // Capture the start time
            isTiming = true;
            levelStarted = true; // Mark the level as started
            currentLevel = levelName; // Set the current level name
            timePoints.Clear(); // Clear any previous time points

            Debug.Log($"Timer started for {currentLevel} at {startTime} seconds");
        }
    }

    // Method to stop the timer and log the time spent
    public void StopTimer()
    {
        if (isTiming)
        {
            float timeSpent = Time.time - startTime; // Calculate total time spent
            Debug.Log($"Timer stopped for {currentLevel}. Level completed in {timeSpent} seconds");

            isTiming = false;
            levelStarted = false; // Reset level started for the next level
            
            // Log to Google Sheets if dataLogger is available
            if (dataLogger != null)
            {
                StartCoroutine(dataLogger.LogDataToGoogleSheet(currentLevel, timeSpent));
            }
            else
            {
                Debug.LogWarning("WebGLDataLogger not found! Unable to log data to Google Sheets.");
            }
        }
    }

    // Method to reset the timer (usually called when returning to the main menu)
    public void ResetTimer()
    {
        isTiming = false;
        levelStarted = false;
        startTime = 0;
        currentLevel = ""; // Clear the current level name
        timePoints.Clear(); // Clear all recorded time points

        Debug.Log("Timer has been reset.");
    }

    // Method to add a custom time point (for specific in-game events)
    public void AddTimePoint()
    {
        if (isTiming)
        {
            float currentTime = Time.time - startTime; // Calculate time since start
            timePoints.Add(currentTime); // Record the time point

            Debug.Log($"Time point added at {currentTime} seconds in {currentLevel}");
        }
    }

    // Method to check if the timer is currently running
    public bool IsTiming()
    {
        return isTiming;
    }
}