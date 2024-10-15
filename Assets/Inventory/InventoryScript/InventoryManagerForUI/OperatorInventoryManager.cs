using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OperatorInventoryManager : MonoBehaviour
{
    public static OperatorInventoryManager instance;
    public OperatorInventorySO myOperatorBag;
    public GameObject operatorSlotGrid;
    public OperatorSlot operatorSlotPrefab;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    void OnEnable()
    {
        RefreshOperatorInventory();
    }
    //
    // public void RefreshOperatorInventory()
    // {
    //     // Clear existing slots
    //     foreach (Transform child in operatorSlotGrid.transform)
    //     {
    //         Destroy(child.gameObject);
    //     }
    //
    //     // Group operators by character and count
    //     Dictionary<char, (OperatorSO operatorSO, int count)> operatorCounts = new Dictionary<char, (OperatorSO, int)>();
    //
    //     foreach (Item item in myOperatorBag.items)
    //     {
    //         if (item is OperatorSO operatorData)
    //         {
    //             if (operatorCounts.ContainsKey(operatorData.operatorChar))
    //             {
    //                 var (existingOp, existingCount) = operatorCounts[operatorData.operatorChar];
    //                 operatorCounts[operatorData.operatorChar] = (existingOp, existingCount + 1);
    //             }
    //             else
    //             {
    //                 operatorCounts[operatorData.operatorChar] = (operatorData, 1);
    //             }
    //             Debug.Log($"Operator: {operatorData.operatorChar}, Count: {operatorCounts[operatorData.operatorChar].count}");
    //         }
    //     }
    //
    //     // Create new slots for each unique operator in the inventory
    //     foreach (var kvp in operatorCounts)
    //     {
    //         OperatorSlot newSlot = Instantiate(operatorSlotPrefab, operatorSlotGrid.transform);
    //         newSlot.SetOperator(kvp.Value.operatorSO, kvp.Value.count);
    //         Debug.Log($"Created slot for operator: {kvp.Value.operatorSO.operatorChar}, Count: {kvp.Value.count}");
    //     }
    // }
    
    public void RefreshOperatorInventory()
    {
        Debug.Log("RefreshOperatorInventory called");
        foreach (Transform child in operatorSlotGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in myOperatorBag.items)
        {
            if (item is OperatorSO operatorItem)
            {
                OperatorSlot newSlot = Instantiate(operatorSlotPrefab, operatorSlotGrid.transform);
                newSlot.SetOperator(operatorItem, item.count);
                Debug.Log($"Created UI slot for operator: {operatorItem.operatorChar}, Count: {item.count}");
            }
        }
    }

    public void AddOperator(OperatorSO operatorData)
    {
        Debug.Log($"AddOperator called for {operatorData.operatorChar}");
        myOperatorBag.AddItem(operatorData);
        RefreshOperatorInventory();
    }
}