

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

//     private List<WeakReference<NumberSlot>> numberSlots = new List<WeakReference<NumberSlot>>();
//     private float updateInterval = 1f;
//     private float lastUpdateTime;

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
//         UpdateNumberSlots();
//     }

//     private void UpdateNumberSlots()
//     {
//         if (numberSlotsParent == null) return;

//         numberSlots.RemoveAll(weakRef => !weakRef.TryGetTarget(out _));

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

//     private void UpdateSlotsInteractability()
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return; 
        
//         bool slotsInteractable = invisibleObject.activeSelf;

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

//         if (submitButton != null)
//         {
//             submitButton.interactable = slotsInteractable;
//         }

//         if (returnButton != null)
//         {
//             returnButton.interactable = slotsInteractable;
//         }
//     }

//     private void OnNumberSlotClick(NumberSlot slot)
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (slot == null || inputField == null) return;

//         if (!string.IsNullOrEmpty(inputField.text))
//         {
//             Debug.Log("Input field already has a number");
//             return;
//         }

//         string number = slot.numberText.text;
//         inputField.text = number;

//         NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));
//         if (numberToRemove != null)
//         {
//             NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
//             NumberInventoryManager.instance.RefreshNumberInventory();
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
//         }
//         inputField.text = "";
//     }

//     public void OnInvisibleObjectStateChanged()
//     {
//         UpdateSlotsInteractability();
//     }
// }
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System;
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
//         SetupNumberSlots();
//     }
//     private void SetupNumberSlots()
//     {
//         if (numberSlotsParent == null) return;
//         foreach (Transform child in numberSlotsParent)
//         {
//             NumberSlot slot = child.GetComponent<NumberSlot>();
//             if (slot != null)
//             {
//                 slot.SetOnClickListener(() => OnNumberSlotClick(slot));
//             }
//         }
//     }
//     private void OnNumberSlotClick(NumberSlot slot)
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (slot == null || inputField == null) return;
//         if (!string.IsNullOrEmpty(inputField.text))
//         {
//             Debug.Log("Input field already has a number");
//             return;
//         }
//         string number = slot.numberText.text;
//         inputField.text = number;
//         NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));
//         if (numberToRemove != null)
//         {
//             NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
//             NumberInventoryManager.instance.RefreshNumberInventory();
//         }
//     }


//     public void OnBackspaceClicked()
//     {
//         if (invisibleObject == null || !invisibleObject.activeSelf) return;
//         if (string.IsNullOrEmpty(inputField.text)) return;

//         // 获取要返回的数字
//         int number = int.Parse(inputField.text);
//         inputField.text = "";
        
//         // 添加回背包并刷新
//         NumberSO numberToAdd = NumberManager.instance.GetNumber(number);
//         if (numberToAdd != null)
//         {
//             Debug.Log($"Adding number {number} back to inventory.");
//             NumberInventoryManager.instance.myNumberBag.AddItem(numberToAdd);
            
//             Debug.Log("Refreshing number inventory...");
//             NumberInventoryManager.instance.RefreshNumberInventory();
            
//             Debug.Log($"Number {number} added back to inventory in NumberWallInputManager");
//         }
//         else
//         {
//             Debug.LogWarning($"Number {number} not found in NumberManager, could not add back to inventory.");
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
//         }
//         inputField.text = "";
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
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
        SetupNumberSlots();
    }
    private void SetupNumberSlots()
    {
        if (numberSlotsParent == null) return;
        foreach (Transform child in numberSlotsParent)
        {
            NumberSlot slot = child.GetComponent<NumberSlot>();
            if (slot != null)
            {
                slot.SetOnClickListener(() => OnNumberSlotClick(slot));
            }
        }
    }
    private void OnNumberSlotClick(NumberSlot slot)
    {
        if (invisibleObject == null || !invisibleObject.activeSelf) return;
        if (slot == null || inputField == null) return;
        if (!string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("Input field already has a number");
            return;
        }
        string number = slot.numberText.text;
        inputField.text = number;
        NumberSO numberToRemove = NumberManager.instance.GetNumber(int.Parse(number));
        if (numberToRemove != null)
        {
            NumberInventoryManager.instance.myNumberBag.RemoveItem(numberToRemove);
            NumberInventoryManager.instance.RefreshNumberInventory();
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
        }
        inputField.text = "";
    }
}
