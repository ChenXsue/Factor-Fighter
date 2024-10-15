using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPickup : MonoBehaviour
{
    public int numberValue;
    private bool hasBeenPickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            NumberSO numberSO = NumberManager.instance.GetNumber(numberValue);
            NumberInventoryManager.instance.AddNumber(numberSO);
            Debug.Log($"Picked up number: {numberValue}");
            Destroy(gameObject);
        }
    }
}
