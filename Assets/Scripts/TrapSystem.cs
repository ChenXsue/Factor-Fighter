using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapSystem : MonoBehaviour 
{
    [Header("Trap Settings")]
    public List<GameObject> spikeTraps = new List<GameObject>();
    public GameObject equationBox;        // 方程框对象（黄色半透明方块）
    public TextMeshPro equationText;      // 显示方程的文本
    public float detectionRange = 2f;     // 检测范围
    public int expectedResult = 4;        // 期望的结果（比如8/2=4）
    public GameObject space; 

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;     // 击退力的大小
    public float knockbackDuration = 0.3f;// 击退效果的持续时间
    public GameObject InputManagerObject;

    private Transform player;
    private bool isInRange = false;
    private bool isSolved = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        equationText.enabled = false;
        space.SetActive(false);

        // 查找所有尖刺
        GameObject[] spikes = GameObject.FindGameObjectsWithTag("Spike");
        foreach (GameObject spike in spikes)
        {
            spikeTraps.Add(spike);
        }
    }

    void Update()
    {
        if (isSolved) return;

        // 直接使用equationBox的位置计算距离
        float distanceToPlayer = Vector2.Distance((Vector2)equationBox.transform.position, (Vector2)player.position);
        
        // 根据距离显示或隐藏方程
        if (distanceToPlayer <= detectionRange)
        {
            if (!isInRange)
            {
                isInRange = true;
                equationText.enabled = true;
                space.SetActive(true);
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                equationText.enabled = false;
                space.SetActive(false);
            }
        }
    }

    public bool TryPlaceNumber(int number)
    {
        if (!isInRange || isSolved) return false;

        if (number == expectedResult)
        {
            SolvePuzzle();
            WebGLDataLogger.answerSum++;
            return true;
        }
        else{
            WebGLDataLogger.answerSum++;
            WebGLDataLogger.wrongNum++;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Triggered by player, applying damage and knockback.");
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

    void SolvePuzzle()
    {
        isSolved = true;
        
        // 销毁所有相关的尖刺
        // foreach (GameObject spike in spikeTraps)
        // {
        //     if (spike != null)
        //     {
        //         Destroy(spike);
        //     }
        // }
        // spikeTraps.Clear();
        
        // 销毁方程框
        Destroy(equationBox);
        Destroy(gameObject);
        WebGLDataLogger.numberSum ++;

        InputManager inputManager = InputManagerObject.GetComponent<InputManager>();
        inputManager.TrapSolved();

        /*
        NumberSO numberSO = NumberManager.instance.GetNumber(expectedResult);
        NumberInventoryManager.instance.AddNumber(numberSO);
        */
    }

    // // 在场景视图中显示检测范围
    // void OnDrawGizmos()
    // {
    //     if (equationBox != null)
    //     {
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawWireSphere(equationBox.transform.position, detectionRange);
    //     }
    // }
}