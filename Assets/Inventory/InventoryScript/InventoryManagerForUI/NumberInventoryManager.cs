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

    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    void OnEnable()
    {
        RefreshNumberInventory();
    }

    public void RefreshNumberInventory()
    {
        HashSet<int> usedNumbers = new HashSet<int>();

        foreach (Item item in myNumberBag.Items)
        {
            if (item is NumberSO numberItem)
            {
                if (activeSlots.TryGetValue(numberItem.value, out NumberSlot slot))
                {
                    slot.SetNumber(numberItem, numberItem.count);
                }
                else
                {
                    NumberSlot newSlot = Instantiate(numberSlotPrefab, numberSlotGrid.transform);
                    newSlot.SetNumber(numberItem, numberItem.count);
                    activeSlots[numberItem.value] = newSlot;
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

    public void AddNumber(NumberSO numberData)
    {
        myNumberBag.AddItem(numberData);
        RefreshNumberInventory();
    }
}