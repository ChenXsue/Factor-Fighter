using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberCarrier : MonoBehaviour 
{
    [Header("Visual Settings")]
    public GameObject numberCirclePrefab;     // 头顶圆圈的预制体
    public GameObject boxPrefab;              // 箱子预制体
    public Vector2 circleOffset = new Vector2(0, 1f);  // 圆圈在头顶的偏移位置
    
    private GameObject currentNumberCircle;    // 当前头顶的圆圈实例
    private string carriedQuestion = "";            // 当前携带的问题
    private int answer = 0;
    private bool isCarrying = false;          // 是否正在携带数字
    private NumberBox currentBox;             // 当前可拾取的箱子

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isCarrying && currentBox != null)
            {
                // 拾取箱子
                currentBox.PickUp();
            }
            else if (isCarrying)
            {
                // 检查是否在任何方程框范围内
                TrapSystem[] trapSystems = FindObjectsOfType<TrapSystem>();
                bool placedInEquation = false;

                foreach (TrapSystem trap in trapSystems)
                {
                    if (trap.TryPlaceNumber(answer))
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
    }

    // 设置当前可拾取的箱子
    public void SetCurrentBox(NumberBox box)
    {
        currentBox = box;
    }

    // 拾取数字的方法
    public void PickUpNumber(string question, int ans)
    {
        if (!isCarrying)
        {
            carriedQuestion = question;
            answer = ans;
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
        currentNumberCircle.transform.localScale = new Vector3(8f, 6f, 6f); // 拉伸宽度

        // 设置圆圈中的数字
        TextMeshPro numberText = currentNumberCircle.GetComponentInChildren<TextMeshPro>();
        if (numberText != null)
        {
            numberText.text = carriedQuestion;
            numberText.rectTransform.pivot = new Vector2(0.54f, 0.65f);
            numberText.sortingOrder = 1; // 数值比 box 的 sortingOrder 大
            numberText.fontSize = 20; // 设置字体大小为26
        }

        // 设置圆圈为黄色
        SpriteRenderer circleRenderer = currentNumberCircle.GetComponent<SpriteRenderer>();
        if (circleRenderer != null)
        {
            circleRenderer.color = Color.yellow;
            circleRenderer.sortingOrder = 1; // 圆圈在最上层
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
            boxScript.number = answer;
            boxScript.numberText.text = carriedQuestion;
            boxScript.numberText.enableAutoSizing = false; // 禁用 Auto Size
            // boxScript.numberText.margin = new Vector4(-2, 0, 0, 0); // 左边距 -10
            boxScript.numberText.rectTransform.anchorMin = new Vector2(0.5f, 1.5f); // 锚点设置为顶部
            boxScript.numberText.rectTransform.anchorMax = new Vector2(0.5f, 1.5f);
            boxScript.numberText.rectTransform.pivot = new Vector2(0.57f, 0.55f);
            boxScript.numberText.sortingOrder = 1; // 数值比 box 的 sortingOrder 大
            boxScript.numberText.fontSize = 24; // 设置字体大小为26
            // boxScript.numberText.fontStyle = TMPro.FontStyles.Bold; // 设置字体加粗
            // 调试输出
            // Debug.Log("Text is set to: " + boxScript.numberText.text);
            // Debug.Log("Font size: " + boxScript.numberText.fontSize);
            // Debug.Log("Font style: " + boxScript.numberText.fontStyle);


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
        carriedQuestion = "";
    }

    public bool IsCarrying()
    {
        return isCarrying;
    }

    public string GetCarriedNumber()
    {
        return carriedQuestion;
    }
}