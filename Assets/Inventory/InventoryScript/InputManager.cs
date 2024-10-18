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
        Debug.Log("Backspace button clicked");

        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("Input field is empty, nothing to backspace");
            return;
        }

        char lastChar = inputField.text[inputField.text.Length - 1];
        Debug.Log($"Last character removed: {lastChar}");

        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        Debug.Log($"New input field text: {inputField.text}");

        if (char.IsDigit(lastChar))
        {
            int number = int.Parse(lastChar.ToString());
            Debug.Log($"Trying to add number {number} back to inventory");
            NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
            if (numberToAdd != null)
            {
                NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
                NumberInventoryManager.instance.RefreshNumberInventory();
                Debug.Log($"Number {number} added back to inventory");
            }
            else
            {
                Debug.LogWarning($"Failed to get NumberSO for {number}");
            }
        }
        else
        {
            Debug.Log($"Trying to add operator {lastChar} back to inventory");
            OperatorSO operatorToAdd = ScriptableObject.CreateInstance<OperatorSO>();
            operatorToAdd.operatorChar = lastChar;
            operatorToAdd.itemName = lastChar.ToString();
            OperatorInventoryManager.instance.myOperatorBag.AddItem(operatorToAdd);
            OperatorInventoryManager.instance.RefreshOperatorInventory();
            Debug.Log($"Operator {lastChar} added back to inventory");
        }
    }

    public void OnInvisibleObjectStateChanged()
    {
        Debug.Log("OnInvisibleObjectStateChanged called");
        UpdateSlotsInteractability();
    }
}