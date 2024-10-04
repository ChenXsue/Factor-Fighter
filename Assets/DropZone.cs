using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public MathGameManager gameManager; // Reference to the MathGameManager script

    // DropZone.cs - OnDrop method
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag; // Get the dragged item

        if (droppedItem != null)
        {
            // Log the name of the dropped item to check if it's being handled
            Debug.Log("Dropped item: " + droppedItem.name);

            // Set the dragged item as a child of the equation panel (this DropZone)
            droppedItem.transform.SetParent(transform);

            // Get the RectTransform of the dragged item
            RectTransform droppedRectTransform = droppedItem.GetComponent<RectTransform>();

            // Convert mouse position to the local coordinates of the equation panel
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(), // The RectTransform of the DropZone (equation panel)
                eventData.position,            // The position of the pointer
                eventData.pressEventCamera,    // The camera used during the event
                out localPoint                 // The result will be stored in localPoint
            );

            // Set the dragged item to the correct position in the equation panel
            droppedRectTransform.anchoredPosition = localPoint;

            // Log the position to check if it's being set correctly
            Debug.Log("Position of " + droppedItem.name + ": " + localPoint);
            Debug.Log("Dropped item position: " + droppedRectTransform.anchoredPosition);

            TMPro.TextMeshProUGUI textComponent = droppedItem.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                string value = textComponent.text;
                Debug.Log("Dropped value: " + value);

            }
            else
            {
                Debug.LogError("Dropped item does not contain a TextMeshProUGUI component!");
            }
        }
    }
}