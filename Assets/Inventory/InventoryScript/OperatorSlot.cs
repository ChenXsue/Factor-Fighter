using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OperatorSlot : MonoBehaviour
{
    public TextMeshProUGUI operatorText;
    public TextMeshProUGUI countText;

    public void SetOperator(OperatorSO operatorData, int count)
    {
        if (operatorData != null)
        {
            operatorText.text = operatorData.operatorChar.ToString();
            //countText.text = count.ToString();
            countText.text = count > 1 ? count.ToString() : "";
            Debug.Log($"Setting slot for {operatorData.operatorChar}, Count: {count}");
        }
    }
}