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
    
    private RoomManager roomManager;
    private MathProblem currentProblem;
    
    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        Ene_MathProblemManager problemManager = FindObjectOfType<Ene_MathProblemManager>();
        if (problemManager != null)
        {
            currentProblem = problemManager.GetRandomMathProblem();
            Debug.Log($"Math problem for door {doorId}: {currentProblem.question}");
        }
    }
    
    public string GetMathProblem()
    {
        return currentProblem.question;
    }
    
    public bool CheckAnswer(int playerAnswer)
    {
        return playerAnswer == currentProblem.answer;
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
                MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
                if (mathUI != null)
                {
                    mathUI.ShowMathProblem(this);
                    roomManager.PauseGame();
                }
            }
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