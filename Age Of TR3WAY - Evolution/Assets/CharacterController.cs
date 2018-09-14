using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float speed = 10f;
    private Transform target;
    private int waypointIndex;

    void Start()
    {
        target = WaypointController.waypoints[waypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= .2f)
        {
            if (waypointIndex != WaypointController.lastWaypointIndex)
                GetNextWaypoint();
            else
                Destroy(gameObject);
        }
    }

    private void GetNextWaypoint()
    {
        waypointIndex++;
        target = WaypointController.waypoints[waypointIndex];
    }
}
