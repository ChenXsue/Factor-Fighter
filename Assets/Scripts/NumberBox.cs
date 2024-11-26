using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberBox : MonoBehaviour 
{
    [Header("Box Settings")]
    public int number;                    // 这个箱子算式的答案
    public float detectionRange = 2f;     // 玩家可以拾取的距离
    public TextMeshPro numberText;        // 显示数字的TMP组件
    public GameObject space; 

    private Transform player;             // 玩家的Transform组件
    private bool isInRange = false;       // 玩家是否在范围内
    private SpriteRenderer spriteRenderer;// 箱子的渲染器组件
    private NumberCarrier carrier;        // 缓存NumberCarrier引用

    void Start()
    {
        // 初始化
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            carrier = player.GetComponent<NumberCarrier>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;  // 设置箱子为黄色
        numberText.enabled = false;           // 初始时隐藏数字
        numberText.sortingOrder = 4; // 数值比 box 的 sortingOrder 大
        space.SetActive(false);

        // numberText.text = formula;  // 设置显示的数字
    }

    void Update()
    {
        if (player == null || carrier == null) return;

        // 计算与玩家的距离
        float distanceToPlayer = Vector2.Distance((Vector2)transform.position, (Vector2)player.position);
        
        // 根据距离显示或隐藏数字
        if (distanceToPlayer <= detectionRange)
        {
            if (!isInRange)
            {
                isInRange = true;
                numberText.enabled = true;
                space.SetActive(true);

                // 通知carrier当前可拾取的箱子
                carrier.SetCurrentBox(this);
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                numberText.enabled = false;
                space.SetActive(false);

                // 如果离开范围，通知carrier清除当前箱子引用
                carrier.SetCurrentBox(null);
            }
        }
    }

    public void PickUp() 
    {
        carrier.PickUpNumber(numberText.text, number);
        Destroy(gameObject);
    }
}