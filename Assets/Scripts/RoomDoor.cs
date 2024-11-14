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

    [Header("Door Condition")]
    public string requiredCondition; // "even", "prime", "greaterThan10"
    
    private RoomManager roomManager;

    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
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
        // First check if the player has this number
        NumberSO numberToUse = NumberManager.instance.GetNumber(playerNumber);
        if (!NumberInventoryManager.instance.HasNumber(numberToUse))
        {
            Debug.Log($"Player doesn't have number {playerNumber}");
            return false;
        }

        return CheckCondition(playerNumber);
    }

    private bool CheckCondition(int playerNumber)
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
            if (!IsAnyDoorUnlocked())
            {
                MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
                if (mathUI != null)
                {
                    mathUI.ShowMathProblem(this);
                    roomManager.PauseGame();
                }
            }
            else
            {
                roomManager.TeleportPlayer(other.gameObject, doorId);
            }
        }
    }

    public void AttemptUnlock(int playerNumber)
    {
        NumberSO playerNumberSO = NumberManager.instance.GetNumber(playerNumber);
        if (playerNumberSO != null && CheckCondition(playerNumber))
        {
            // Remove the player's number first
            NumberInventoryManager.instance.RemoveNumber(playerNumberSO);
            
            // Then defeat the door (which will add the new number)
            Debug.Log($"Player provided the correct number {playerNumber} for door {doorId}");
            Defeat();
        }
        else
        {
            Debug.Log($"Player provided the wrong number for door {doorId}");
        }
    }

    public void Defeat()
    {
        Debug.Log($"Door {doorId} defeated");
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

            if (!HasRewardBeenCollected())
            {
                ExchangeNumbers();
            }
        }
    }

    private void ExchangeNumbers()
    {
        NumberSO doorNumberSO = NumberManager.instance.GetNumber(associatedNumber);
        if (doorNumberSO != null)
        {
            NumberInventoryManager.instance.AddNumber(doorNumberSO);
            Debug.Log($"Added door number {associatedNumber} to inventory");
            
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

    private bool IsAnyDoorUnlocked()
    {
        return roomManager.IsDoorUnlocked(doorId) || 
               (pairedDoorId != 0 && roomManager.IsDoorUnlocked(pairedDoorId));
    }

    private bool HasRewardBeenCollected()
    {
        return roomManager.HasCollectedReward(doorId) || 
               (pairedDoorId != 0 && roomManager.HasCollectedReward(pairedDoorId));
    }
}