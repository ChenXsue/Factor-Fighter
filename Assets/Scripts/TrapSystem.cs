using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapSystem : MonoBehaviour 
{
    [Header("Trap Settings")]
    public List<GameObject> spikeTraps = new List<GameObject>();
    public GameObject equationBox;        // 方程框对象（黄色半透明方块）
    public TextMeshPro equationText;      // 显示方程的文本
    public float detectionRange = 2f;     // 检测范围
    public int expectedResult = 4;        // 期望的结果（比如8/2=4）

    public GameObject InputManagerObject;

    private Transform player;
    private bool isInRange = false;
    private bool isSolved = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        equationText.enabled = false;

        // 查找所有尖刺
        GameObject[] spikes = GameObject.FindGameObjectsWithTag("Spike");
        foreach (GameObject spike in spikes)
        {
            spikeTraps.Add(spike);
        }
    }

    void Update()
    {
        if (isSolved) return;

        // 直接使用equationBox的位置计算距离
        float distanceToPlayer = Vector2.Distance((Vector2)equationBox.transform.position, (Vector2)player.position);
        
        // 根据距离显示或隐藏方程
        if (distanceToPlayer <= detectionRange)
        {
            if (!isInRange)
            {
                isInRange = true;
                equationText.enabled = true;
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                equationText.enabled = false;
            }
        }
    }

    public bool TryPlaceNumber(int number)
    {
        if (!isInRange || isSolved) return false;

        if (number == expectedResult)
        {
            SolvePuzzle();
            return true;
        }
        return false;
    }

    void SolvePuzzle()
    {
        isSolved = true;
        
        // 销毁所有相关的尖刺
        foreach (GameObject spike in spikeTraps)
        {
            if (spike != null)
            {
                Destroy(spike);
            }
        }
        spikeTraps.Clear();
        
        // 销毁方程框
        Destroy(equationBox);
        Destroy(gameObject);

        InputManager inputManager = InputManagerObject.GetComponent<InputManager>();
        inputManager.TrapSolved();

        /*
        NumberSO numberSO = NumberManager.instance.GetNumber(expectedResult);
        NumberInventoryManager.instance.AddNumber(numberSO);
        */
    }

    // // 在场景视图中显示检测范围
    // void OnDrawGizmos()
    // {
    //     if (equationBox != null)
    //     {
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawWireSphere(equationBox.transform.position, detectionRange);
    //     }
    // }
}