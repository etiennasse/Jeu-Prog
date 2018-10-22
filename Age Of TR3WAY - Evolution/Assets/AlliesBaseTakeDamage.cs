using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlliesBaseTakeDamage : MonoBehaviour {

    public Image imageHealth;
    public float startHealth = 5000;
    private float health;
    public float waitTime = 1f;
    float timer;
    // Use this for initialization
    void Start () {
        health = startHealth;
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            TakeDamage(1f);
            timer = 0f;
        }
    }


    public void TakeDamage(float amount)
    {
        health -= 250f;

        imageHealth.fillAmount = health / startHealth;

        if (CharacterIsDead())
        {
            Die();
        }

    }

    public void Die()
    {
        Destroy(gameObject, 1.7f);
    }

    public bool CharacterIsDead()
    {
        return health <= 0;
    }
}
