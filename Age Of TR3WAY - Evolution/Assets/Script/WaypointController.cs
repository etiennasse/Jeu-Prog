using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour {

    public static Transform[] waypoints;
    public static Transform[] ennemyWaypoints;
    public static int lastWaypointIndex;

    void Awake()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
        ennemyWaypoints = waypoints;
        Array.Reverse(ennemyWaypoints);
        lastWaypointIndex = waypoints.Length - 1;
    }
}
