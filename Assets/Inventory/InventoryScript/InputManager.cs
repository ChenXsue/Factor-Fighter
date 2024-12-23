using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;  // 在文件顶部添加

public class InputManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public GameObject placeHolder;
    public TMP_InputField numberWallInput;
    public TMP_InputField doorProblemInput;
    public GameObject invisibleObject;
    public GameObject numberWallPanel;
    public GameObject doorProblemPanel;
    public GameObject numberWall1;
    public GameObject numberWall2;
    public GameObject numberWall3;
    public GameObject numberWall4;
    public GameObject numberWallIncorrect;
    public TMP_InputField angelInput;
    public GameObject angelPanel;
    public GameObject trapSystemObject;
    public Transform operatorSlotsParent;
    public Transform numberSlotsParent;
    public Button backspaceButton;
    
    private List<WeakReference<OperatorSlot>> operatorSlots = new List<WeakReference<OperatorSlot>>();
    private List<WeakReference<NumberSlot>> numberSlots = new List<WeakReference<NumberSlot>>();
    private float updateInterval = 0.1f;
    private float lastUpdateTime;
    
    [Header("Operator Presets")]
    [SerializeField] private OperatorSO plusOperator;
    [SerializeField] private OperatorSO minusOperator;
    [SerializeField] private OperatorSO multipleOperator;
    [SerializeField] private OperatorSO divideOperator;

    [SerializeField] private OperatorSO leftParenthesisOperator;   
    [SerializeField] private OperatorSO rightParenthesisOperator;  


    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        placeHolder.SetActive(true);
        // 确保只有一个实例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    

    private void Start()
    {
        Debug.Log("InputManager Start method called");

        if (inputField != null)
        {
            inputField.interactable = false;
        }

        if(numberWallInput != null){
            numberWallInput.interactable = false;
        }

        if(doorProblemInput != null){
            doorProblemInput.interactable = false;
        }

        if (backspaceButton != null)
        {
            Debug.Log("Backspace button reference found");
            //backspaceButton.onClick.AddListener(OnBackspaceClicked);
            Debug.Log("Backspace button listener added");
        }
        else
        {
            Debug.LogError("Backspace button reference is missing!");
        }

        UpdateSlots(); // 初始化时更新一次
    }

    private void Update()
    {
        if (Time.unscaledTime - lastUpdateTime >= updateInterval)
        {
            UpdateSlots();
            lastUpdateTime = Time.unscaledTime;
        }

        if (numberWallPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            NumberWallSubmit();
        }
    }


    private void OnEnable()
    {
        UpdateSlots(); // 启用时更新
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

    public void UpdateNumberSlots()
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

    // private void OnOperatorSlotClick(OperatorSlot slot)
    // {
    //     if (invisibleObject == null || !invisibleObject.activeSelf) return;
    //     if (slot == null || inputField == null) return;

    //     // 获取当前运算符
    //     char operatorChar = slot.operatorText.text[0];
    //     bool isParenthesis = operatorChar == '(' || operatorChar == ')';

    //     // 检查当前输入的最后一个token
    //     string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
    //     // 如果输入为空
    //     if (tokens.Length == 0)
    //     {
    //         // 只允许输入 +、- 和左括号
    //         if (!isParenthesis && operatorChar != '+' && operatorChar != '-')
    //         {
    //             Debug.Log("Can only input +, -, or ( at the beginning");
    //             return;
    //         }
    //         // 如果是右括号，不允许在开始时输入
    //         if (operatorChar == ')')
    //         {
    //             Debug.Log("Cannot input ) at the beginning");
    //             return;
    //         }
    //     }
    //     // 如果最后一个token是运算符
    //     else if (tokens.Length > 0)
    //     {
    //         string lastToken = tokens[tokens.Length - 1];
    //         // 如果最后是运算符，只允许输入左括号
    //         if (IsOperator(lastToken[0]))
    //         {
    //             if (!isParenthesis || operatorChar == ')')
    //             {
    //                 Debug.Log("Cannot input two operators consecutively except for opening parenthesis");
    //                 return;
    //             }
    //         }
    //     }

    //     // 添加空格和运算符
    //     if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
    //     {
    //         inputField.text += " ";
    //     }
    //     inputField.text += operatorChar + " ";

    //     // 从背包中移除运算符（包括括号）
    //     OperatorSO operatorToRemove = OperatorInventoryManager.instance.myOperatorBag.items
    //         .Find(item => item is OperatorSO && (item as OperatorSO).operatorChar == operatorChar) as OperatorSO;

    //     if (operatorToRemove != null)
    //     {
    //         OperatorInventoryManager.instance.myOperatorBag.RemoveItem(operatorToRemove);
    //         OperatorInventoryManager.instance.RefreshOperatorInventory();
    //     }
    // }

    private void OnOperatorSlotClick(OperatorSlot slot)
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (slot == null || inputField == null) return;

        // 获取当前运算符
        char operatorChar = slot.operatorText.text[0];
        bool isParenthesis = operatorChar == '(' || operatorChar == ')';

        // 检查当前输入的最后一个token
        string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
        // 如果输入为空
        if (tokens.Length == 0)
        {
            // 只允许输入 +、- 和左括号
            if (!isParenthesis && operatorChar != '+' && operatorChar != '-')
            {
                Debug.Log("Can only input +, -, or ( at the beginning");
                return;
            }
            // 如果是右括号，不允许在开始时输入
            if (operatorChar == ')')
            {
                Debug.Log("Cannot input ) at the beginning");
                return;
            }
        }
        // 如果有之前的输入
        else if (tokens.Length > 0)
        {
            string lastToken = tokens[tokens.Length - 1];
            
            // 如果最后是右括号，只能接运算符（不能接数字或左括号）
            if (lastToken == ")")
            {
                if (operatorChar == '(' || !IsOperator(operatorChar))
                {
                    Debug.Log("After ), can only input operators except (");
                    return;
                }
            }
            
            // 如果最后是左括号，不能直接接运算符（除了+/-）
            if (lastToken == "(")
            {
                if (IsOperator(operatorChar) && operatorChar != '+' && operatorChar != '-' && operatorChar != '(')
                {
                    Debug.Log("After (, can only input numbers, +, -, or another (");
                    return;
                }
            }
            
            // 如果最后是运算符（不含括号），只能接数字或左括号
            if (IsOperator(lastToken[0]) && lastToken != "(" && lastToken != ")")
            {
                if (operatorChar != '(')
                {
                    Debug.Log("After an operator, can only input numbers or (");
                    return;
                }
            }
            
            // 如果最后是数字，不能接左括号
            if (int.TryParse(lastToken, out _))
            {
                if (operatorChar == '(')
                {
                    Debug.Log("Cannot input ( after a number");
                    return;
                }
            }
            
            // 检查括号匹配
            if (operatorChar == ')')
            {
                int leftCount = tokens.Count(t => t == "(");
                int rightCount = tokens.Count(t => t == ")");
                if (leftCount <= rightCount)
                {
                    Debug.Log("Cannot add ) without matching (");
                    return;
                }
            }
        }

        // 添加空格和运算符
        if (!string.IsNullOrEmpty(inputField.text) && !inputField.text.EndsWith(" "))
        {
            inputField.text += " ";
        }
        inputField.text += operatorChar + " ";

        // 从背包中移除运算符
        OperatorSO operatorToRemove = OperatorInventoryManager.instance.myOperatorBag.items
            .Find(item => item is OperatorSO && (item as OperatorSO).operatorChar == operatorChar) as OperatorSO;

        if (operatorToRemove != null)
        {
            OperatorInventoryManager.instance.myOperatorBag.RemoveItem(operatorToRemove);
            OperatorInventoryManager.instance.RefreshOperatorInventory();
        }
    }

  

    private void OnNumberSlotClick(NumberSlot slot)
    {
        // Number Wall Check
        if (numberWallPanel == null || !numberWallPanel.activeSelf)
        {
            Debug.Log("Number Wall is not active");
        } else {
            Debug.Log("Number Wall is active");
            if (slot == null || numberWallInput == null) return;
            placeHolder.SetActive(false);
            Debug.Log("Number slot clicked for number wall");
            // 检查当前输入的最后一个token
            string[] tokens = numberWallInput.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 0)
            {
                string lastToken = tokens[tokens.Length - 1];
                // 如果最后一个token是数字，不允许继续输入数字
                if (int.TryParse(lastToken, out _))
                {
                    Debug.Log("Cannot input number after number");
                    return;
                }
            }

            string number = slot.numberText.text;
            if (!string.IsNullOrEmpty(numberWallInput.text) && !numberWallInput.text.EndsWith(" "))
            {
                numberWallInput.text += " ";
            }
            numberWallInput.text += number + " ";

            NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));

            if (numberToRemove != null)
            {
                NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
                NumberInventoryManager.instance.RefreshNumberInventory();
            }
        }
        // Angel Check
        if (angelPanel == null || !angelPanel.activeSelf)
        {
            Debug.Log("Angel is not active");
        } else {
            Debug.Log("Angel is active");
            if (slot == null || angelInput == null) return;

            Debug.Log("Number slot clicked for angel");
            // 检查当前输入的最后一个token
            string[] tokens = angelInput.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 0)
            {
                string lastToken = tokens[tokens.Length - 1];
                // 如果最后一个token是数字，不允许继续输入数字
                if (int.TryParse(lastToken, out _))
                {
                    Debug.Log("Cannot input number after number");
                    return;
                }
            }

            string number = slot.numberText.text;
            if (!string.IsNullOrEmpty(angelInput.text) && !angelInput.text.EndsWith(" "))
            {
                angelInput.text += " ";
            }
            angelInput.text += number + " ";

            NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));

            if (numberToRemove != null)
            {
                NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
                NumberInventoryManager.instance.RefreshNumberInventory();
            }
        }

        

        // Door Problem Check
        if (doorProblemPanel == null || !doorProblemPanel.activeSelf)
        {
            Debug.Log("Door problem is not active");
        } else {
            Debug.Log("Door problem is active");
            if (slot == null || doorProblemInput == null) return;

            Debug.Log("Number slot clicked for door problem");
            // 检查当前输入的最后一个token
            string[] tokens = doorProblemInput.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 0)
            {
                string lastToken = tokens[tokens.Length - 1];
                // 如果最后一个token是数字，不允许继续输入数字
                if (int.TryParse(lastToken, out _))
                {
                    Debug.Log("Cannot input number after number");
                    return;
                }
            }

            string number = slot.numberText.text;
            if (!string.IsNullOrEmpty(doorProblemInput.text) && !doorProblemInput.text.EndsWith(" "))
            {
                doorProblemInput.text += " ";
            }
            doorProblemInput.text += number + " ";

            NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));

            if (numberToRemove != null)
            {
                NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
                NumberInventoryManager.instance.RefreshNumberInventory();
            }
        }


        // Final door number check
        if (invisibleObject == null || !invisibleObject.activeSelf) {
            return;
        } else {
            if (slot == null || inputField == null) return;

            // 检查当前输入的最后一个token
            string[] tokens = inputField.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 0)
            {
                string lastToken = tokens[tokens.Length - 1];
                // 如果最后一个token是数字，不允许继续输入数字
                if (lastToken == ")")
                {
                    Debug.Log("Cannot input number after right parenthesis )");
                    return;
                }
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
    }

    public void TrapSolved()
    {
        TrapSystem trapSystem = trapSystemObject.GetComponent<TrapSystem>();
        int expectedResult = trapSystem.expectedResult;
        NumberSO numberSO = NumberManager.instance.GetNumber(expectedResult);
        NumberInventoryManager.instance.myNumberBag.AddItem(numberSO);
        NumberInventoryManager.instance.RefreshNumberInventory();
    }

    public void AngelSubmit(string OperationNumber)
    {
        Debug.Log($"Scene name: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        Debug.Log($"Attempting to submit with operation: {OperationNumber}");
        Debug.Log($"Angel Panel active: {angelPanel.activeSelf}");
        Debug.Log($"Angel Panel component exists: {angelPanel.GetComponent<AngelPanel>() != null}");
        
        var panel = angelPanel.GetComponent<AngelPanel>();
        if (panel != null)
        {
            Debug.Log($"Input Number component exists: {panel.inputNumber != null}");
            Debug.Log($"Operation1 button exists: {panel.Operation1 != null}");
            Debug.Log($"Operation2 button exists: {panel.Operation2 != null}");
        }
    

        if (!string.IsNullOrEmpty(angelInput.text))
        {
            int numberToAdd = angelPanel.GetComponent<AngelPanel>().CalculateOperation(OperationNumber);
            if (numberToAdd == 0)
            {
                Debug.LogError("Invalid number to add: " + numberToAdd);
                return;
            }

            NumberSO numberSO = NumberManager.instance.GetNumber(numberToAdd);
            NumberInventoryManager.instance.myNumberBag.AddItem(numberSO);
            NumberInventoryManager.instance.RefreshNumberInventory();

            angelPanel.GetComponent<AngelPanel>().updateAngel();
            angelPanel.SetActive(false);
            
        }
        return;
    }

    public void NumberWallSubmit()
    {
        bool result = false;
        Number_Wall numberWallInstance;
        Number_Wall collidingInstance = null;
        GameObject numberWallObject = null;

        // Check which number wall we are colliding
        if (numberWall1 != null || numberWall2 != null || numberWall3 != null || numberWall4 != null)
        {
            Debug.Log("Number Wall is active");
            if (numberWall1 != null)
            {
                numberWallInstance = numberWall1.GetComponent<Number_Wall>();
                if (numberWallInstance.isColliding)
                {
                    collidingInstance = numberWallInstance;
                    numberWallObject = numberWall1;
                }
            }
            if (numberWall2 != null)
            {
                numberWallInstance = numberWall2.GetComponent<Number_Wall>();
                if (numberWallInstance.isColliding)
                {
                    collidingInstance = numberWallInstance;
                    numberWallObject = numberWall2;
                }
            }
            if (numberWall3 != null)
            {
                numberWallInstance = numberWall3.GetComponent<Number_Wall>();
                if (numberWallInstance.isColliding)
                {
                    collidingInstance = numberWallInstance;
                    numberWallObject = numberWall3;
                }
            }
            if (numberWall4 != null)
            {
                numberWallInstance = numberWall4.GetComponent<Number_Wall>();
                if (numberWallInstance.isColliding)
                {
                    collidingInstance = numberWallInstance;
                    numberWallObject = numberWall4;
                }
            }
        } else {
            Debug.Log("Number Wall is not active");
            return;
        }
        
        result = collidingInstance.CheckAnswer();
        if (result)
        {
            NumberSO numberToAdd = NumberManager.instance.GetNumber(collidingInstance.areaSize);
            Debug.Log($"Number {collidingInstance.areaSize} added to inventory");
            
            if (numberToAdd != null)
            {
                NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
                NumberInventoryManager.instance.RefreshNumberInventory();
                Debug.Log($"Number {collidingInstance.areaSize} added to inventory");
            }

            numberWallObject.SetActive(false);
        }
        else
        {
            numberWallIncorrect.SetActive(true);
        }
    }

    public void BackpaceSlot(TMP_InputField input)
    {
        if (string.IsNullOrEmpty(input.text))
        {
            Debug.Log("Input field is empty, nothing to backspace");
            return;
        }
        placeHolder.SetActive(true);
        string[] tokens = input.text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0) return;

        string lastToken = tokens[tokens.Length - 1];

        if (input.text.EndsWith(" "))
        {
            input.text = input.text.Substring(0, input.text.Length - lastToken.Length - 1);
        }
        else
        {
            input.text = input.text.Substring(0, input.text.Length - lastToken.Length);
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

        // 在 backspace 后更新
        UpdateSlots();
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
            case '(':
                return leftParenthesisOperator;  
            case ')':
                return rightParenthesisOperator; 
            default:
                Debug.LogError($"Unknown operator character: {operatorChar}");
                return null;
        }
    }
    
    private bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/' || c == '(' || c == ')';
    }

    public void OnInvisibleObjectStateChanged()
    {
        if (invisibleObject != null && invisibleObject.activeSelf)
        {
            UpdateSlots(); // 只在面板显示时更新
        }
    }
}