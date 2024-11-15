using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class RoomDoor : MonoBehaviour 
{
    [Header("Door Settings")]
    public int doorId;
    public int pairedDoorId;
    public int targetRoomNumber;
    public int associatedNumber;
    public GameObject thisDoorText;
    public GameObject pairedDoorText;
    
    private RoomManager roomManager;
    private MathProblem currentProblem;

    [Header("Door Condition")]
    public string requiredCondition; // "even", "prime", "greaterThan10"


    
    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        //Ene_MathProblemManager problemManager = FindObjectOfType<Ene_MathProblemManager>();
        // if (problemManager != null)
        // {
        //     currentProblem = problemManager.GetRandomMathProblem();
        //     Debug.Log($"Math problem for door {doorId}: {currentProblem.question}");
        // }

    }

    public string GetMathProblem()
    {
        switch (requiredCondition)
        {
            case "even":
                return "Give me an even number";
            case "prime":
                return "Give me a prime number";
            case "greaterThan10":
                return "Give me a number greater than 10";
            default:
                return "Invalid condition";
        }
    }
    
    public bool CheckAnswer(int playerNumber)
    {
        switch (requiredCondition)
        {
            case "even":
                return playerNumber % 2 == 0;
            case "prime":
                return IsPrime(playerNumber);
            case "greaterThan10":
                return playerNumber > 10;
            default:
                return false;
        }
    }

    private bool IsPrime(int number)
    {
        if (number <= 1) return false;
        for (int i = 2; i <= Mathf.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && roomManager != null)
        {
            // 
            if (roomManager.IsDoorUnlocked(doorId) || roomManager.IsDoorUnlocked(pairedDoorId))
            {
                roomManager.TeleportPlayer(other.gameObject, doorId);
            }
            else
            {
                // MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
                // if (mathUI != null)
                // {
                //     mathUI.ShowMathProblem(this);
                //     roomManager.PauseGame();
                // }
                DoorProblemUI doorProblemUI = FindObjectOfType<DoorProblemUI>();
                if(doorProblemUI != null)
                {
                    doorProblemUI.ShowMathProblem(this);
                    roomManager.PauseGame();
                }
            }
        }
    }
    
    public void Defeat()
    {
        WebGLDataLogger.numberUsed ++;
        WebGLDataLogger.numberSum ++;
        Debug.Log($"Door {doorId} defeated");
        thisDoorText.SetActive(false);
        pairedDoorText.SetActive(false);
        Unlock();
    }
    
    private void Unlock()
    {
        Debug.Log($"Door {doorId} unlocked");
        if (roomManager != null)
        {
            roomManager.UnlockDoor(doorId);
            if (pairedDoorId != 0)
            {
                roomManager.UnlockDoor(pairedDoorId);
            }
            
            if (!roomManager.HasCollectedReward(doorId) && !roomManager.HasCollectedReward(pairedDoorId))
            {
                NumberSO numberSO = NumberManager.instance.GetNumber(associatedNumber);
                if (numberSO != null)
                {
                    NumberInventoryManager.instance.AddNumber(numberSO);
                    Debug.Log($"Obtained number: {associatedNumber} from door {doorId}");
                    roomManager.MarkRewardCollected(doorId);
                    if (pairedDoorId != 0)
                    {
                        roomManager.MarkRewardCollected(pairedDoorId);
                    }
                }
                else
                {
                    Debug.LogWarning($"Failed to get number SO for value: {associatedNumber}");
                }
            }
        }
    }
}