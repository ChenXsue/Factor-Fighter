using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class WebGLDataLogger : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 确保 WebGLDataLogger 跨场景保留
    }

    private string apiUrl = "https://proxy-server-158552166643.us-central1.run.app/proxy"; // 替换为你的 Google Apps Script URL
    
    private string playerId = "TestPlayer"; // 默认的 playerId
    public static int operatorSum = 0;
    public static int numberSum = 1; //tutorial level has 1 number initially
    public static int numberUsed = 0;
    public static int operatorUsed = 0;
    public static int answerSum = 0;
    public static int wrongNum = 0;

    [System.Serializable]
    private class LogData
    {
        public string playerId;
        public string level;
        public float completionTime;
        public string passFormula;
        public int operatorSum;
        public int numberSum;
        public int operatorUsed;
        public int numberUsed;
        public double accuracy;

        public LogData(string playerId, string level, float completionTime, string passFormula, int operatorSum, 
        int numberSum, int operatorUsed, int numberUsed, double accuracy)
        {
            this.playerId = playerId;
            this.level = level;
            this.completionTime = completionTime;
            this.passFormula = passFormula;
            this.operatorSum = operatorSum;
            this.numberSum = numberSum;
            this.operatorUsed = operatorUsed;
            this.numberUsed = numberUsed;
            this.accuracy = accuracy;
        }
        
    }

    // 设置玩家ID
    public void SetPlayerId(string id)
    {
        playerId = id;
    }

    public IEnumerator LogDataToGoogleSheet(string level, float completionTime, string passFormula)
    {
        foreach (char c in passFormula)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                operatorUsed++;
                numberUsed++;
            }
        }
        numberUsed++;
        if (level.Contains("Level1") || level.Contains("Level3") || level.Contains("Level4"))
        {
            numberSum += 3; // Level 1 has 4 number initially
        }
        if (level.Contains("Level2"))
        {
            numberSum += 3; // Level 2 has 4 number initially
            List<string> solutions = new List<string>
            {
                "12+9",
                "18+3",
                "18+6-3",
                "24-6+3",
                "24-3"
            };
            string result = FindSolution(passFormula, solutions);
            passFormula = result;
        }
        if(passFormula == "Time's up"){
            if(level.Contains("Level1") || level.Contains("Tutorial")){
                completionTime = 90;
            }
            if(level.Contains("Level4")){
                completionTime = 600;
            }
            else{
                completionTime = 300;
            }
        }
        double accuracy = (double)(answerSum - wrongNum) / answerSum;
        // 创建 LogData 实例
        LogData data = new LogData(playerId, level, completionTime, passFormula, operatorSum, numberSum, operatorUsed, numberUsed, accuracy);
        operatorSum = 0;
        numberSum = 1;
        numberUsed = 0;
        operatorUsed = 0;
        answerSum = 0;
        wrongNum = 0;

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
        public static string FindSolution(string input, List<string> solutions)
    {
        // 将输入字符串标准化
        string normalizedInput = NormalizeExpression(input);

        // 遍历预设的解法列表
        foreach (string solution in solutions)
        {
            // 将每个解法标准化
            string normalizedSolution = NormalizeExpression(solution);

            // 比较标准化后的字符串，如果相同，则返回该解法
            if (normalizedInput == normalizedSolution)
            {
                return solution;
            }
        }

        // 如果没有找到匹配的解法
        return input;
    }

    // 标准化表达式，将数字和运算符排序
    public static string NormalizeExpression(string expression)
    {
        // 提取所有数字
        List<string> numbers = Regex.Matches(expression, @"\d+")
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        // 提取所有运算符
        List<string> operators = Regex.Matches(expression, @"[+\-*/]")
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        // 对数字和运算符分别排序
        numbers.Sort();
        operators.Sort();

        // 将排序后的数字和运算符合并成一个标准化字符串
        return string.Join("", numbers) + string.Join("", operators);
    }
}