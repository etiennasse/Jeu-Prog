﻿using System.Collections;
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
    public ParticleSystem ps;
    private GameObject currentBasePrefab;

    void Start () {
        health = startHealth;
        currentBasePrefab = Instantiate(Base100, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 4, gameObject.transform.position.z), Base100.transform.rotation);
    }
	
    public void TakeDamage(float amount)
    {
        health -= amount;
        imageHealth.fillAmount = health / startHealth;

        UpdateBaseState();
            print(currentBasePrefab);
            Instantiate(ps, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }

    private void UpdateBaseState()
    {
        if (this.IsAt75Percent() && currentBaseObject != Base75)
        {
            Destroy(currentBaseObject);
            currentBaseObject = Instantiate(Base75, new Vector3(10, 0, 205), Base75.transform.rotation);
        }
        else if (this.IsAt50Percent() && currentBaseObject != Base50)
        {
            Instantiate(ps, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(currentBasePrefab);
            currentBasePrefab = Instantiate(Base50, new Vector3(10, 0, 205), Base25.transform.rotation);
        }
        else if (this.IsAt25Percent() && currentBaseObject != Base25)
        {
            Instantiate(ps, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(currentBasePrefab);
            currentBasePrefab = Instantiate(Base25, new Vector3(10, 0, 205), Base0.transform.rotation);
        }
        else if (this.IsDead() && currentBaseObject != Base0)
        {
            Destroy(currentBaseObject);
            currentBaseObject = Instantiate(Base0, new Vector3(10, 0, 205), Base0.transform.rotation);
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
            SceneManager.LoadScene(3);
        }
        Destroy(gameObject, 1.7f);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public bool IsAt75Percent()
    {
        return health <= 3750 && health > 2500;
    }

    public bool IsAt50Percent()
    {
        return health <= 2500 && health > 1250;
    }

    public bool IsAt25Percent()
    {
        return health <= 1250 && health > 0;
    }
}
