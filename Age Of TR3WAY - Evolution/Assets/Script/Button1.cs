using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button1 : MonoBehaviour {

    [SerializeField]  public GameObject prefab;
    [SerializeField] GameObject spawnPoint;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    public void spawn()
    {
        Quaternion rot = Quaternion.Euler(0, 90, 0);
        Instantiate(prefab, spawnPoint.transform.position, rot);
    }
}
