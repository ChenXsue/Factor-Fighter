using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OperatorInventoryManager : MonoBehaviour
{
    public static OperatorInventoryManager instance;
    public OperatorInventorySO myOperatorBag;
    public GameObject operatorSlotGrid;
    public OperatorSlot operatorSlotPrefab;

    private int operatorSum = 0;
    private Dictionary<char, OperatorSlot> activeSlots = new Dictionary<char, OperatorSlot>();

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
        operatorSum++;
        myOperatorBag.AddItem(operatorData);
        RefreshOperatorInventory();
    }
}