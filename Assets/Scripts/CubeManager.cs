using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeManager : MonoBehaviour
{
    public static CubeManager Instance { get; private set; }
    private HashSet<string> pickedUpCubes = new HashSet<string>();

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

    public void PickUpCube(string cubeId)
    {
        pickedUpCubes.Add(cubeId);
    }

    public bool IsCubePickedUp(string cubeId)
    {
        return pickedUpCubes.Contains(cubeId);
    }

    public void ResetCubes()
    {
        pickedUpCubes.Clear();
    }
}