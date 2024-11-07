using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasicEnemy : MonoBehaviour
{
    public string enemyId;
    private MathProblem currentProblem;
    private bool isDefeated = false;
    
    private RoomManager roomManager;

    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();

        if (string.IsNullOrEmpty(enemyId))
        {
            enemyId = "BasicEnemy_" + GetInstanceID();
        }

        Ene_MathProblemManager problemManager = FindObjectOfType<Ene_MathProblemManager>();
        if (problemManager != null)
        {
            currentProblem = problemManager.GetRandomMathProblem();
            Debug.Log("Math problem for this enemy: " + currentProblem.question);
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

    public void Defeat()
    {
        isDefeated = true;
        Debug.Log("Enemy defeated!");
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDefeated)
        {
            if (roomManager != null)
            {
                roomManager.PauseGame();
            }

            MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
            if (mathUI != null)
            {
                mathUI.ShowMathProblem(this);
            }
        }
    }
}






// public class BasicEnemy : MonoBehaviour
// {
//     public string enemyId;
//     private MathProblem currentProblem;
//     public static bool isGamePaused = false;

//     void Start()
//     {
//         if (string.IsNullOrEmpty(enemyId))
//         {
//             enemyId = "BasicEnemy_" + GetInstanceID();
//         }

//         if (EnemyManager.Instance.IsEnemyDefeated(enemyId))
//         {
//             gameObject.SetActive(false);
//             return;
//         }

//         Ene_MathProblemManager problemManager = FindObjectOfType<Ene_MathProblemManager>();
//         currentProblem = problemManager.GetRandomMathProblem();
//         Debug.Log("Math problem for this enemy: " + currentProblem.question);
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
//         EnemyManager.Instance.DefeatEnemy(enemyId);
//         Debug.Log("Enemy defeated!");
//         gameObject.SetActive(false);
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             PauseGame();
//             MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
//             mathUI.ShowMathProblem(this);
//         }
//     }

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