using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
// public class InventorySO : ScriptableObject
// {
//     public List<Item> items = new List<Item>();
//
//     public void AddItem(Item newItem)
//     {
//         Item existingItem = items.Find(item => item.itemName == newItem.itemName);
//         if (existingItem != null)
//         {
//             existingItem.count++;
//             Debug.Log($"Increased count for {existingItem.itemName}. New count: {existingItem.count}");
//         }
//         else
//         {
//             newItem.count = 1;
//             items.Add(newItem);
//             Debug.Log($"Added new item: {newItem.itemName}. Count: 1");
//         }
//     }
//
//     public void RemoveItem(Item itemToRemove)
//     {
//         Item existingItem = items.Find(item => item.itemName == itemToRemove.itemName);
//         if (existingItem != null)
//         {
//             existingItem.count--;
//             if (existingItem.count <= 0)
//             {
//                 items.Remove(existingItem);
//             }
//         }
//     }
//
//     public void Clear()
//     {
//         items.Clear();
//     }
// }
public class InventorySO : ScriptableObject
{
    public List<Item> items = new List<Item>();

    public virtual void AddItem(Item newItem)
    {
        Debug.Log($"AddItem called for {newItem.itemName}");
        Item existingItem = items.Find(item => ItemsAreEqual(item, newItem));
        if (existingItem != null)
        {
            existingItem.count++;
            Debug.Log($"Increased count for {existingItem.itemName}. New count: {existingItem.count}");
        }
        else
        {
            newItem.count = 1;
            items.Add(newItem);
            Debug.Log($"Added new item: {newItem.itemName}. Count: 1");
        }
    }

    protected virtual bool ItemsAreEqual(Item item1, Item item2)
    {
        return item1.itemName == item2.itemName;
    }

    public void RemoveItem(Item itemToRemove)
    {
        Item existingItem = items.Find(item => ItemsAreEqual(item, itemToRemove));
        if (existingItem != null)
        {
            existingItem.count--;
            if (existingItem.count <= 0)
            {
                items.Remove(existingItem);
            }
        }
    }

    public void Clear()
    {
        items.Clear();
    }
}