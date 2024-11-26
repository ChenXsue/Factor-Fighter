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
