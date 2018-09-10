using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button2 : MonoBehaviour {

    [SerializeField] public GameObject prefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawn()
    {

        Quaternion rot = Quaternion.Euler(0, 90, 0);
        Vector3 pos = new Vector3(15, 5, 205);
        Instantiate(prefab, pos, rot);
    }
}
