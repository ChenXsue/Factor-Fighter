using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//运算符背包
[CreateAssetMenu(fileName = "New Operator Inventory", menuName = "Inventory/Operator Inventory")]
public class OperatorInventorySO : InventorySO
{
    // 
    protected override bool ItemsAreEqual(Item item1, Item item2)
    {
        if (item1 is OperatorSO op1 && item2 is OperatorSO op2)
        {
            return op1.operatorChar == op2.operatorChar;
        }
        return base.ItemsAreEqual(item1, item2);
    }
}

