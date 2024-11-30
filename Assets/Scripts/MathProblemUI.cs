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
    private RoomManager roomManager;
    private GameObject player;

    [SerializeField] public HealthManager healthManager;

    public float time = 10f;  // 倒计时时间
    private float timer;
    [SerializeField] public TextMeshProUGUI timerText;

    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (answerInputField != null)
        {
            answerInputField.onSubmit.AddListener(OnInputFieldSubmit);
            answerInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        }
    }

    private void OnDestroy()
    {
        if (answerInputField != null)
        {
            answerInputField.onSubmit.RemoveListener(OnInputFieldSubmit);
        }
    }

    private void Update()
    {
        if (mathProblemPanel.activeSelf)
        {
            timer -= Time.unscaledDeltaTime;
            UpdateTimerDisplay();

            if (timer <= 0)
            {
                timer = 0;
                HandleTimeOut();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"0:{seconds:00}";
    }

    private void HandleTimeOut()
    {
        healthManager.TakeDamage(1); // 扣血
        Debug.Log("Current health after taking damage for time out: " + healthManager.currentHealth);

        if (healthManager.currentHealth > 0)
        {
            responsePanel.SetActive(true); // 显示错误反馈
        }
        else
        {
            mathProblemPanel.SetActive(false); // 隐藏问题面板
        }

        Debug.Log("Time ran out!");
        timer = 59;
    }

    private void OnInputFieldSubmit(string text)
    {
        if (mathProblemPanel.activeSelf)
        {
            CheckAnswer();
        }
    }

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
        timer = time;
        UpdateTimerDisplay();
    }

    public void ResetTimer()
    {
        timer = time;
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
                    WebGLDataLogger.answerSum++;
                    currentEnemy.Defeat();
                }
                else
                {
                    WebGLDataLogger.answerSum++;
                    WebGLDataLogger.wrongNum++;
                    HandleWrongAnswer();
                }
            }
            else if (currentRoomDoor != null)
            {
                isCorrect = currentRoomDoor.CheckAnswer(playerAnswer);
                if (isCorrect)
                {
                    currentRoomDoor.Defeat();
                    if (player != null)
                    {
                        roomManager.TeleportPlayer(player, currentRoomDoor.doorId);
                    }
                }
                else
                {
                    HandleWrongAnswer();
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
                    HandleWrongAnswer();
                }
            }

            if (isCorrect)
            {
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

    private void HandleWrongAnswer()
    {
        healthManager.TakeDamage(1);

        if (healthManager.currentHealth > 0)
        {
            responsePanel.SetActive(true); // 显示错误反馈
        }
        else
        {
            mathProblemPanel.SetActive(false); // 隐藏问题面板
        }

        Debug.Log("Wrong answer!");
        timer = 59;
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

        currentEnemy = null;
        currentBasicEnemy = null;
        currentRoomDoor = null;
    }
}
