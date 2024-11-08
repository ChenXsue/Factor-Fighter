// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// public class NumberInventoryManager : MonoBehaviour
// {
//     public static NumberInventoryManager instance;
//     public NumberInventorySO myNumberBag;
//     public GameObject numberSlotGrid;
//     public NumberSlot numberSlotPrefab;
    
//     private Dictionary<int, NumberSlot> activeSlots = new Dictionary<int, NumberSlot>();

//     void Awake()
//     {
//         if (instance != null) Destroy(this);
//         instance = this;
//     }

//     void OnEnable()
//     {
//         RefreshNumberInventory();
//     }


//     // public void RefreshNumberInventory()
//     // {
//     //     HashSet<int> usedNumbers = new HashSet<int>();
        
//     //     foreach (Item item in myNumberBag.Items)
//     //     {
//     //         if (item is NumberSO numberItem)
//     //         {
//     //             if (activeSlots.TryGetValue(numberItem.value, out NumberSlot slot))
//     //             {
//     //                 slot.SetNumber(numberItem, numberItem.count);
//     //             }
//     //             else
//     //             {
//     //                 NumberSlot newSlot = Instantiate(numberSlotPrefab, numberSlotGrid.transform);
//     //                 newSlot.SetNumber(numberItem, numberItem.count);
//     //                 activeSlots[numberItem.value] = newSlot;
//     //             }
//     //             usedNumbers.Add(numberItem.value);
//     //         }
//     //     }
        
//     //     List<int> numbersToRemove = new List<int>();
//     //     foreach (var kvp in activeSlots)
//     //     {
//     //         if (!usedNumbers.Contains(kvp.Key))
//     //         {
//     //             Destroy(kvp.Value.gameObject);
//     //             numbersToRemove.Add(kvp.Key);
//     //         }
//     //     }
//     //     foreach (int number in numbersToRemove)
//     //     {
//     //         activeSlots.Remove(number);
//     //     }
//     // }
//     public void RefreshNumberInventory()
//     {
//         // 记录已使用的槽位
//         HashSet<int> usedNumbers = new HashSet<int>();

//         foreach (var item in myNumberBag.Items)
//         {
//             if (item is NumberInventorySO.SerializableNumberItem numberItem)
//             {
//                 if (activeSlots.TryGetValue(numberItem.value, out NumberSlot existingSlot))
//                 {
//                     // 更新已有槽位的数据
//                     existingSlot.SetNumber(new NumberSO { value = numberItem.value }, numberItem.count);
//                 }
//                 else
//                 {
//                     // 创建新槽位
//                     NumberSlot newSlot = Instantiate(numberSlotPrefab, numberSlotGrid.transform);
//                     newSlot.SetNumber(new NumberSO { value = numberItem.value }, numberItem.count);
//                     activeSlots[numberItem.value] = newSlot;
//                     newSlot.SetOnClickListener(() => OnNumberSlotClick(newSlot));
//                 }
//                 usedNumbers.Add(numberItem.value);
//             }
//         }

//         // 删除未使用的槽位
//         List<int> numbersToRemove = new List<int>();
//         foreach (var kvp in activeSlots)
//         {
//             if (!usedNumbers.Contains(kvp.Key))
//             {
//                 Destroy(kvp.Value.gameObject);
//                 numbersToRemove.Add(kvp.Key);
//             }
//         }
//         foreach (int number in numbersToRemove)
//         {
//             activeSlots.Remove(number);
//         }
//     }

//     private void OnNumberSlotClick(NumberSlot slot)
//     {
//         Debug.Log($"Number Slot {slot.numberText.text} clicked");
//         // 在这里添加你希望的逻辑，比如将数字信息传递给其他管理器或更新输入字段等
//         NumberWallInputManager.Instance.HandleSlotClick(slot);
//     }




//     public void AddNumber(NumberSO numberData)
//     {
//         myNumberBag.AddItem(numberData);
//         RefreshNumberInventory();
//     }
// }

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