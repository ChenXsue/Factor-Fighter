using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberInventoryManager : MonoBehaviour
{
    public static NumberInventoryManager instance;
    public NumberInventorySO myNumberBag;
    public GameObject numberSlotGrid;
    public NumberSlot numberSlotPrefab;

    private Dictionary<int, NumberSlot> activeSlots = new Dictionary<int, NumberSlot>();

    public System.Action<NumberSO> onNumberSelected;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void OnEnable()
    {
        RefreshNumberInventory();
    }


    public void RefreshNumberInventory()
    {
        // Clear existing slots
        foreach (var slot in activeSlots.Values)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        activeSlots.Clear();

        // Recreate slots for current inventory
        foreach (var item in myNumberBag.Items)
        {
            if (item is NumberSO numberSO)
            {
                CreateNumberSlot(numberSO);
            }
        }
    }

    private void CreateNumberSlot(NumberSO numberSO)
    {
        NumberSlot newSlot = Instantiate(numberSlotPrefab, numberSlotGrid.transform);
        newSlot.SetNumber(numberSO, 1);
        newSlot.SetOnClickListener(() => OnNumberSlotClicked(numberSO));

        activeSlots[numberSO.value] = newSlot;
    }

    private void OnNumberSlotClicked(NumberSO numberSO)
    {
        Debug.Log($"Number {numberSO.value} clicked");
        onNumberSelected?.Invoke(numberSO);
    }


    public void RemoveNumber(NumberSO numberToRemove)
    {
        if (numberToRemove == null)
        {
            Debug.LogWarning("Attempted to remove null NumberSO");
            return;
        }

        Debug.Log($"Attempting to remove number {numberToRemove.value} from inventory");

        myNumberBag.RemoveItem(numberToRemove);
        
        RefreshNumberInventory();
        
    }

    public bool HasNumber(NumberSO numberSO)
    {
        if (numberSO == null) return false;
        foreach (var item in myNumberBag.Items)
        {
            if (item is NumberSO numItem && numItem.value == numberSO.value)
            {
                return true;
            }
        }
        return false;
    }

    public void AddNumber(NumberSO numberData)
    {
        if (numberData == null)
        {
            Debug.LogWarning("Attempted to add null NumberSO");
            return;
        }

        myNumberBag.AddItem(numberData);
        RefreshNumberInventory();
        Debug.Log($"Added number {numberData.value} to inventory");
    }

    // Helper method to get a number from the inventory by value
    public NumberSO GetNumberByValue(int value)
    {
        foreach (var item in myNumberBag.Items)
        {
            if (item is NumberSO numItem && numItem.value == value)
            {
                return numItem;
            }
        }
        return null;
    }

    // Method to clear the entire inventory
    public void ClearInventory()
    {
        myNumberBag.Clear();
        RefreshNumberInventory();
        Debug.Log("Inventory cleared");
    }

    private void OnDisable()
    {
        // Clean up when disabled
        foreach (var slot in activeSlots.Values)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        activeSlots.Clear();
    }
    
}