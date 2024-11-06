using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// NumberCarrier.cs - 挂载在玩家对象上
public class NumberCarrier : MonoBehaviour
{
    [Header("Visual Settings")]
    public GameObject numberCirclePrefab;     // 头顶圆圈的预制体

    public GameObject boxPrefab;              // 新增：箱子预制体
    public Vector2 circleOffset = new Vector2(0, 1f);  // 圆圈在头顶的偏移位置
    
    private GameObject currentNumberCircle;    // 当前头顶的圆圈实例
    private int carriedNumber = 0;            // 当前携带的数字
    private bool isCarrying = false;          // 是否正在携带数字

    // 拾取数字的方法
    public void PickUpNumber(int number)
    {
        if (!isCarrying)
        {
            carriedNumber = number;
            isCarrying = true;
            CreateNumberCircle();
        }
    }

    // 创建头顶的数字圆圈
    void CreateNumberCircle()
    {
        Vector3 circlePosition = (Vector2)transform.position + circleOffset;
        currentNumberCircle = Instantiate(numberCirclePrefab, circlePosition, Quaternion.identity);
        currentNumberCircle.transform.SetParent(transform);

        // 设置圆圈中的数字
        TextMeshPro numberText = currentNumberCircle.GetComponentInChildren<TextMeshPro>();
        if (numberText != null)
        {
            numberText.text = carriedNumber.ToString();
        }

        // 设置圆圈为黄色
        SpriteRenderer circleRenderer = currentNumberCircle.GetComponent<SpriteRenderer>();
        if (circleRenderer != null)
        {
            circleRenderer.color = Color.yellow;
        }
    }

    void Update()
    {
        if (isCarrying && Input.GetKeyDown(KeyCode.Space))
        {
            // 检查是否在任何方程框范围内
            TrapSystem[] trapSystems = FindObjectsOfType<TrapSystem>();
            bool placedInEquation = false;

            foreach (TrapSystem trap in trapSystems)
            {
                if (trap.TryPlaceNumber(carriedNumber))
                {
                    placedInEquation = true;
                    break;
                }
            }

            if (!placedInEquation)
            {
                // 如果不在方程框范围内，放下新箱子
                DropBox();
            }

            // 清除头顶的数字圆圈
            ClearNumberCircle();
        }
    }

    // 放下箱子
    void DropBox()
    {
        Vector2 dropPosition = (Vector2)transform.position + (Vector2)transform.right * 2f;
        // 使用预制体实例化
        GameObject newBox = Instantiate(boxPrefab, dropPosition, Quaternion.identity);
        
        NumberBox boxScript = newBox.GetComponent<NumberBox>();
        if(boxScript != null)
        {
            boxScript.number = carriedNumber;
            boxScript.numberText.text = carriedNumber.ToString();
        }
    }

    // 清除头顶的数字圆圈
    void ClearNumberCircle()
    {
        if (currentNumberCircle != null)
        {
            Destroy(currentNumberCircle);
        }
        isCarrying = false;
        carriedNumber = 0;
    }

    public bool IsCarrying()
    {
        return isCarrying;
    }

    public int GetCarriedNumber()
    {
        return carriedNumber;
    }
}