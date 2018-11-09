using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBaseHealth : MonoBehaviour {

    public float startHealth = 5000;
    private float health;

    void Start()
    {
        health = startHealth;
    }


    public void TakeDamage(float amount)
    {
        health -= amount;


        if (IsDead())
        {
            Die();
        }

    }

    public void Die()
    {
        Destroy(gameObject, 1.7f);
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
