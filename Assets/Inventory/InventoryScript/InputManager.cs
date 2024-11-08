// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System;
// using System.Collections.Generic;

// public class InputManager : MonoBehaviour
// {
//     [Header("UI References")]
//     public TMP_InputField inputField;
//     public GameObject invisibleObject;
//     public Transform operatorSlotsParent;
//     public Transform numberSlotsParent;
//     public Button backspaceButton;
    
//     private List<WeakReference<OperatorSlot>> operatorSlots = new List<WeakReference<OperatorSlot>>();
//     private List<WeakReference<NumberSlot>> numberSlots = new List<WeakReference<NumberSlot>>();
//     private float updateInterval = 1f;
//     private float lastUpdateTime;
    
//     [Header("Operator Presets")]
//     [SerializeField] private OperatorSO plusOperator;
//     [SerializeField] private OperatorSO minusOperator;
//     [SerializeField] private OperatorSO multipleOperator;
//     [SerializeField] private OperatorSO divideOperator;

//     private void Start()
//     {
//         Debug.Log("InputManager Start method called");

//         if (inputField != null)
//         {
//             inputField.interactable = false;
//         }

//         if (backspaceButton != null)
//         {
//             Debug.Log("Backspace button reference found");
//             backspaceButton.onClick.AddListener(OnBackspaceClicked);
//             Debug.Log("Backspace button listener added");
//         }
//         else
//         {
//             Debug.LogError("Backspace button reference is missing!");
//         }

//         UpdateSlots();
//         lastUpdateTime = Time.time;
//     }

//     private void Update()
//     {
//         if (Time.unscaledTime - lastUpdateTime >= updateInterval)
//         {
//             UpdateSlots();
//             lastUpdateTime = Time.unscaledTime;
//         }
//     }

//     private void UpdateSlots()
//     {
//         UpdateOperatorSlots();
//         UpdateNumberSlots();
//     }

//     private void UpdateOperatorSlots()
//     {
//         if (operatorSlotsParent == null) return;

//         CleanupDestroyedSlots(operatorSlots);

//         foreach (Transform child in operatorSlotsParent)
//         {
//             if (child == null) continue;

//             OperatorSlot slot = child.GetComponent<OperatorSlot>();
//             if (slot != null && !operatorSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
//             {
//                 operatorSlots.Add(new WeakReference<OperatorSlot>(slot));
//                 slot.SetOnClickListener(() => OnOperatorSlotClick(slot));
//             }
//         }
//     }

//     private void UpdateNumberSlots()
//     {
//         if (numberSlotsParent == null) return;

//         CleanupDestroyedSlots(numberSlots);

//         foreach (Transform child in numberSlotsParent)
//         {
//             if (child == null) continue;

//             NumberSlot slot = child.GetComponent<NumberSlot>();
//             if (slot != null && !numberSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
//             {
//                 numberSlots.Add(new WeakReference<NumberSlot>(slot));
//                 slot.SetOnClickListener(() => OnNumberSlotClick(slot));
//             }
//         }
//     }

//     private void CleanupDestroyedSlots<T>(List<WeakReference<T>> slots) where T : class
//     {
//         slots.RemoveAll(weakRef => !weakRef.TryGetTarget(out _));
//     }

//     private void OnOperatorSlotClick(OperatorSlot slot)
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (slot == null || inputField == null) return;

//         // 检查当前输入的最后一个token
//         string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
//         // 如果输入为空，只允许输入+ -
//         if (tokens.Length == 0)
//         {
//             char operatorChar = slot.operatorText.text[0];
//             if (operatorChar != '+' && operatorChar != '-')
//             {
//                 Debug.Log("Can only input + or - at the beginning");
//                 return;
//             }
//         }
//         // 如果最后一个token是运算符，不允许继续输入运算符
//         else if (tokens.Length > 0 && IsOperator(tokens[tokens.Length - 1][0]))
//         {
//             Debug.Log("Cannot input two operators consecutively");
//             return;
//         }

//         char op = slot.operatorText.text[0];
//         if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
//         {
//             inputField.text += " ";
//         }
//         inputField.text += op + " ";

//         OperatorSO operatorToRemove = OperatorInventoryManager.instance.myOperatorBag.items
//             .Find(item => item is OperatorSO && (item as OperatorSO).operatorChar == op) as OperatorSO;

