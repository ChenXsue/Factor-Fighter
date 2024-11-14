using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private float speedX, speedY;
    private Rigidbody2D rb;
    private RoomManager roomManager;
    public bool isKnockedBack = false;  // 用于控制击退状态

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
        if (!isKnockedBack) // 如果不处于击退状态，允许玩家控制
        {
            speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
            speedY = Input.GetAxisRaw("Vertical") * movementSpeed;
            rb.velocity = new Vector2(speedX, speedY);
        }
    }
}
