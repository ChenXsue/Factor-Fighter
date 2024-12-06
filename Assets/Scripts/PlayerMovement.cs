using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    public float movementSpeed;
    private float speedX, speedY;
    private Rigidbody2D rb;
    private RoomManager roomManager;
    public bool isKnockedBack = false;
    private bool canMove = false;  // 新添加：控制是否可以移动
    [SerializeField] private float moveDelay = 5.3f;  // 与计时器相同的延迟时间

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        roomManager = FindObjectOfType<RoomManager>();
        
        // 设置初始位置
        if (roomManager != null && roomManager.defaultSpawnPoint != null)
        {
            transform.position = roomManager.defaultSpawnPoint.position;
        }

        // 启动延迟移动协程
        StartCoroutine(DelayedMovement());
    }

    IEnumerator DelayedMovement()
    {
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    void Update()
    {
        if (!isKnockedBack && canMove) // 只有在可以移动且不处于击退状态时才允许玩家控制
        {
            speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
            speedY = Input.GetAxisRaw("Vertical") * movementSpeed;
            rb.velocity = new Vector2(speedX, speedY);
        }
        else if (!canMove && !isKnockedBack) // 如果还不能移动且不在击退状态，确保速度为0
        {
            rb.velocity = Vector2.zero;
        }
    }

    public IEnumerator ApplyKnockback(Vector2 knockbackDirection, float knockbackForce, float knockbackDuration)
    {
        isKnockedBack = true;
        
        // 设置玩家的 Rigidbody2D 速度为击退方向和力度
        rb.velocity = knockbackDirection * knockbackForce;
        
        // 等待击退时间
        yield return new WaitForSeconds(knockbackDuration);
        
        // 恢复玩家控制
        isKnockedBack = false;
    }
}