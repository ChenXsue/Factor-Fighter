using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Number Inventory", menuName = "Inventory/Number Inventory")]
// public class NumberInventorySO : InventorySO
// {
//     
//     [SerializeField] private List<int> serializedNumbers = new List<int>();
//
//     protected override bool ItemsAreEqual(Item item1, Item item2)
//     {
//         if (item1 is NumberSO num1 && item2 is NumberSO num2)
//         {
//             return num1.value == num2.value;
//         }
//         return base.ItemsAreEqual(item1, item2);
//     }
//
//     private void OnBeforeSerialize()
//     {
//         serializedNumbers.Clear();
//         foreach (var item in items)
//         {
//             if (item is NumberSO numberSO)
//             {
//                 serializedNumbers.Add(numberSO.value);
//             }
//         }
//     }
//
//     private void OnAfterDeserialize()
//     {
//         items.Clear();
//         foreach (var number in serializedNumbers)
//         {
//             var numberSO = CreateInstance<NumberSO>();
//             numberSO.value = number;
//             numberSO.itemName = number.ToString();
//             items.Add(numberSO);
//         }
//     }
// }
public class NumberInventorySO : ScriptableObject
{
    [System.Serializable]
    public class SerializableNumberItem
    {
        public int value;
        public int count;
    }

    [SerializeField] private List<SerializableNumberItem> serializedItems = new List<SerializableNumberItem>();

    public List<Item> Items
    {
        get
        {
            List<Item> result = new List<Item>();
            foreach (var serItem in serializedItems)
            {
                NumberSO numberSO = ScriptableObject.CreateInstance<NumberSO>();
                numberSO.value = serItem.value;
                numberSO.itemName = serItem.value.ToString();
                numberSO.count = serItem.count;
                result.Add(numberSO);
            }
            return result;
        }
    }

    public void AddItem(NumberSO newItem)
    {
        var existingItem = serializedItems.Find(i => i.value == newItem.value);
        if (existingItem != null)
        {
            existingItem.count++;
        }
        else
        {
            serializedItems.Add(new SerializableNumberItem { value = newItem.value, count = 1 });
        }
    }

    public void RemoveItem(NumberSO itemToRemove)
    {
        var existingItem = serializedItems.Find(i => i.value == itemToRemove.value);
        if (existingItem != null)
        {
            existingItem.count--;
            if (existingItem.count <= 0)
            {
                serializedItems.Remove(existingItem);
            }
        }
    }

    public void Clear()
    {
        serializedItems.Clear();
    }
}