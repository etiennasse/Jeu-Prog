using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    public Camera CamToLook;

    void Start()
    {
        //transform.Rotate( 180,0,0 );
    }

    void Update()
    {
        Vector3 v = CamToLook.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(CamToLook.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
