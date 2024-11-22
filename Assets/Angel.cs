using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Angel : MonoBehaviour
{
    public GameObject angelPanel;
    public  TMP_InputField inputField;
    public int scalar;
    private RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        scalar = 3;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the angel!");

            if (roomManager != null)
            {
                roomManager.PauseGame();

                angelPanel.SetActive(true);
            }
        }
    }

    public int CalculateNumber()
    {
        int number = int.Parse(inputField.text);
        int result = number * scalar;
        Debug.Log($"The result is {result}");
        return result;
    }

    public void ResumeGame()
    {
        roomManager.ResumeGame();
    }
}
