using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//操作数和操作符的基类
public abstract class Item : ScriptableObject
{
    public string itemName;
    public int count;
    
    
    public abstract string GetDisplayText();
}



