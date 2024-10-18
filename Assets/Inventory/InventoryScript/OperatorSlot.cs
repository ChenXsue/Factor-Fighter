using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class OperatorSlot : MonoBehaviour
{
    public TextMeshProUGUI operatorText;
    public TextMeshProUGUI countText;
    public Button button; 

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void SetOperator(OperatorSO operatorData, int count)
    {
        if (operatorData != null)
        {
            operatorText.text = operatorData.operatorChar.ToString();
            countText.text = count > 1 ? count.ToString() : "";
            Debug.Log($"Setting slot for {operatorData.operatorChar}, Count: {count}");
        }
    }

    public void SetOnClickListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}