//         if (operatorToRemove != null)
//         {
//             OperatorInventoryManager.instance.myOperatorBag.RemoveItem(operatorToRemove);
//             OperatorInventoryManager.instance.RefreshOperatorInventory();
//         }
//     }

//     private void OnNumberSlotClick(NumberSlot slot)
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (slot == null || inputField == null) return;

//         // 检查当前输入的最后一个token
//         string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//         if (tokens.Length > 0)
//         {
//             string lastToken = tokens[tokens.Length - 1];
//             // 如果最后一个token是数字，不允许继续输入数字
//             if (int.TryParse(lastToken, out _))
//             {
//                 Debug.Log("Cannot input number after number, please input an operator first");
//                 return;
//             }
//         }

//         string number = slot.numberText.text;
//         if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
//         {
//             inputField.text += " ";
//         }
//         inputField.text += number + " ";

//         NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));

//         if (numberToRemove != null)
//         {
//             NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
//             NumberInventoryManager.instance.RefreshNumberInventory();
//         }
//     }


//     public void OnBackspaceClicked()
//     {
//         Debug.Log("Backspace button clicked");

//         if (string.IsNullOrEmpty(inputField.text))
//         {
//             Debug.Log("Input field is empty, nothing to backspace");
//             return;
//         }

//         string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//         if (tokens.Length == 0) return;

//         string lastToken = tokens[tokens.Length - 1];

//         if (inputField.text.EndsWith(" "))
//         {
//             inputField.text = inputField.text.Substring(0, inputField.text.Length - lastToken.Length - 1);
//         }
//         else
//         {
//             inputField.text = inputField.text.Substring(0, inputField.text.Length - lastToken.Length);
//         }

//         if (int.TryParse(lastToken, out int number))
//         {
//             NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
//             if (numberToAdd != null)
//             {
//                 NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
//                 NumberInventoryManager.instance.RefreshNumberInventory();
//                 Debug.Log($"Number {number} added back to inventory");
//             }
//         }
//         else if (lastToken.Length == 1 && IsOperator(lastToken[0]))
//         {
//             OperatorSO operatorToAdd = GetOperatorPreset(lastToken[0]);
//             if (operatorToAdd != null)
//             {
//                 OperatorInventoryManager.instance.myOperatorBag.AddItem(operatorToAdd);
//                 OperatorInventoryManager.instance.RefreshOperatorInventory();
//                 Debug.Log($"Added operator {lastToken} back to inventory");
//             }
//         }
//     }

//     public OperatorSO GetOperatorPreset(char operatorChar)
//     {
//         switch (operatorChar)
//         {
//             case '+':
//                 return plusOperator;
//             case '-':
//                 return minusOperator;
//             case '*':
//                 return multipleOperator;
//             case '/':
//                 return divideOperator;
//             default:
//                 Debug.LogError($"Unknown operator character: {operatorChar}");
//                 return null;
//         }
//     }
    
//     private bool IsOperator(char c)
//     {
//         return c == '+' || c == '-' || c == '*' || c == '/';
//     }

//     public void OnInvisibleObjectStateChanged()
//     {
//         // 保留空方法以满足调用
//     }
// }
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System;
// using System.Collections.Generic;

// public class InputManager : MonoBehaviour
// {
//     [Header("UI References")]
//     public TMP_InputField inputField;
//     public GameObject invisibleObject;
//     public Transform operatorSlotsParent;
//     public Transform numberSlotsParent;
//     public Button backspaceButton;
    
//     private List<WeakReference<OperatorSlot>> operatorSlots = new List<WeakReference<OperatorSlot>>();
//     private List<WeakReference<NumberSlot>> numberSlots = new List<WeakReference<NumberSlot>>();
//     private float updateInterval = 1f;
//     private float lastUpdateTime;
    
//     [Header("Operator Presets")]
//     [SerializeField] private OperatorSO plusOperator;
//     [SerializeField] private OperatorSO minusOperator;
//     [SerializeField] private OperatorSO multipleOperator;
//     [SerializeField] private OperatorSO divideOperator;

//     private void Start()
//     {
//         Debug.Log("InputManager Start method called");

//         if (inputField != null)
//         {
//             inputField.interactable = false;
//         }

//         if (backspaceButton != null)
//         {
//             Debug.Log("Backspace button reference found");
//             backspaceButton.onClick.AddListener(OnBackspaceClicked);
//             Debug.Log("Backspace button listener added");
//         }
//         else
//         {
//             Debug.LogError("Backspace button reference is missing!");
//         }

