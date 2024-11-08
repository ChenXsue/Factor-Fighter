// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class NumberWallInputManager : MonoBehaviour
// {
//     [Header("UI References")]
//     public TMP_InputField inputField;
//     public GameObject invisibleObject;
//     public Transform numberSlotsParent;
//     public Button backspaceButton;
//     public Button submitButton;
//     public Button returnButton;

//     [Header("Problem Settings")]
//     public int targetNumber = 16;
//     public int correctAnswer = 4;

//     [SerializeField] private GameObject question;

//     private void Start()
//     {
//         if (inputField != null)
//         {
//             inputField.interactable = false;
//         }

//         if (backspaceButton != null)
//         {
//             backspaceButton.onClick.AddListener(OnBackspaceClicked);
//         }

//         if (submitButton != null)
//         {
//             submitButton.onClick.AddListener(OnSubmitClicked);
//         }

//         if (returnButton != null)
//         {
//             returnButton.onClick.AddListener(OnReturnClicked);
//         }

//         // 初始化时设置点击事件
//         SetupNumberSlots();
//     }

//     private void OnEnable()
//     {
//         SetupNumberSlots(); // 启用时设置
//     }

//     private void SetupNumberSlots()
//     {
//         if (numberSlotsParent == null) return;

//         Debug.Log("Setting up number slots");
//         foreach (Transform child in numberSlotsParent)
//         {
//             NumberSlot slot = child.GetComponent<NumberSlot>();
//             if (slot != null)
//             {
//                 Debug.Log($"Setting up slot for number: {slot.numberText.text}");
//                 if (slot.button != null)
//                 {
//                     slot.button.onClick.AddListener(() => OnNumberSlotClick(slot));
//                     Debug.Log($"Added click listener for number: {slot.numberText.text}");
//                 }
//                 else
//                 {
//                     Debug.LogError($"Button is null for slot: {slot.numberText.text}");
//                 }
//             }
//         }
//     }

//     private void OnNumberSlotClick(NumberSlot slot)
//     {
//         Debug.Log($"Attempting to click slot with number: {slot.numberText.text}");
//         if (invisibleObject == null || !invisibleObject.activeSelf)
//         {
//             Debug.Log("Click blocked: invisibleObject check failed");
//             return;
//         }
//         if (slot == null || inputField == null)
//         {
//             Debug.Log("Click blocked: slot or inputField is null");
//             return;
//         }

//         if (!string.IsNullOrEmpty(inputField.text))
//         {
//             Debug.Log("Click blocked: input field already has a number");
//             return;
//         }

//         string number = slot.numberText.text;
//         Debug.Log($"Processing click for number: {number}");
//         inputField.text = number;

//         NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));
//         if (numberToRemove != null)
//         {
//             Debug.Log($"Removing number {number} from inventory");
//             NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
//             NumberInventoryManager.instance.RefreshNumberInventory();
            
//             Debug.Log($"Scheduling SetupNumberSlots after removing {number}");
//             Invoke("SetupNumberSlots", 0.1f);
//         }
//     }

//     public void OnBackspaceClicked()
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (string.IsNullOrEmpty(inputField.text)) return;

//         int number = int.Parse(inputField.text);
//         inputField.text = "";

//         NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
//         if (numberToAdd != null)
//         {
//             NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
//             NumberInventoryManager.instance.RefreshNumberInventory();
            
//             // backspace 后更新
//             SetupNumberSlots();
//         }
//     }

//     public void OnSubmitClicked()
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (string.IsNullOrEmpty(inputField.text)) return;

//         int submittedNumber = int.Parse(inputField.text);
        
//         if (submittedNumber == correctAnswer)
//         {
//             if (question != null)
//             {
//                 question.SetActive(false);
//             }

//             CloseAllPanels();
//             Number_Wall.ResumeGame();
//         }
//         else
//         {
//             Debug.Log("Wrong answer!");
//         }
//     }

//     public void OnReturnClicked()
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
        
//         if (!string.IsNullOrEmpty(inputField.text))
//         {
//             OnBackspaceClicked();
//         }

//         CloseAllPanels();
//         Number_Wall.ResumeGame();
//     }

