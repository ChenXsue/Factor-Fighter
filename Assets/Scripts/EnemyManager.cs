using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    private HashSet<string> defeatedEnemies = new HashSet<string>();

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

    public void DefeatEnemy(string enemyId)
    {
        defeatedEnemies.Add(enemyId);
    }

    public bool IsEnemyDefeated(string enemyId)
    {
        return defeatedEnemies.Contains(enemyId);
    }

    public void ResetEnemies()
    {
        defeatedEnemies.Clear();
    }
}