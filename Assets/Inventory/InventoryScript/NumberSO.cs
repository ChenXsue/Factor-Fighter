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
