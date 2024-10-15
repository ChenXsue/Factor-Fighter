using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NumberSlot : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public TextMeshProUGUI countText;

    public void SetNumber(NumberSO numberData, int count)
    {
        if (numberData != null)
        {
            numberText.text = numberData.value.ToString();
            countText.text = count > 1 ? count.ToString() : "";
            Debug.Log($"Setting slot for number: {numberData.value}, Count: {count}");
        }
    }
}