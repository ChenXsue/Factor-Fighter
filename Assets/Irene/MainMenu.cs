using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void level1()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void level2()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void level3()
    {
        SceneManager.LoadSceneAsync("Level2");
    }
    public void level4()
    {
        SceneManager.LoadSceneAsync("Level3");
    }
    public void level5()
    {
        SceneManager.LoadSceneAsync("Level4");
    }

}
