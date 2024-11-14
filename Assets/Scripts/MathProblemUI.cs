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
    
    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        
        if (answerInputField != null)
        {
            answerInputField.onSubmit.AddListener(OnInputFieldSubmit);
        }
    }

    private void OnDestroy()
    {
        
        if (answerInputField != null)
        {
            answerInputField.onSubmit.RemoveListener(OnInputFieldSubmit);
        }
    }
    
    // 当在输入框中按下回车键时触发
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
                    healthManager.TakeDamage(1);
                    responsePanel.SetActive(true);
                }  
            }
            else if (currentRoomDoor != null)
            {
                // First check if player has this number
                NumberSO numberToUse = NumberManager.instance.GetNumber(playerAnswer);
                if (numberToUse != null && NumberInventoryManager.instance.HasNumber(numberToUse))
                {
                    isCorrect = currentRoomDoor.CheckAnswer(playerAnswer);
                    if (isCorrect)
                    {
                        // The number removal and exchange will be handled in RoomDoor.AttemptUnlock
                        currentRoomDoor.AttemptUnlock(playerAnswer);
                        if (player != null)
                        {
                            roomManager.TeleportPlayer(player, currentRoomDoor.doorId);
                        }
                    }
                    else
                    {
                        healthManager.TakeDamage(1);
                        responsePanel.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("You don't have this number in your inventory!");
                    responsePanel.SetActive(true);
                    return;
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
                    healthManager.TakeDamage(1);
                    responsePanel.SetActive(true);
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