using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float speed = 5f;
    public float turnSpeed = 14f;
    private Transform target;
    private int waypointIndex;
    Animator a;

    void Start()
    {
        target = WaypointController.alliesWaypoints[waypointIndex];
        a = GetComponent<Animator>();
        a.Play("Walk");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        UpdateRotation();

        if (Vector3.Distance(transform.position, target.position) <= .2f)
        {
            if (waypointIndex != WaypointController.lastWaypointIndex)
                GetNextWaypoint();
        }
    }

    void UpdateRotation() {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.LerpUnclamped(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void GetNextWaypoint()
    {
        waypointIndex++;
        target = WaypointController.alliesWaypoints[waypointIndex];
    }
}
