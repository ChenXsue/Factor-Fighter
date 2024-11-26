using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class TimerForLevel : MonoBehaviour
{
    public float time = 90f;
    
    public float remainingTime;
    
    private DateTime startTime;
    private bool isPaused = false;
    
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public GameObject gameoverCanvas;
    
    [SerializeField] public GameObject menuCanvas;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = time;
        startTime = DateTime.Now;
    }
    // Update is called once per frame
    
    void Update()
    {
        if (!isPaused)
        {
            UpdateTimerDisplay();
        }
    }    
    // This function updates the displayed timer
    void UpdateTimerDisplay()
    {
        TimeSpan elapsed = DateTime.Now - startTime;
        remainingTime = time - (float)elapsed.TotalSeconds;
        remainingTime = Mathf.Clamp(remainingTime, 0, Mathf.Infinity);
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        if (remainingTime <= 0)
        {
            Debug.Log("Time's up!");
            timerOut();
        }
    }
    
    
    public void PauseTimer()
    {
        isPaused = true;
    }
    public void ResumeTimer()
    {
        isPaused = false;
        startTime = DateTime.Now - TimeSpan.FromSeconds(time - remainingTime);
    }
    void timerOut()
    {
        Time.timeScale = 0f;
        if(menuCanvas.activeSelf)
        {
            menuCanvas.SetActive(false);
        }
        gameoverCanvas.SetActive(true);
        gameoverCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Time's up!";
        GameTimer.Instance.StopTimer("Time's up");
    }
}