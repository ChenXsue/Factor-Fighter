using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// NumberBox.cs - 挂载在所有可拾取的数字箱子上
public class NumberBox : MonoBehaviour
{
    [Header("Box Settings")]
    public int number;                    // 这个箱子代表的数字
    public float detectionRange = 2f;     // 玩家可以拾取的距离
    public TextMeshPro numberText;        // 显示数字的TMP组件

    private Transform player;             // 玩家的Transform组件
    private bool isInRange = false;       // 玩家是否在范围内
    private SpriteRenderer spriteRenderer;// 箱子的渲染器组件

    void Start()
    {
        // 初始化
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;  // 设置箱子为黄色
        numberText.enabled = false;           // 初始时隐藏数字
        numberText.text = number.ToString();  // 设置显示的数字
    }

    void Update()
    {
        // 计算与玩家的距离
        float distanceToPlayer = Vector2.Distance((Vector2)transform.position, (Vector2)player.position);
        
        // 根据距离显示或隐藏数字
        if (distanceToPlayer <= detectionRange)
        {
            if (!isInRange)
            {
                isInRange = true;
                numberText.enabled = true;
            }

            // 在范围内按空格键拾取
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PickUp();
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                numberText.enabled = false;
            }
        }
    }

    void PickUp()
    {
        // 获取玩家的NumberCarrier组件并尝试拾取
        NumberCarrier carrier = player.GetComponent<NumberCarrier>();
        if (carrier != null && !carrier.IsCarrying())
        {
            carrier.PickUpNumber(number);
            Destroy(gameObject);  // 销毁箱子
        }
    }
}