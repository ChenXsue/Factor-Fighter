using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NumberPickup : MonoBehaviour
{
    public int numberValue;
    public GameObject NumberText;
    //public string cubeId; // 确保在Inspector中设置唯一的ID

    private bool isPickedUp;

    private void Start()
    {
        isPickedUp = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPickedUp == false)
        {
            PickUpCube();
        }
    }

    private void PickUpCube()
    {
        // CubeManager.Instance.PickUpCube(cubeId);
        NumberSO numberSO = NumberManager.instance.GetNumber(numberValue);
        NumberInventoryManager.instance.AddNumber(numberSO);
        Debug.Log($"Picked up number: {numberValue}");
        isPickedUp = true;
        NumberText.SetActive(false);
    }
}