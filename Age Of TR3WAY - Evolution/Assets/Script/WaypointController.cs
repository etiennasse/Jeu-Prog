using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public static Transform[] alliesWaypoints;
    public static Transform[] ennemyWaypoints;
    public static int lastWaypointIndex;
    void Awake()
    {
        alliesWaypoints = new Transform[transform.childCount];
        for (int i = 0; i < alliesWaypoints.Length; i++)
        {
            alliesWaypoints[i] = transform.GetChild(i);
        }
        ennemyWaypoints = (Transform[])alliesWaypoints.Clone();
        Array.Reverse(ennemyWaypoints);
        lastWaypointIndex = alliesWaypoints.Length - 1;
    }
}
