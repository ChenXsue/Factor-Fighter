using UnityEngine;

public class PatrolScript : MonoBehaviour
{
    public Transform[] patrolPoints; // Array to store the four patrol points
    public float speed = 2.0f;       // Movement speed
    private int currentPointIndex = 0; // Index of the current target patrol point

    private void Update()
    {
        if (patrolPoints.Length == 0)
            return;

        // Get the current target patrol point
        Transform targetPoint = patrolPoints[currentPointIndex];

        // Move the monster towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Check if the monster is close to the target point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Move to the next patrol point
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}