//         UpdateSlots();
//         lastUpdateTime = Time.time;
//     }

//     private void Update()
//     {
//         UpdateSlotsInteractability();
    
//         if (Time.unscaledTime - lastUpdateTime >= updateInterval)
//         {
//             UpdateSlots();
//             lastUpdateTime = Time.unscaledTime;
//         }
//     }

//     private void UpdateSlots()
//     {
//         UpdateOperatorSlots();
//         UpdateNumberSlots();
//     }

//     private void UpdateOperatorSlots()
//     {
//         if (operatorSlotsParent == null) return;

//         CleanupDestroyedSlots(operatorSlots);

//         foreach (Transform child in operatorSlotsParent)
//         {
//             if (child == null) continue;

//             OperatorSlot slot = child.GetComponent<OperatorSlot>();
//             if (slot != null && !operatorSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
//             {
//                 operatorSlots.Add(new WeakReference<OperatorSlot>(slot));
//                 slot.SetOnClickListener(() => OnOperatorSlotClick(slot));
//             }
//         }
//     }

//     private void UpdateNumberSlots()
//     {
//         if (numberSlotsParent == null) return;

//         CleanupDestroyedSlots(numberSlots);

//         foreach (Transform child in numberSlotsParent)
//         {
//             if (child == null) continue;

//             NumberSlot slot = child.GetComponent<NumberSlot>();
//             if (slot != null && !numberSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
//             {
//                 numberSlots.Add(new WeakReference<NumberSlot>(slot));
//                 slot.SetOnClickListener(() => OnNumberSlotClick(slot));
//             }
//         }
//     }

//     private void CleanupDestroyedSlots<T>(List<WeakReference<T>> slots) where T : class
//     {
//         slots.RemoveAll(weakRef => !weakRef.TryGetTarget(out _));
//     }

//     private void UpdateSlotsInteractability()
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;

//         bool slotsInteractable = invisibleObject.activeSelf;
        
//         foreach (var weakRef in operatorSlots)
//         {
//             if (weakRef.TryGetTarget(out OperatorSlot slot) && slot != null && slot.button != null)
//             {
//                 slot.button.interactable = slotsInteractable;
//             }
//         }

//         foreach (var weakRef in numberSlots)
//         {
//             if (weakRef.TryGetTarget(out NumberSlot slot) && slot != null && slot.button != null)
//             {
//                 slot.button.interactable = slotsInteractable;
//             }
//         }

//         if (backspaceButton != null)
//         {
//             backspaceButton.interactable = slotsInteractable;
//         }
//     }

//     private void OnOperatorSlotClick(OperatorSlot slot)
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (slot == null || inputField == null) return;

//         // 检查当前输入的最后一个token
//         string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
//         // 如果输入为空，只允许输入+ -
//         if (tokens.Length == 0)
//         {
//             char operatorChar = slot.operatorText.text[0];
//             if (operatorChar != '+' && operatorChar != '-')
//             {
//                 Debug.Log("Can only input + or - at the beginning");
//                 return;
//             }
//         }
//         // 如果最后一个token是运算符，不允许继续输入运算符
//         else if (tokens.Length > 0 && IsOperator(tokens[tokens.Length - 1][0]))
//         {
//             Debug.Log("Cannot input two operators consecutively");
//             return;
//         }

//         char op = slot.operatorText.text[0];
//         if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
//         {
//             inputField.text += " ";
//         }
//         inputField.text += op + " ";

//         OperatorSO operatorToRemove = OperatorInventoryManager.instance.myOperatorBag.items
//             .Find(item => item is OperatorSO && (item as OperatorSO).operatorChar == op) as OperatorSO;

//         if (operatorToRemove != null)
//         {
//             OperatorInventoryManager.instance.myOperatorBag.RemoveItem(operatorToRemove);
//             OperatorInventoryManager.instance.RefreshOperatorInventory();
//         }
//     }

//     private void OnNumberSlotClick(NumberSlot slot)
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (slot == null || inputField == null) return;

//         // 检查当前输入的最后一个token
//         string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//         if (tokens.Length > 0)
//         {
//             string lastToken = tokens[tokens.Length - 1];
//             // 如果最后一个token是数字，不允许继续输入数字
//             if (int.TryParse(lastToken, out _))
//             {
//                 Debug.Log("Cannot input number after number, please input an operator first");
//                 return;
//             }
//         }

