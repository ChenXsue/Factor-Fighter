using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorPickup : MonoBehaviour
{
    public OperatorSO operatorData;
    public OperatorInventorySO operatorInventory;
    private bool hasBeenPickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenPickedUp)
        {
            
            hasBeenPickedUp = true;
            OperatorInventoryManager.instance.AddOperator(operatorData);
            Debug.Log($"Picked up {operatorData.operatorChar}");
            Destroy(gameObject);
        }
    }
}