//     private void CloseAllPanels()
//     {
//         if (invisibleObject != null)
//         {
//             invisibleObject.SetActive(false);
//             SetupNumberSlots(); // 关闭面板前更新
//         }
//         inputField.text = "";
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberWallInputManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField; 
    public GameObject invisibleObject; 
    public Transform numberSlotsParent; 
    public Button backspaceButton;
    public Button submitButton;
    public Button returnButton;

    [Header("Problem Settings")]
    public int targetNumber = 16;
    public int correctAnswer = 4;

    [SerializeField] private GameObject question;

    private float updateInterval = 1f;
    private float lastUpdateTime;

    private void Start()
    {
        if (inputField != null)
        {
            inputField.interactable = false;
        }

        if (backspaceButton != null)
        {
            backspaceButton.onClick.AddListener(OnBackspaceClicked);
        }

        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitClicked);
        }

        if (returnButton != null)
        {
            returnButton.onClick.AddListener(OnReturnClicked);
        }

        // 初始化时设置点击事件
        SetupNumberSlots();
    }

    private void OnEnable()
    { 
        SetupNumberSlots(); // 启用时设置 
    } 

    private void Update()
    {
        if (Time.unscaledTime - lastUpdateTime >= updateInterval)
        {
            if (invisibleObject != null && invisibleObject.activeSelf)
            {
                SetupNumberSlots();
            }
            lastUpdateTime = Time.unscaledTime;
        }
    }

    private void SetupNumberSlots()
    {
        if (numberSlotsParent == null) return;

        Debug.Log("Setting up number slots");
        foreach (Transform child in numberSlotsParent)
        {
            NumberSlot slot = child.GetComponent<NumberSlot>();
            if (slot != null)
            {
                Debug.Log($"Setting up slot for number: {slot.numberText.text}");
                if (slot.button != null)
                {
                    slot.button.onClick.AddListener(() => OnNumberSlotClick(slot));
                    Debug.Log($"Added click listener for number: {slot.numberText.text}");
                }
                else
                {
                    Debug.LogError($"Button is null for slot: {slot.numberText.text}");
                }
            }
        }
    }

    private void OnNumberSlotClick(NumberSlot slot)
    {
        Debug.Log($"Attempting to click slot with number: {slot.numberText.text}");
        if (invisibleObject == null || !invisibleObject.activeSelf)
        {
            Debug.Log("Click blocked: invisibleObject check failed");

            return;
        }
        if (slot == null || inputField == null)
        {
            Debug.Log("Click blocked: slot or inputField is null");
            return;
        }

        if (!string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("Click blocked: input field already has a number");
            return;
        }

        string number = slot.numberText.text;
        Debug.Log($"Processing click for number: {number}");
        inputField.text = number;

        NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));
        if (numberToRemove != null)
        {
            Debug.Log($"Removing number {number} from inventory");
            NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
            NumberInventoryManager.instance.RefreshNumberInventory();
            
            Debug.Log($"Scheduling SetupNumberSlots after removing {number}");
            Invoke("SetupNumberSlots", 0.1f);
        }
    }

    public void OnBackspaceClicked()
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (string.IsNullOrEmpty(inputField.text)) return;

        int number = int.Parse(inputField.text);
        inputField.text = "";

        NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
        if (numberToAdd != null)
        {
            NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
            NumberInventoryManager.instance.RefreshNumberInventory();
            
            // backspace 后更新
            SetupNumberSlots();
        }
    }

    public void OnSubmitClicked()
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (string.IsNullOrEmpty(inputField.text)) return;

        int submittedNumber = int.Parse(inputField.text);
        
        if (submittedNumber == correctAnswer)
        {
            if (question != null)
            {
                question.SetActive(false);
            }

            CloseAllPanels();
            Number_Wall.ResumeGame();
        }
        else
        {
            Debug.Log("Wrong answer!");
        }
    }

    public void OnReturnClicked()
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        
        if (!string.IsNullOrEmpty(inputField.text))
        {
            OnBackspaceClicked();
        }

        CloseAllPanels();
        Number_Wall.ResumeGame();
    }

    private void CloseAllPanels()
    {
        if (invisibleObject != null)
        {
            invisibleObject.SetActive(false);
            //SetupNumberSlots(); // 关闭面板前更新
        }
        inputField.text = "";
    }
}