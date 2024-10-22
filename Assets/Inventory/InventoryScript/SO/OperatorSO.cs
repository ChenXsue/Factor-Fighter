using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//操作符，会为每一个操作符创建一个SO，包括PlusOperator，MinusOperator，MultipleOperator和DivideOperator
[CreateAssetMenu(fileName = "New Operator", menuName = "Inventory/Operator")]
public class OperatorSO : Item
{
    public char operatorChar;

    public override string GetDisplayText()
    {
        return operatorChar.ToString();
    }
}