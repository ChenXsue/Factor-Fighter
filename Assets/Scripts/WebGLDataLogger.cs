using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WebGLDataLogger : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 确保 WebGLDataLogger 跨场景保留
    }

    private string apiUrl = "https://proxy-server-158552166643.us-central1.run.app/proxy"; // 替换为你的 Google Apps Script URL
    
    private string playerId = "TestPlayer"; // 默认的 playerId

    [System.Serializable]
    private class LogData
    {
        public string playerId;
        public string level;
        public float completionTime;
        public string passFormula;

        public LogData(string playerId, string level, float completionTime, string passFormula)
        {
            this.playerId = playerId;
            this.level = level;
            this.completionTime = completionTime;
            this.passFormula = passFormula;
        }
    }

    // 设置玩家ID
    public void SetPlayerId(string id)
    {
        playerId = id;
    }

    public IEnumerator LogDataToGoogleSheet(string level, float completionTime, string passFormula)
    {
        if (level.Contains("Sample"))
        {
            level = "Level1";
        }
        if (level.Contains("Level2"))
        {
            level = "Level2";
        }
        if (passFormula.Contains("9"))
        {
            passFormula = "12+9";
        }
        if (passFormula.Contains("8"))
        {
            passFormula = "18+6-3";
        }
        if (passFormula.Contains("4"))
        {
            passFormula = "24-6+3";
        }
        // 创建 LogData 实例
        LogData data = new LogData(playerId, level, completionTime, passFormula);

        // 序列化 LogData 实例到 JSON
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log("Sending JSON data: " + jsonData); // 打印 JSON 数据到控制台，方便调试

        var request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 发送请求
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data logged to Google Sheets successfully.");
        }
        else
        {
            Debug.LogError("Failed to log data: " + request.error);
        }
    }
}