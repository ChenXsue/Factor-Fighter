using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class RoomManager : MonoBehaviour 
{
    public static RoomManager Instance { get; private set; }
    public Vector3 playerSpawnPosition;
    public bool isFirstSpawn = true;

    private HashSet<int> unlockedDoors = new HashSet<int>();
    private List<int> collectedNumbers = new List<int>();
    // 添加场景位置字典
    private Dictionary<string, Vector3> sceneInitialPositions = new Dictionary<string, Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScenePositions(); // 添加初始位置初始化
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 添加初始位置初始化方法
    private void InitializeScenePositions()
    {
        sceneInitialPositions.Add("SampleScene", new Vector3(2.49f, -3.24f, 0f));
        sceneInitialPositions.Add("Level1", new Vector3(-11.68f, 9.095887f, 0f));
        // 添加其他关卡的初始位置
        sceneInitialPositions.Add("Tutorial", new Vector3(-10.1f, 13.77f, 0f));

    }

    public void UnlockDoor(int doorNumber)
    {
        unlockedDoors.Add(doorNumber);
    }

    public bool IsDoorUnlocked(int doorNumber)
    {
        return unlockedDoors.Contains(doorNumber);
    }

    public void AddNumber(int number)
    {
        collectedNumbers.Add(number);
        Debug.Log($"Added number {number}. Current numbers: {string.Join(", ", collectedNumbers)}");
    }

    public List<int> GetCollectedNumbers()
    {
        return new List<int>(collectedNumbers);
    }

    public void ResetGame()
    {
        unlockedDoors.Clear();
        collectedNumbers.Clear();
        
        // 添加重置玩家位置的代码
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneInitialPositions.ContainsKey(currentScene))
        {
            playerSpawnPosition = sceneInitialPositions[currentScene];
            Debug.Log($"Reset player position for scene {currentScene} to: {playerSpawnPosition}");
        }

        isFirstSpawn = true;
    }
}