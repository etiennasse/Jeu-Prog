using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public const float ZOOM_BOUNDARY = 0;
    public const float DEZOOM_BOUNDARY = 0;
    public const int BOUNDARY = 30;
    public const int RIGHT_MAX = 170;
    public const int LEFT_MAX = 25;

    [SerializeField] public float Movespeed = 30.0f;
    [SerializeField] public float Zoomspeed = 0.5f;
    [SerializeField] public float Rotation = 0.5f;
    [SerializeField] public float MaxRotation = 0.3f;
    [SerializeField] public float MinRotation = 0.1f;
    [SerializeField] public float MaxDezoom = 40f;
    [SerializeField] public float MaxZoom = 6f;

    int width;
    int height;

    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }

    void Update()
    {
        UpdateCameraLocation();
        UpdateZoom();
    }

    private void UpdateCameraLocation()
    {
        if (Input.mousePosition.x > width - BOUNDARY && transform.position.x < RIGHT_MAX)
        {
            transform.position -= new Vector3(Time.deltaTime * -Movespeed, 0.0f, 0.0f);
        }

        if (Input.mousePosition.x < 0 + BOUNDARY && transform.position.x > LEFT_MAX)
        {
            transform.position -= new Vector3(Time.deltaTime * Movespeed, 0.0f, 0.0f);
        }
    }

    private void UpdateZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > ZOOM_BOUNDARY)
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

        if (Input.GetAxis("Mouse ScrollWheel") < DEZOOM_BOUNDARY)
        {
            if (transform.position.y <= MaxDezoom)
            {
                GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + Zoomspeed, transform.position.z - Zoomspeed);
            }
            if (transform.rotation.x < MaxRotation)
            {
                transform.Rotate(Rotation, 0, 0);
            }
        }
    }
}
