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

            // 施加击退效果
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerMovement.StartCoroutine(playerMovement.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration));
            }
        }
    }
}
