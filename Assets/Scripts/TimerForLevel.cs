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
    private bool isStarted = false;  // 新添加的变量来追踪计时器是否已经开始
    
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public GameObject gameoverCanvas;     
    [SerializeField] public GameObject menuCanvas;
    [SerializeField] private float startDelay = 5.3f;  // 延迟开始的时间

    void Start()
    {
        remainingTime = time;
        // 不在这里设置 startTime，而是使用协程延迟设置
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(startDelay);
        
        // 设置开始时间并启动计时器
        startTime = DateTime.Now;
        isStarted = true;
    }

    void Update()
    {
        // 只有在计时器已启动且未暂停时才更新显示
        if (isStarted && !isPaused)
        {
            UpdateTimerDisplay();
        }
        else if (!isStarted)
        {
            // 在开始前显示初始时间
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

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