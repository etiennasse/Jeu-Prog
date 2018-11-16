using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseHealth : MonoBehaviour {

    public Image imageHealth;
    public float startHealth = 5000;
    private float health;


    public GameObject Base0;
    public GameObject Base25;
    public GameObject Base50;
    public GameObject Base75;
    public GameObject Base100;
    private GameObject currentBasePrefab;

    void Start () {
        health = startHealth;
        currentBasePrefab = Instantiate(Base100, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 4, gameObject.transform.position.z), Base100.transform.rotation);
    }
	
    public void TakeDamage(float amount)
    {
        health -= amount;

        imageHealth.fillAmount = health / startHealth;

        if (health <= 3750 && health > 2500 && currentBasePrefab != Base75)
        {
            Destroy(currentBasePrefab);
            currentBasePrefab = Instantiate(Base75, new Vector3(10, 0, 205), Base75.transform.rotation);
        }
        else if (health <= 2500 && health > 1250 && currentBasePrefab != Base50)
        {
            Destroy(currentBasePrefab);
            currentBasePrefab = Instantiate(Base50, new Vector3(10, 0, 205), Base50.transform.rotation);
        }
        else if (health <= 1250 && health > 0 && currentBasePrefab != Base25)
        {
            Destroy(currentBasePrefab);
            currentBasePrefab = Instantiate(Base25, new Vector3(10, 0, 205), Base25.transform.rotation);
        }
        else if (health <= 0 && currentBasePrefab != Base0)
        {
            Destroy(currentBasePrefab);
            currentBasePrefab = Instantiate(Base0, new Vector3(10, 0, 205), Base0.transform.rotation);
        }

        if (IsDead())
        {
            Die();
        }

    }

    private void Die()
    {
        print(currentBasePrefab.tag);
        if (currentBasePrefab.tag == "EnemiesBase")
        {
            SceneManager.LoadScene(2);
        }
        else if (currentBasePrefab.tag == "AlliesBase")
        {
            SceneManager.LoadScene(2);
        }
        Destroy(gameObject, 1.7f);
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
