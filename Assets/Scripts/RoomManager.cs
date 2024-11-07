using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class RoomManager : MonoBehaviour 
{
    [System.Serializable]
    public class DoorTeleportPoint
    {
        public int doorId;
        public Transform spawnPoint;
        public int targetRoom;
    }

    [Header("Room Settings")]
    public DoorTeleportPoint[] doorTeleportPoints;
    public Transform defaultSpawnPoint;
    
    [Header("Camera References")]
    public CameraManager cameraManager;

    private HashSet<int> unlockedDoors = new HashSet<int>();
    private HashSet<int> rewardCollectedDoors = new HashSet<int>(); // 新增：记录已收集奖励的门
    private bool isGamePaused = false;

    public void UnlockDoor(int doorId)
    {
        unlockedDoors.Add(doorId);
    }
    
    public bool IsDoorUnlocked(int doorId)
    {
        return unlockedDoors.Contains(doorId);
    }

    // 新增：检查门是否已收集奖励
    public bool HasCollectedReward(int doorId)
    {
        return rewardCollectedDoors.Contains(doorId);
    }

    // 新增：标记门已收集奖励
    public void MarkRewardCollected(int doorId)
    {
        rewardCollectedDoors.Add(doorId);
    }

    public void TeleportPlayer(GameObject player, int doorId)
    {
        var doorPoint = System.Array.Find(doorTeleportPoints, p => p.doorId == doorId);
        if (doorPoint != null)
        {
            player.transform.position = doorPoint.spawnPoint.position;
            if (cameraManager != null)
            {
                cameraManager.SwitchToRoom(doorPoint.targetRoom);
            }
        }
    }

    public void ResetGame()
    {
        unlockedDoors.Clear();
        rewardCollectedDoors.Clear(); // 新增：重置已收集奖励的记录
        isGamePaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }
}

// // public class RoomManager : MonoBehaviour 
// // {
// //     public static RoomManager Instance { get; private set; }
// //     public Vector3 playerSpawnPosition;
// //     public bool isFirstSpawn = true;

// //     private HashSet<int> unlockedDoors = new HashSet<int>();
// //     private List<int> collectedNumbers = new List<int>();
// //     // 添加场景位置字典
// //     private Dictionary<string, Vector3> sceneInitialPositions = new Dictionary<string, Vector3>();

// //     private void Awake()
// //     {
// //         if (Instance == null)
// //         {
// //             Instance = this;
// //             DontDestroyOnLoad(gameObject);
// //             InitializeScenePositions(); // 添加初始位置初始化
// //         }
// //         else
// //         {
// //             Destroy(gameObject);
// //         }
// //     }

// //     // 添加初始位置初始化方法
// //     private void InitializeScenePositions()
// //     {
// //         sceneInitialPositions.Add("SampleScene", new Vector3(2.49f, -3.24f, 0f));
// //         sceneInitialPositions.Add("Level1", new Vector3(-8.19f, 9.095887f, 0f));
// //         // 添加其他关卡的初始位置
// //         sceneInitialPositions.Add("Tutorial", new Vector3(-10.1f, 13.77f, 0f));

// //     }

// //     public void UnlockDoor(int doorNumber)
// //     {
// //         unlockedDoors.Add(doorNumber);
// //     }

// //     public bool IsDoorUnlocked(int doorNumber)
// //     {
// //         return unlockedDoors.Contains(doorNumber);
// //     }

// //     public void AddNumber(int number)
// //     {
// //         collectedNumbers.Add(number);
// //         Debug.Log($"Added number {number}. Current numbers: {string.Join(", ", collectedNumbers)}");
// //     }

// //     public List<int> GetCollectedNumbers()
// //     {
// //         return new List<int>(collectedNumbers);
// //     }

// //     public void ResetGame()
// //     {
// //         unlockedDoors.Clear();
// //         collectedNumbers.Clear();
        
// //         // 添加重置玩家位置的代码
// //         string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
// //         if (sceneInitialPositions.ContainsKey(currentScene))
// //         {
// //             playerSpawnPosition = sceneInitialPositions[currentScene];
// //             Debug.Log($"Reset player position for scene {currentScene} to: {playerSpawnPosition}");
// //         }

// //         isFirstSpawn = true;
// //     }
// // }

// // 改造后的RoomManager
// public class RoomManager : MonoBehaviour 
// {
//     [System.Serializable]
//     public class DoorTeleportPoint
//     {
//         public int doorNumber;
//         public Transform spawnPoint;
//     }

//     [Header("Room Settings")]
//     public DoorTeleportPoint[] doorTeleportPoints;
//     public Transform defaultSpawnPoint;
    
//     [Header("Camera References")]
//     public CameraManager cameraManager;

//     private HashSet<int> unlockedDoors = new HashSet<int>();
    
//     public void UnlockDoor(int doorNumber)
//     {
//         unlockedDoors.Add(doorNumber);
//     }
    
//     public bool IsDoorUnlocked(int doorNumber)
//     {
//         return unlockedDoors.Contains(doorNumber);
//     }

//     public void TeleportPlayer(GameObject player, int doorNumber)
//     {
//         var doorPoint = System.Array.Find(doorTeleportPoints, p => p.doorNumber == doorNumber);
//         if (doorPoint != null)
//         {
//             player.transform.position = doorPoint.spawnPoint.position;
//             // 切换到对应房间的摄像机
//             if (cameraManager != null)
//             {
//                 cameraManager.SwitchToRoom(doorNumber);
//             }
//         }
//     }

//     public void ResetGame()
//     {
//         unlockedDoors.Clear();
//     }
// }