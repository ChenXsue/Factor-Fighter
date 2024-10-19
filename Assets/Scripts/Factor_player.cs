using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factor_player : MonoBehaviour
{
    public static Factor_player Instance { get; private set; }

    void Awake()
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
}