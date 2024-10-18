using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NumberPickup : MonoBehaviour
{
    public int numberValue;
    public string cubeId; // 确保在Inspector中设置唯一的ID

    private void Start()
    {
        if (string.IsNullOrEmpty(cubeId))
        {
            cubeId = "Cube_" + GetInstanceID();
        }

        if (CubeManager.Instance.IsCubePickedUp(cubeId))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !CubeManager.Instance.IsCubePickedUp(cubeId))
        {
            PickUpCube();
        }
    }

    private void PickUpCube()
    {
        CubeManager.Instance.PickUpCube(cubeId);
        NumberSO numberSO = NumberManager.instance.GetNumber(numberValue);
        NumberInventoryManager.instance.AddNumber(numberSO);
        Debug.Log($"Picked up number: {numberValue}");
        gameObject.SetActive(false);
    }
}