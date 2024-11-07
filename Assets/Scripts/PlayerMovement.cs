using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    private RoomManager roomManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        roomManager = FindObjectOfType<RoomManager>();
        
        // 设置初始位置
        if (roomManager != null && roomManager.defaultSpawnPoint != null)
        {
            transform.position = roomManager.defaultSpawnPoint.position;
        }
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movementSpeed;
        rb.velocity = new Vector2(speedX, speedY);
    }
}

// public class PlayerMovement : MonoBehaviour
// {
//     public float movementSpeed;
//     float speedX, speedY;
//     Rigidbody2D rb;

//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         if (RoomManager.Instance.isFirstSpawn)
//         {
//             Debug.Log("Game instance is first spawn");
//             RoomManager.Instance.isFirstSpawn = false;
//         }
//         else
//         {
//             // Debug.Log("Player spawn position: " + RoomManager.Instance.playerSpawnPosition);
//             transform.position = RoomManager.Instance.playerSpawnPosition;
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
//         speedY = Input.GetAxisRaw("Vertical") * movementSpeed;
//         rb.velocity = new Vector2(speedX, speedY);
//     }
// }