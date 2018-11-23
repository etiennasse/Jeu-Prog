using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryAnimationStart : MonoBehaviour {

    private Animator animator;
    double TimeBeforeAnimation;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        animator.SetBool("Victory", true);
        TimeBeforeAnimation = Random.Range(0, 2);
    }
	
	// Update is called once per frame
	void Update () {
        TimeBeforeAnimation -= Time.deltaTime;
        if (TimeBeforeAnimation <= 0)
        {
            animator.SetTrigger("VictoryTrigger");
            TimeBeforeAnimation = Random.Range(0f, 4f);
        }

    }
}
