using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OperatorInventoryManager : MonoBehaviour
{
    public static OperatorInventoryManager instance;
    public OperatorInventorySO myOperatorBag;
    public GameObject operatorSlotGrid;
    public OperatorSlot operatorSlotPrefab;
    
    private Dictionary<char, OperatorSlot> activeSlots = new Dictionary<char, OperatorSlot>();

    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    void OnEnable()
    {
        RefreshOperatorInventory();
    }

    public void RefreshOperatorInventory()
    {
        HashSet<char> usedOperators = new HashSet<char>();
        
        foreach (Item item in myOperatorBag.items)
        {
            if (item is OperatorSO operatorItem)
            {
                if (activeSlots.TryGetValue(operatorItem.operatorChar, out OperatorSlot slot))
                {
                    slot.SetOperator(operatorItem, item.count);
                }
                else
                {
                    OperatorSlot newSlot = Instantiate(operatorSlotPrefab, operatorSlotGrid.transform);
                    newSlot.SetOperator(operatorItem, item.count);
                    activeSlots[operatorItem.operatorChar] = newSlot;
                }
                usedOperators.Add(operatorItem.operatorChar);
            }
        }
        
        List<char> operatorsToRemove = new List<char>();
        foreach (var kvp in activeSlots)
        {
            if (!usedOperators.Contains(kvp.Key))
            {
                Destroy(kvp.Value.gameObject);
                operatorsToRemove.Add(kvp.Key);
            }
        }
        foreach (char op in operatorsToRemove)
        {
            activeSlots.Remove(op);
        }
    }

    public void AddOperator(OperatorSO operatorData)
    {
        Debug.Log($"AddOperator called for {operatorData.operatorChar}");
        myOperatorBag.AddItem(operatorData);
        RefreshOperatorInventory();
    }
}