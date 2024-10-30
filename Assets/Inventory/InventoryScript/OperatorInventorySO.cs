using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Operator Inventory", menuName = "Inventory/Operator Inventory")]
public class OperatorInventorySO : InventorySO
{
    protected override bool ItemsAreEqual(Item item1, Item item2)
    {
        if (item1 is OperatorSO op1 && item2 is OperatorSO op2)
        {
            return op1.operatorChar == op2.operatorChar;
        }
        return base.ItemsAreEqual(item1, item2);
    }

    public override void AddItem(Item newItem)
    {
        if (!(newItem is OperatorSO operatorItem))
        {
            Debug.LogError($"Attempted to add non-operator item to operator inventory: {newItem.GetType()}");
            return;
        }

        Debug.Log($"Adding operator: {(operatorItem as OperatorSO).operatorChar}");

        // 查找是否已存在相同的运算符
        OperatorSO existingOperator = items.Find(item => 
            item is OperatorSO op && 
            (op as OperatorSO).operatorChar == operatorItem.operatorChar) as OperatorSO;

        if (existingOperator != null)
        {
            // 如果存在，只增加计数
            existingOperator.count++;
            Debug.Log($"Increased count for operator {existingOperator.operatorChar}. New count: {existingOperator.count}");
        }
        else
        {
            // 如果不存在，使用原始实例添加到列表中
            operatorItem.count = 1;
            items.Add(operatorItem);
            Debug.Log($"Added new operator {operatorItem.operatorChar} to inventory");
        }
    }
}
