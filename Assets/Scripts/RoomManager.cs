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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
    }
}