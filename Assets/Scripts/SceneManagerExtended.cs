using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerExtended : MonoBehaviour
{
    public static SceneManagerExtended Instance { get; private set; }
    private HashSet<string> visitedScenes = new HashSet<string>();
    private string currentSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentSceneName = SceneManager.GetActiveScene().name;
            visitedScenes.Add(currentSceneName);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNewScene(string newSceneName)
    {
        if (currentSceneName != newSceneName)
        {
            StartCoroutine(LoadSceneCoroutine(newSceneName));
        }
    }

    private System.Collections.IEnumerator LoadSceneCoroutine(string newSceneName)
    {
        // 淡出效果（可选）
        // yield return StartCoroutine(FadeOutEffect());

        // 卸载当前场景
        if (currentSceneName != null)
        {
            yield return SceneManager.UnloadSceneAsync(currentSceneName);
        }

        // 加载新场景
        yield return SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);

        // 设置新场景为活动场景
        Scene newScene = SceneManager.GetSceneByName(newSceneName);
        SceneManager.SetActiveScene(newScene);

        // 更新当前场景名称
        currentSceneName = newSceneName;

        // 记录已访问的场景
        visitedScenes.Add(newSceneName);

        // 淡入效果（可选）
        // yield return StartCoroutine(FadeInEffect());

        Debug.Log($"Loaded new scene: {newSceneName}");
    }

    public bool HasVisitedScene(string sceneName)
    {
        return visitedScenes.Contains(sceneName);
    }

    // 可选：添加淡入淡出效果的方法
    // private System.Collections.IEnumerator FadeOutEffect() { ... }
    // private System.Collections.IEnumerator FadeInEffect() { ... }
}