using UnityEngine;

public class EnemMove : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2;
    private int currentWaypointIndex = 0;

    void Update()
    {
  
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);


        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
           
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}

