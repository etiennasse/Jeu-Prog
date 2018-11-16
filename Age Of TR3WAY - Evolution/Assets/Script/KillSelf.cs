using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour {
    public const int TIME_BEFORE_DESTROY = 3;
	
	void Start () {
        Invoke("DestroySelf", TIME_BEFORE_DESTROY);
    }
	
	void DestroySelf () {
        Destroy(gameObject);
	}
}
