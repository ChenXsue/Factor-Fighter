using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    // 指定该关卡是否应重置生命值
    public bool resetHealthOnStart = false;

    void Start()
    {
        if (resetHealthOnStart && HealthManager.Instance != null)
        {
            HealthManager.Instance.ResetHealth();
        }
    }
}
