using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float speed = 5f;
    private Transform target;
    private int waypointIndex;
    Animator a;

    void Start()
    {
        target = WaypointController.waypoints[waypointIndex];
        a = GetComponent<Animator>();
        a.Play("Walk");
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
        }
    }

    private void GetNextWaypoint()
    {
        waypointIndex++;
        target = WaypointController.waypoints[waypointIndex];
    }
}
