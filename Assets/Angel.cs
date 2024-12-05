using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Angel : MonoBehaviour
{
    public GameObject angelPanel;
    public  TMP_InputField inputField;
    public string Operation1;
    public string Operation2;
    public int AngelID;
    private RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the angel!");

            if (roomManager != null)
            {
                roomManager.PauseGame();

                angelPanel.GetComponent<AngelPanel>().SetCurrentAngelID(AngelID, Operation1, Operation2);
                angelPanel.SetActive(true);
                angelPanel.GetComponent<AngelPanel>().AngelGameObject = gameObject;
            }
        }
    }

    public void ResumeGame()
    {
        roomManager.ResumeGame();
    }
}
