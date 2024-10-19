using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberManager : MonoBehaviour
{
    public static NumberManager instance;
    public NumberSO numberTemplate;
    private Dictionary<int, NumberSO> numberInstances = new Dictionary<int, NumberSO>();

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    public NumberSO GetNumber(int value)
    {
        if (!numberInstances.ContainsKey(value))
        {
            NumberSO newNumber = Instantiate(numberTemplate);
            newNumber.value = value;
            newNumber.itemName = value.ToString();
            numberInstances[value] = newNumber;
        }
        return numberInstances[value];
    }
}