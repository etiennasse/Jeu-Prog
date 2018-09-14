using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    [SerializeField] public float Movespeed = 30.0f;
    [SerializeField] public float Zoomspeed = 0.5f;
    [SerializeField] public float Rotation = 0.5f;
    [SerializeField] public float MaxRotation = 0.3f;
    [SerializeField] public float MinRotation = 0.1f;
    [SerializeField] public float MaxDezoom = 40f;
    [SerializeField] public float MaxZoom = 6f;

    int boundary = 30;
    int width;
    int height;

    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }

    void Update()
    {
            if (Input.mousePosition.x > width - boundary && transform.position.x < 170)
            {
                transform.position -= new Vector3(Time.deltaTime * -Movespeed, 0.0f, 0.0f);
            }

            if (Input.mousePosition.x < 0 + boundary && transform.position.x > 25)
            {
                transform.position -= new Vector3(Time.deltaTime * Movespeed, 0.0f, 0.0f);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Zoom
            {
                if (transform.position.y >= MaxZoom)
                {
                    GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - Zoomspeed, transform.position.z + Zoomspeed);
                }
                if (transform.rotation.x > MinRotation && transform.position.y <= 30)
                {
                    transform.Rotate(-Rotation, 0, 0);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0) //Dezoom
            {
            if (transform.position.y <= MaxDezoom)
            {
                GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + Zoomspeed, transform.position.z - Zoomspeed);
            }
            if (transform.rotation.x < MaxRotation )
                {
                    transform.Rotate(Rotation, 0, 0);
                }
            }
    }
}
