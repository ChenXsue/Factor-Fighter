using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//操作数的SO，用来管理所有的数字，但是使用了工厂模式，用一个NumberSO来管理字典，字典中包括其他的SO
[CreateAssetMenu(fileName = "New Number", menuName = "Inventory/Number")]
public class NumberSO : Item
{
    public int value;

    public override string GetDisplayText()
    {
        return value.ToString();
    }

}

//创建一个 NumberManager 类来管理数字：
// public class NumberManager : MonoBehaviour
// {
//     public NumberSO numberTemplate;
//     private Dictionary<int, NumberSO> numberInstances = new Dictionary<int, NumberSO>();
//
//     public NumberSO GetNumber(int value)
//     {
//         if (!numberInstances.ContainsKey(value))
//         {
//             NumberSO newNumber = Instantiate(numberTemplate);
//             newNumber.value = value;
//             newNumber.itemName = value.ToString();
//             numberInstances[value] = newNumber;
//         }
//         return numberInstances[value];
//     }
// }

//使用 NumberManager：
//
// public class GameManager : MonoBehaviour
// {
//     public NumberManager numberManager;
//     public NumberInventorySO numberInventory;
//
//     public void AddNumberToInventory(int value)
//     {
//         NumberSO numberItem = numberManager.GetNumber(value);
//         numberInventory.AddItem(numberItem);
//     }
// }


