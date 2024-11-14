using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Number Inventory", menuName = "Inventory/Number Inventory")]
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
        var existingItem = serializedItems.Find(item => item.value == itemToRemove.value);
        if (existingItem != null)
        {
            existingItem.count--;
            if (existingItem.count <= 0)
            {
                serializedItems.RemoveAll(item => item.value == itemToRemove.value);
            }
            Debug.Log($"Removed number {itemToRemove.value} from bag. Remaining count: {(existingItem.count <= 0 ? 0 : existingItem.count)}");
        }
        else
        {
            Debug.LogWarning($"Attempted to remove number {itemToRemove.value} but it wasn't found in the bag");
        }
    }

    public bool HasItem(int value)
    {
        return serializedItems.Exists(item => item.value == value && item.count > 0);
    }


    public void Clear()
    {
        serializedItems.Clear();
    }
}
