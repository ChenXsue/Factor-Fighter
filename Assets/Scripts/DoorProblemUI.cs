using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorProblemUI : MonoBehaviour
{
    public TextMeshProUGUI problemText;

    public GameObject doorMathProblemPanel;

    public TMP_InputField answerInputField;

    public GameObject responsePanel;

    private RoomDoor currentRoomDoor;

    private RoomManager roomManager;

    private GameObject player;



    // Start is called before the first frame update
    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (answerInputField != null)
        {
            //answerInputField.onSubmit.AddListener(OnInputFieldSubmit);
        }
    }

    private void OnInputFieldSubmit(string text)
    {
        if (doorMathProblemPanel.activeSelf)
        {
            //CheckAnswer();
        }
    }

    private void OnDestroy()
    {
        
        if (answerInputField != null)
        {
            answerInputField.onSubmit.RemoveListener(OnInputFieldSubmit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowMathProblem(RoomDoor roomDoor)
    {
        currentRoomDoor = roomDoor;
        SetupProblem(roomDoor.GetMathProblem());
    }

    private void SetupProblem(string problem)
    {
        problemText.text = problem;
        doorMathProblemPanel.SetActive(true);
    }

    public void CheckAnswer()
    {
        if (currentRoomDoor != null)
        {
            int playerNumber = int.Parse(answerInputField.text);
            if (currentRoomDoor.CheckAnswer(playerNumber))
            {
                Debug.Log("Correct answer!");
                currentRoomDoor.Defeat();
                if(player != null)
                {
                    roomManager.TeleportPlayer(player, currentRoomDoor.doorId);
                }
                doorMathProblemPanel.SetActive(false);
                ResumeGame();

            }
            else
            {
                Debug.Log("Wrong answer!");
                //responsePanel.SetActive(true);
            }


        }
    }


    private void ResumeGame()
    {
        if (roomManager != null)
        {
            roomManager.ResumeGame();
            Debug.Log("Game resumed through RoomManager");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Game resumed (fallback)");
        }
        answerInputField.text = "";
        currentRoomDoor = null;
    }


}
