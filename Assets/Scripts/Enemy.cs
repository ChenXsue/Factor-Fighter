using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour 
{
    public string enemyId;
    private MathProblem currentProblem;
    public OperatorSO operatorData;
    public GameObject operatorsPanel;
    
    private RoomManager roomManager;
    private bool isDefeated = false; // 直接在组件中记录状态

    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();

        if (string.IsNullOrEmpty(enemyId))
        {
            enemyId = "Enemy_" + GetInstanceID();
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
        if (operatorData != null)
        {
            OperatorInventoryManager.instance.AddOperator(operatorData);
            WebGLDataLogger.operatorSum++;
            Debug.Log($"Added operator {operatorData.operatorChar} to inventory");
        }
        else
        {
            Debug.LogWarning("No operator set for this enemy!");
        }
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


// public class Enemy : MonoBehaviour
// {
//     public string enemyId;
//     private MathProblem currentProblem;
//     public OperatorSO operatorData;
//     public GameObject operatorsPanel;
//     public static bool isGamePaused = false;

//     void Start()
//     {
//         if (string.IsNullOrEmpty(enemyId))
//         {
//             enemyId = "Enemy_" + GetInstanceID();
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
//         if (operatorData != null)
//         {
//             OperatorInventoryManager.instance.AddOperator(operatorData);
//             Debug.Log($"Added operator {operatorData.operatorChar} to inventory");
//         }
//         else
//         {
//             Debug.LogWarning("No operator set for this enemy!");
//         }
//         gameObject.SetActive(false);
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             PauseGame();
//             Debug.Log(this);
  

//             MathProblemUI mathUI = FindObjectOfType<MathProblemUI>();
//             Debug.Log(mathUI);
             
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