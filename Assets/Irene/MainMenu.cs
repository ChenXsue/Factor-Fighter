using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void level1()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void level2()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

}
