using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class RoomDoor : MonoBehaviour 
{
    [Header("Door Settings")]
    public int doorId;
    public int pairedDoorId; // 新增：配对的门ID
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
            // 检查当前门或配对门是否已解锁
            if (!roomManager.IsDoorUnlocked(doorId) && !roomManager.IsDoorUnlocked(pairedDoorId))
            {
                MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
                if (mathUI != null)
                {
                    mathUI.ShowMathProblem(this);
                    roomManager.PauseGame();
                }
                roomManager.TeleportPlayer(other.gameObject, doorId);
            }
            else
            {
                roomManager.TeleportPlayer(other.gameObject, doorId);
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
            // 解锁当前门和配对门
            roomManager.UnlockDoor(doorId);
            if (pairedDoorId != 0)
            {
                roomManager.UnlockDoor(pairedDoorId);
            }

            // 只在首次解锁时添加数字
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

// using UnityEditor.Build.Content;

// public class RoomDoor : MonoBehaviour
// {
//     [Header("Door Settings")]
//     public int doorNumber;
//     public int targetRoomNumber; // 新增：目标房间编号
//     public int associatedNumber;

//     private RoomManager roomManager;
//     private MathProblem currentProblem;
    
//     void Start()
//     {
//         roomManager = FindObjectOfType<RoomManager>();
//         Ene_MathProblemManager problemManager = FindObjectOfType<Ene_MathProblemManager>();
//         currentProblem = problemManager.GetRandomMathProblem();
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && roomManager != null)
//         {
//             if (!roomManager.IsDoorUnlocked(doorNumber))
//             {
//                 MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
//                 mathUI.ShowMathProblem(this);
//                 PauseGame();
//             }
//             else
//             {
//                 // 传送玩家到新位置并切换摄像机
//                 roomManager.TeleportPlayer(other.gameObject, targetRoomNumber);
//             }
//         }
//     }

//     public string GetMathProblem()
//     {
//         return currentProblem.question;
//     }

//     public bool CheckAnswer(int playerAnswer)
//     {
//         return playerAnswer == currentProblem.answer;
//     }

//     public void Defeat()
//     {
//         Debug.Log($"Door {doorNumber} defeated");
//         Unlock();
//     }

//     private void Unlock()
//     {
//         Debug.Log($"Door {doorNumber} unlocked");
//         RoomManager.Instance.UnlockDoor(doorNumber);
        
//         // 获取与门关联的数字并添加到玩家的库存中
//         NumberSO numberSO = NumberManager.instance.GetNumber(associatedNumber);
//         NumberInventoryManager.instance.AddNumber(numberSO);
        
//         Debug.Log($"Obtained number: {associatedNumber} from door {doorNumber}");
//     }

//     // private void OnTriggerEnter2D(Collider2D other)
//     // {
//     //     if (other.CompareTag("Player"))
//     //     {
//     //         if (!RoomManager.Instance.IsDoorUnlocked(doorNumber))
//     //         {
//     //             MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
//     //             mathUI.ShowMathProblem(this);
//     //             PauseGame();
//     //         }
//     //         else
//     //         {
//     //             // Debug.Log("door number: " + doorNumber + " next room player position: " + nextRoomPlayerPosition);
//     //             RoomManager.Instance.playerSpawnPosition = nextRoomPlayerPosition;
//     //             SceneManager.LoadScene(nextRoomScene);
//     //         }
//     //     }
//     // }



//     public static void PauseGame()
//     {
//         Time.timeScale = 0f;
//         isGamePaused = true;
//         Debug.Log("Game paused");
//     }

//     public static void ResumeGame()
//     {
//         Time.timeScale = 1f;
//         isGamePaused = false;
//         Debug.Log("Game resumed");
//     }
// }