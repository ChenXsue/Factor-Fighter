using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class operator1 : MonoBehaviour
{
    public static operator1 OperatorInstance;

    public List<char> Operators_Bag = new List<char>();

    public void Awake()
    {
        OperatorInstance = this;
    }
    public int value = '+';
    
    [SerializeField] TextMeshProUGUI value_text;
    // Update is called once per frame
    void Update()
    {
        
    }
}
