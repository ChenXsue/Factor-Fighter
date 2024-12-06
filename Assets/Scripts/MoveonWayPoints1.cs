using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnWayPoints1 : MonoBehaviour 
{
    public List<GameObject> waypoints;
    public float speed = 4;
    public float startDelay = 5.3f;  // 添加延迟启动时间
    
    private int index = 0;
    private bool canMove = false;  // 控制是否可以移动
    
    void Start()
    {
        // 启动协程来延迟移动
        StartCoroutine(DelayedStart());
    }
    
    IEnumerator DelayedStart()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(startDelay);
        
        // 延迟结束后允许移动
        canMove = true;
    }
    
    void Update()
    {
        // 只有在可以移动的状态下才执行移动逻辑
        if (canMove)
        {
            Vector3 destination = waypoints[index].transform.position;
            Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            transform.position = newPos;
            
            float distance = Vector3.Distance(transform.position, destination);
            if(distance <= 0.05)
            {
                index = (index + 1) % waypoints.Count;
            }
        }
    }
}