//         string number = slot.numberText.text;
//         if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
//         {
//             inputField.text += " ";
//         }
//         inputField.text += number + " ";

//         NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));

//         if (numberToRemove != null)
//         {
//             NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
//             NumberInventoryManager.instance.RefreshNumberInventory();
//         }
//     }

//     public void OnBackspaceClicked()
//     {
//         Debug.Log("Backspace button clicked");

//         if (string.IsNullOrEmpty(inputField.text))
//         {
//             Debug.Log("Input field is empty, nothing to backspace");
//             return;
//         }

//         string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//         if (tokens.Length == 0) return;

//         string lastToken = tokens[tokens.Length - 1];

//         if (inputField.text.EndsWith(" "))
//         {
//             inputField.text = inputField.text.Substring(0, inputField.text.Length - lastToken.Length - 1);
//         }
//         else
//         {
//             inputField.text = inputField.text.Substring(0, inputField.text.Length - lastToken.Length);
//         }

//         if (int.TryParse(lastToken, out int number))
//         {
//             NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
//             if (numberToAdd != null)
//             {
//                 NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
//                 NumberInventoryManager.instance.RefreshNumberInventory();
//                 Debug.Log($"Number {number} added back to inventory");
//             }
//         }
//         else if (lastToken.Length == 1 && IsOperator(lastToken[0]))
//         {
//             OperatorSO operatorToAdd = GetOperatorPreset(lastToken[0]);
//             if (operatorToAdd != null)
//             {
//                 OperatorInventoryManager.instance.myOperatorBag.AddItem(operatorToAdd);
//                 OperatorInventoryManager.instance.RefreshOperatorInventory();
//                 Debug.Log($"Added operator {lastToken} back to inventory");
//             }
//         }
//     }

//     public OperatorSO GetOperatorPreset(char operatorChar)
//     {
//         switch (operatorChar)
//         {
//             case '+':
//                 return plusOperator;
//             case '-':
//                 return minusOperator;
//             case '*':
//                 return multipleOperator;
//             case '/':
//                 return divideOperator;
//             default:
//                 Debug.LogError($"Unknown operator character: {operatorChar}");
//                 return null;
//         }
//     }
    
//     private bool IsOperator(char c)
//     {
//         return c == '+' || c == '-' || c == '*' || c == '/';
//     }

