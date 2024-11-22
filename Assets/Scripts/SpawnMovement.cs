using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMovement : MonoBehaviour
{
    public float fallSpeed = 5f;
    
    private void Update()
    {
        // Move downward at a constant speed
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        
        // Destroy if falls too far
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}

