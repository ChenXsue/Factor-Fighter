using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberInventoryManager : MonoBehaviour
{
    public static NumberInventoryManager instance;
    public NumberInventorySO myNumberBag;
    public GameObject numberSlotGrid;
    public NumberSlot numberSlotPrefab;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    void OnEnable()
    {
        RefreshNumberInventory();
    }

    // public void RefreshNumberInventory()
    // {
    //     foreach (Transform child in numberSlotGrid.transform)
    //     {
    //         Destroy(child.gameObject);
    //     }
    //
    //     foreach (Item item in myNumberBag.items)
    //     {
    //         if (item is NumberSO numberItem)
    //         {
    //             NumberSlot newSlot = Instantiate(numberSlotPrefab, numberSlotGrid.transform);
    //             newSlot.SetNumber(numberItem, item.count);
    //             Debug.Log($"Created UI slot for number: {numberItem.value}, Count: {item.count}");
    //         }
    //     }
    // }
    
    public void RefreshNumberInventory()
    {
        foreach (Transform child in numberSlotGrid.transform)
        {
            Destroy(child.gameObject);
        }
        

        foreach (Item item in myNumberBag.Items)
        {
            if (item is NumberSO numberItem)
            {
                NumberSlot newSlot = Instantiate(numberSlotPrefab, numberSlotGrid.transform);
                newSlot.SetNumber(numberItem, numberItem.count);
                Debug.Log($"Created UI slot for number: {numberItem.value}, Count: {numberItem.count}");
            }
        }
    }

    public void AddNumber(NumberSO numberData)
    {
        myNumberBag.AddItem(numberData);
        RefreshNumberInventory();
    }
}