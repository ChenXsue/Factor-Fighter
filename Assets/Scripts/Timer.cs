using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    void Update()
    {
        // Convert the remaining time to minutes and seconds
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;
            // Display a message on the screen
            timerText.text = "Time's up!";
            // Change the color of the timer text to red
            timerText.color = Color.red;
            // Freeze the game
            Time.timeScale = 0;   
        }
        
    }

}
