using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject invisibleObject;
    public Transform operatorSlotsParent;
    public Transform numberSlotsParent;
    public Button backspaceButton;
    
    private List<WeakReference<OperatorSlot>> operatorSlots = new List<WeakReference<OperatorSlot>>();
    private List<WeakReference<NumberSlot>> numberSlots = new List<WeakReference<NumberSlot>>();
    private float updateInterval = 1f;
    private float lastUpdateTime;
    
    [Header("Operator Presets")]
    [SerializeField] private OperatorSO plusOperator;
    [SerializeField] private OperatorSO minusOperator;
    [SerializeField] private OperatorSO multipleOperator;
    [SerializeField] private OperatorSO divideOperator;

    private void Start()
    {
        Debug.Log("InputManager Start method called");

        if (inputField != null)
        {
            inputField.interactable = false;
        }

        if (backspaceButton != null)
        {
            Debug.Log("Backspace button reference found");
            backspaceButton.onClick.AddListener(OnBackspaceClicked);
            Debug.Log("Backspace button listener added");
        }
        else
        {
            Debug.LogError("Backspace button reference is missing!");
        }

        UpdateSlots();
        lastUpdateTime = Time.time;
    }

    private void Update()
    {
        UpdateSlotsInteractability();
        
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateSlots();
            lastUpdateTime = Time.time;
        }
    }

    private void UpdateSlots()
    {
        UpdateOperatorSlots();
        UpdateNumberSlots();
    }

    private void UpdateOperatorSlots()
    {
        if (operatorSlotsParent == null) return;

        CleanupDestroyedSlots(operatorSlots);

        foreach (Transform child in operatorSlotsParent)
        {
            if (child == null) continue;

            OperatorSlot slot = child.GetComponent<OperatorSlot>();
            if (slot != null && !operatorSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
            {
                operatorSlots.Add(new WeakReference<OperatorSlot>(slot));
                slot.SetOnClickListener(() => OnOperatorSlotClick(slot));
            }
        }
    }

    private void UpdateNumberSlots()
    {
        if (numberSlotsParent == null) return;

        CleanupDestroyedSlots(numberSlots);

        foreach (Transform child in numberSlotsParent)
        {
            if (child == null) continue;

            NumberSlot slot = child.GetComponent<NumberSlot>();
            if (slot != null && !numberSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
            {
                numberSlots.Add(new WeakReference<NumberSlot>(slot));
                slot.SetOnClickListener(() => OnNumberSlotClick(slot));
            }
        }
    }

    private void CleanupDestroyedSlots<T>(List<WeakReference<T>> slots) where T : class
    {
        slots.RemoveAll(weakRef => !weakRef.TryGetTarget(out _));
    }

    private void UpdateSlotsInteractability()
    {
        if (invisibleObject == null) return;

        bool slotsInteractable = invisibleObject.activeSelf;
        
        foreach (var weakRef in operatorSlots)
        {
            if (weakRef.TryGetTarget(out OperatorSlot slot) && slot != null && slot.button != null)
            {
                slot.button.interactable = slotsInteractable;
            }
        }

        foreach (var weakRef in numberSlots)
        {
            if (weakRef.TryGetTarget(out NumberSlot slot) && slot != null && slot.button != null)
            {
                slot.button.interactable = slotsInteractable;
            }
        }

        if (backspaceButton != null)
        {
            backspaceButton.interactable = slotsInteractable;
        }
    }

    private void OnOperatorSlotClick(OperatorSlot slot)
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (slot == null || inputField == null) return;

        char operatorChar = slot.operatorText.text[0];
        inputField.text += operatorChar;

        OperatorSO operatorToRemove = OperatorInventoryManager.instance.myOperatorBag.items
            .Find(item => item is OperatorSO && (item as OperatorSO).operatorChar == operatorChar) as OperatorSO;

        if (operatorToRemove != null)
        {
            OperatorInventoryManager.instance.myOperatorBag.RemoveItem(operatorToRemove);
            OperatorInventoryManager.instance.RefreshOperatorInventory();
        }
    }

    private void OnNumberSlotClick(NumberSlot slot)
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (slot == null || inputField == null) return;

        int number = int.Parse(slot.numberText.text);
        inputField.text += number.ToString();

        NumberSO numberToRemove = NumberManager.instance.GetNumber(number);

        if (numberToRemove != null)
        {
            NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
            NumberInventoryManager.instance.RefreshNumberInventory();
        }
    }
    
    public void OnBackspaceClicked()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("Input field is empty, nothing to backspace");
            return;
        }

        char lastChar = inputField.text[inputField.text.Length - 1];
        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);

        if (char.IsDigit(lastChar))
        {
            // 数字的处理保持不变
            int number = int.Parse(lastChar.ToString());
            NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
            if (numberToAdd != null)
            {
                NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
                NumberInventoryManager.instance.RefreshNumberInventory();
            }
        }
        else
        {
            // 直接使用预设的OperatorSO
            OperatorSO operatorToAdd = GetOperatorPreset(lastChar);
            if (operatorToAdd != null)
            {
                OperatorInventoryManager.instance.myOperatorBag.AddItem(operatorToAdd);
                OperatorInventoryManager.instance.RefreshOperatorInventory();
                Debug.Log($"Added operator {lastChar} back to inventory");
            }
        }
    }

    public OperatorSO GetOperatorPreset(char operatorChar)
    {
        switch (operatorChar)
        {
            case '+':
                return plusOperator;
            case '-':
                return minusOperator;
            case '*':
                return multipleOperator;
            case '/':
                return divideOperator;
            default:
                Debug.LogError($"Unknown operator character: {operatorChar}");
                return null;
        }
    }

    public void OnInvisibleObjectStateChanged()
    {
        Debug.Log("OnInvisibleObjectStateChanged called");
        UpdateSlotsInteractability();
    }
}