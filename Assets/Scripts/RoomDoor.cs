using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
// using UnityEditor.Build.Content;

public class RoomDoor : MonoBehaviour
{
    private MathProblem currentProblem;
    public GameObject numbersPanel;
    public static bool isGamePaused = false;
    public int doorNumber;
    public string nextRoomScene;
    public int associatedNumber; // 门上标记的数字

    public Vector3 nextRoomPlayerPosition;  // Position where the player will spawn in the next room

    private void Start()
    {
        Ene_MathProblemManager problemManager = FindObjectOfType<Ene_MathProblemManager>();
        currentProblem = problemManager.GetRandomMathProblem();
        Debug.Log($"Math problem for door {doorNumber}: {currentProblem.question}");
    }

    public string GetMathProblem()
    {
        return currentProblem.question;
    }

    public bool CheckAnswer(int playerAnswer)
    {
        return playerAnswer == currentProblem.answer;
    }

    public void Defeat()
    {
        Debug.Log($"Door {doorNumber} defeated");
        Unlock();
    }

    private void Unlock()
    {
        Debug.Log($"Door {doorNumber} unlocked");
        RoomManager.Instance.UnlockDoor(doorNumber);
        
        // 获取与门关联的数字并添加到玩家的库存中
        NumberSO numberSO = NumberManager.instance.GetNumber(associatedNumber);
        NumberInventoryManager.instance.AddNumber(numberSO);
        
        Debug.Log($"Obtained number: {associatedNumber} from door {doorNumber}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!RoomManager.Instance.IsDoorUnlocked(doorNumber))
            {
                MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
                mathUI.ShowMathProblem(this);
                PauseGame();
            }
            else
            {
                // Debug.Log("door number: " + doorNumber + " next room player position: " + nextRoomPlayerPosition);
                RoomManager.Instance.playerSpawnPosition = nextRoomPlayerPosition;
                SceneManager.LoadScene(nextRoomScene);
            }
        }
    }


    public static void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        Debug.Log("Game paused");
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        Debug.Log("Game resumed");
    }
}