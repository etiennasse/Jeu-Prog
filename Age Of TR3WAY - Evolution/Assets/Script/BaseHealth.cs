using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour {

    public Image imageHealth;
    public float startHealth = 5000;
    private float health;

    void Start () {
        health = startHealth;
    }
	
    public void TakeDamage(float amount)
    {
        health -= amount;

        imageHealth.fillAmount = health / startHealth;

        if (IsDead())
        {
            Die();
        }

    }

    private void Die()
    {
        Destroy(gameObject, 1.7f);
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
