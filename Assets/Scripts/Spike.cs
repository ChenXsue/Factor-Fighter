using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damageAmount = 1;
    public float knockbackForce = 20f;
    public float knockbackDuration = 0.5f; // 击退时间

    private HealthManager healthManager;

    private void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && healthManager != null && !healthManager.isGameOver)
        {
            Debug.Log("Triggered by player, applying damage and knockback.");

            // 对玩家造成伤害
            healthManager.TakeDamage(damageAmount);

            // 施加击退效果并禁用玩家控制
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                StartCoroutine(ApplyKnockback(playerMovement, collision.GetComponent<Rigidbody2D>()));
            }
        }
    }

    private IEnumerator ApplyKnockback(PlayerMovement player, Rigidbody2D rb)
    {
        player.isKnockedBack = true;

        // 计算并施加击退力
        Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = knockbackDirection * knockbackForce;

        // 等待击退持续时间
        yield return new WaitForSeconds(knockbackDuration);

        // 恢复玩家控制
        player.isKnockedBack = false;
    }
}
