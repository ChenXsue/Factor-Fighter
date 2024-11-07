using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnWayPoints1 : MonoBehaviour
{
    public List<GameObject> waypoints;
    public float speed = 4;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
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
