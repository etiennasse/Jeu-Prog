using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DestroySelf", 3);
    }
	
	// Update is called once per frame
	void DestroySelf () {
        Destroy(gameObject);
	}
}
