using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class NewNumberPanelManager : MonoBehaviour
{
    public NumberInventorySO myNewNumberBag; // 新的数字背包数据
    public GameObject newNumberSlotGrid;     // 新的数字背包 UI 父对象
    public NumberSlot numberSlotPrefab;      // 数字槽的预制体
    public TMP_InputField inputField;        // 输入框
    public GameObject invisibleObject;       // 用于控制可见性
    public Button backspaceButton;           // 退格按钮
    public Button submitButton;              // 提交按钮
    public Button returnButton;              // 返回按钮
    public int targetNumber = 16;            // 目标数字
    public int correctAnswer = 4;            // 正确答案
    [SerializeField] private GameObject question;

    private Dictionary<int, NumberSlot> activeSlots = new Dictionary<int, NumberSlot>();
    private InputManager inputManager;

    void OnEnable()
    {
        inputManager = FindObjectOfType<InputManager>();
        RefreshNewNumberInventory();
        SetupButtons();
    }

    private void SetupButtons()
    {
        if (inputField != null)
        {
            inputField.interactable = false;
        }
        if (backspaceButton != null)
        {
            backspaceButton.onClick.AddListener(OnBackspaceClicked);
        }
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitClicked);
        }
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(OnReturnClicked);
        }
    }

    public void RefreshNewNumberInventory()
    {
        HashSet<int> usedNumbers = new HashSet<int>();

        foreach (Item item in myNewNumberBag.Items)
        {
            if (item is NumberSO numberItem)
            {
                if (activeSlots.TryGetValue(numberItem.value, out NumberSlot slot))
                {
                    slot.SetNumber(numberItem, numberItem.count);
                }
                else
                {
                    NumberSlot newSlot = Instantiate(numberSlotPrefab, newNumberSlotGrid.transform);
                    newSlot.SetNumber(numberItem, numberItem.count);
                    activeSlots[numberItem.value] = newSlot;
                    newSlot.SetOnClickListener(() => OnNumberSlotClick(newSlot));
                }
                usedNumbers.Add(numberItem.value);
            }
        }

        List<int> numbersToRemove = new List<int>();
        foreach (var kvp in activeSlots)
        {
            if (!usedNumbers.Contains(kvp.Key))
            {
                Destroy(kvp.Value.gameObject);
                numbersToRemove.Add(kvp.Key);
            }
        }
        foreach (int number in numbersToRemove)
        {
            activeSlots.Remove(number);
        }
    }

    private void OnNumberSlotClick(NumberSlot slot)
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (slot == null || inputField == null) return;
        if (!string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("Input field already has a number");
            return;
        }
        string number = slot.numberText.text;
        inputField.text = number;
        NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));
        if (numberToRemove != null)
        {
            myNewNumberBag.RemoveItem(numberToRemove);
            RefreshNewNumberInventory();
            NumberInventoryManager.instance.RefreshNumberInventory();
        }
    }

    public void OnBackspaceClicked()
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (string.IsNullOrEmpty(inputField.text)) return;
        int number = int.Parse(inputField.text);
        inputField.text = "";
        NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
        if (numberToAdd != null)
        {
            myNewNumberBag.AddItem(numberToAdd);
            RefreshNewNumberInventory();
            NumberInventoryManager.instance.RefreshNumberInventory();
        }
    }

    public void OnSubmitClicked()
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (string.IsNullOrEmpty(inputField.text)) return;
        int submittedNumber = int.Parse(inputField.text);

        if (submittedNumber == correctAnswer)
        {
            if (question != null)
            {
                question.SetActive(false);
            }
            CloseAllPanels();
            Number_Wall.ResumeGame();
            NumberInventoryManager.instance.RefreshNumberInventory();
        }
        else
        {
            Debug.Log("Wrong answer!");
        }
    }

    public void OnReturnClicked()
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;

        if (!string.IsNullOrEmpty(inputField.text))
        {
            OnBackspaceClicked();
        }
        CloseAllPanels();
        Number_Wall.ResumeGame();
        NumberInventoryManager.instance.RefreshNumberInventory();
    }

    private void CloseAllPanels()
    {
        if (invisibleObject != null)
        {
            invisibleObject.SetActive(false);
        }
        inputField.text = "";
    }
}