//     public void OnInvisibleObjectStateChanged()
//     {
//         Debug.Log("OnInvisibleObjectStateChanged called");
//         UpdateSlotsInteractability();
//     }
// // }
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public GameObject invisibleObject;
    public Transform operatorSlotsParent;
    public Transform numberSlotsParent;
    public Button backspaceButton;
    
    private List<WeakReference<OperatorSlot>> operatorSlots = new List<WeakReference<OperatorSlot>>();
    private List<WeakReference<NumberSlot>> numberSlots = new List<WeakReference<NumberSlot>>();
    private float updateInterval = 1f;
    private float lastUpdateTime;
    
    [Header("Operator Presets")]
    [SerializeField] private OperatorSO plusOperator;
    [SerializeField] private OperatorSO minusOperator;
    [SerializeField] private OperatorSO multipleOperator;
    [SerializeField] private OperatorSO divideOperator;

    private void Start()
    {
        Debug.Log("InputManager Start method called");

        if (inputField != null)
        {
            inputField.interactable = false;
        }

        if (backspaceButton != null)
        {
            Debug.Log("Backspace button reference found");
            backspaceButton.onClick.AddListener(OnBackspaceClicked);
            Debug.Log("Backspace button listener added");
        }
        else
        {
            Debug.LogError("Backspace button reference is missing!");
        }

        UpdateSlots();
        lastUpdateTime = Time.time;
    }

    private void Update()
    {
        if (Time.unscaledTime - lastUpdateTime >= updateInterval)
        {
            UpdateSlots();
            lastUpdateTime = Time.unscaledTime;
        }
    }

    public void UpdateSlots()
    {
        UpdateOperatorSlots();
        UpdateNumberSlots();
    }

    private void UpdateOperatorSlots()
    {
        if (operatorSlotsParent == null) return;

        CleanupDestroyedSlots(operatorSlots);

        foreach (Transform child in operatorSlotsParent)
        {
            if (child == null) continue;

            OperatorSlot slot = child.GetComponent<OperatorSlot>();
            if (slot != null && !operatorSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
            {
                operatorSlots.Add(new WeakReference<OperatorSlot>(slot));
                slot.SetOnClickListener(() => OnOperatorSlotClick(slot));
            }
        }
    }

    private void UpdateNumberSlots()
    {
        if (numberSlotsParent == null) return;

        CleanupDestroyedSlots(numberSlots);

        foreach (Transform child in numberSlotsParent)
        {
            if (child == null) continue;

            NumberSlot slot = child.GetComponent<NumberSlot>();
            if (slot != null && !numberSlots.Exists(weakRef => weakRef.TryGetTarget(out var target) && target == slot))
            {
                numberSlots.Add(new WeakReference<NumberSlot>(slot));
                slot.SetOnClickListener(() => OnNumberSlotClick(slot));
            }
        }
    }

    private void CleanupDestroyedSlots<T>(List<WeakReference<T>> slots) where T : class
    {
        slots.RemoveAll(weakRef => !weakRef.TryGetTarget(out _));
    }

    private void OnOperatorSlotClick(OperatorSlot slot)
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (slot == null || inputField == null) return;

        // 检查当前输入的最后一个token
        string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
        // 如果输入为空，只允许输入+ -
        if (tokens.Length == 0)
        {
            char operatorChar = slot.operatorText.text[0];
            if (operatorChar != '+' && operatorChar != '-')
            {
                Debug.Log("Can only input + or - at the beginning");
                return;
            }
        }
        // 如果最后一个token是运算符，不允许继续输入运算符
        else if (tokens.Length > 0 && IsOperator(tokens[tokens.Length - 1][0]))
        {
            Debug.Log("Cannot input two operators consecutively");
            return;
        }

        char op = slot.operatorText.text[0];
        if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
        {
            inputField.text += " ";
        }
        inputField.text += op + " ";

        OperatorSO operatorToRemove = OperatorInventoryManager.instance.myOperatorBag.items
            .Find(item => item is OperatorSO && (item as OperatorSO).operatorChar == op) as OperatorSO;

        if (operatorToRemove != null)
        {
            OperatorInventoryManager.instance.myOperatorBag.RemoveItem(operatorToRemove);
            OperatorInventoryManager.instance.RefreshOperatorInventory();
        }
    }

    private void OnNumberSlotClick(NumberSlot slot)
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (slot == null || inputField == null) return;

        // 检查当前输入的最后一个token
        string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length > 0)
        {
            string lastToken = tokens[tokens.Length - 1];
            // 如果最后一个token是数字，不允许继续输入数字
            if (int.TryParse(lastToken, out _))
            {
                Debug.Log("Cannot input number after number, please input an operator first");
                return;
            }
        }

        string number = slot.numberText.text;
        if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
        {
            inputField.text += " ";
        }
        inputField.text += number + " ";

        NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));

        if (numberToRemove != null)
        {
            NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
            NumberInventoryManager.instance.RefreshNumberInventory();
        }
    }

    public void OnBackspaceClicked()
    {
        Debug.Log("Backspace button clicked");

        if (string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("Input field is empty, nothing to backspace");
            return;
        }

        string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0) return;

        string lastToken = tokens[tokens.Length - 1];

        if (inputField.text.EndsWith(" "))
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - lastToken.Length - 1);
        }
        else
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - lastToken.Length);
        }

        if (int.TryParse(lastToken, out int number))
        {
            NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
            if (numberToAdd != null)
            {
                NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
                NumberInventoryManager.instance.RefreshNumberInventory();
                Debug.Log($"Number {number} added back to inventory");
            }
        }
        else if (lastToken.Length == 1 && IsOperator(lastToken[0]))
        {
            OperatorSO operatorToAdd = GetOperatorPreset(lastToken[0]);
            if (operatorToAdd != null)
            {
                OperatorInventoryManager.instance.myOperatorBag.AddItem(operatorToAdd);
                OperatorInventoryManager.instance.RefreshOperatorInventory();
                Debug.Log($"Added operator {lastToken} back to inventory");
            }
        }
    }

    public OperatorSO GetOperatorPreset(char operatorChar)
    {
        switch (operatorChar)
        {
            case '+':
                return plusOperator;
            case '-':
                return minusOperator;
            case '*':
                return multipleOperator;
            case '/':
                return divideOperator;
            default:
                Debug.LogError($"Unknown operator character: {operatorChar}");
                return null;
        }
    }
    
    private bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/';
    }

    public void OnInvisibleObjectStateChanged()
    {
        // 保留空方法以满足调用
    }
}
