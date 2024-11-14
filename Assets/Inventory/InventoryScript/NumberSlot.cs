using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class NumberSlot : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public TextMeshProUGUI countText;
    public Button button;
    private NumberSO numberSO;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void SetNumber(NumberSO numberData, int count)
    {
        if (numberData != null)
        {
            numberSO = numberData;
            numberText.text = numberData.value.ToString();
            countText.text = count > 1 ? count.ToString() : "";
            Debug.Log($"Setting slot for number: {numberData.value}, Count: {numberData.count}");

        }

        else
        {
            numberSO = null;
            numberText.text = "";
            countText.text = "";
        }
    }

    public NumberSO GetNumber()
    {
        return numberSO;
    }

    public void SetOnClickListener(UnityAction action)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}