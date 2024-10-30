using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathProblemUI : MonoBehaviour
{
    public TextMeshProUGUI problemText;
    public TMP_InputField answerInputField;
    public GameObject mathProblemPanel;

    public GameObject responsePanel;
    
    private Enemy currentEnemy;
    private RoomDoor currentRoomDoor;
    private BasicEnemy currentBasicEnemy;

    public void ShowMathProblem(Enemy enemy)
    {
        currentEnemy = enemy;
        currentRoomDoor = null;
        currentBasicEnemy = null;
        SetupProblem(enemy.GetMathProblem());
    }

    public void ShowMathProblem(RoomDoor roomDoor)
    {
        currentRoomDoor = roomDoor;
        currentEnemy = null;
        currentBasicEnemy = null;
        SetupProblem(roomDoor.GetMathProblem());
    }

    public void ShowMathProblem(BasicEnemy basicEnemy)
    {
        currentBasicEnemy = basicEnemy;
        currentEnemy = null;
        currentRoomDoor = null;
        SetupProblem(basicEnemy.GetMathProblem());
    }

    private void SetupProblem(string problem)
    {
        problemText.text = problem;
        answerInputField.text = "";
        answerInputField.ActivateInputField();
        mathProblemPanel.SetActive(true);
    }

    public void CheckAnswer()
    {
        if (int.TryParse(answerInputField.text, out int playerAnswer))
        {
            bool isCorrect = false;

            if (currentEnemy != null)
            {
                isCorrect = currentEnemy.CheckAnswer(playerAnswer);
                if (isCorrect)
                {
                    currentEnemy.Defeat();
                }
                else
                {
                    mathProblemPanel.SetActive(false);
                    responsePanel.SetActive(true);
                    HealthManager.Instance.TakeDamage(1); // 答错时减少1点生命值
                }
            }
            else if (currentRoomDoor != null)
            {
                isCorrect = currentRoomDoor.CheckAnswer(playerAnswer);
                if (isCorrect)
                {
                    currentRoomDoor.Defeat();
                }
                else
                {
                    mathProblemPanel.SetActive(false);
                    responsePanel.SetActive(true);
                    HealthManager.Instance.TakeDamage(1); // 答错时减少1点生命值
                }
            }
            else if (currentBasicEnemy != null)
            {
                isCorrect = currentBasicEnemy.CheckAnswer(playerAnswer);
                if (isCorrect)
                {
                    currentBasicEnemy.Defeat();
                }
                else
                {
                    mathProblemPanel.SetActive(false);
                    responsePanel.SetActive(true);
                    HealthManager.Instance.TakeDamage(1); // 答错时减少1点生命值
                }
            }

            if (isCorrect)
            {
                mathProblemPanel.SetActive(false);
                Debug.Log("correct");
                mathProblemPanel.SetActive(false);
                ResumeGame();
            }
            else
            {
                Debug.Log("wrong");
            }
        }
        else
        {
            Debug.Log("Invalid input!");
        }
    }

    private void ResumeGame()
    {
        if (currentEnemy != null)
        {
            Enemy.ResumeGame();
        }
        else if (currentBasicEnemy != null)
        {
            BasicEnemy.ResumeGame();
        }
        else if (currentRoomDoor != null)
        {
            RoomDoor.ResumeGame();
        }
        else
        {
            // 以防万一，如果所有当前对象都是null，我们仍然恢复游戏
            Time.timeScale = 1f;
            Debug.Log("Game resumed (fallback)");
        }

        // 重置当前对象
        currentEnemy = null;
        currentBasicEnemy = null;
        currentRoomDoor = null;
    }
}