using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float speed = 5f;
    public float turnSpeed = 14f;
    private Transform target;
    private int waypointIndex;
    Animator a;
    public Image imageHealth;
    public float startHealth = 100;
    private float health;
    public float waitTime = 1f;
    float timer;

    void Start()
    {
        target = WaypointController.alliesWaypoints[waypointIndex];
        a = GetComponent<Animator>();
        a.Play("Walk");
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CharacterIsDead())
        {
            Vector3 direction = target.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

            UpdateRotation();

            if (Vector3.Distance(transform.position, target.position) <= .2f)
            {
                if (waypointIndex != WaypointController.lastWaypointIndex)
                    GetNextWaypoint();
            }

            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                TakeDamage(1f);
                timer = 0f;
            }
        }
    }

    void UpdateRotation() {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.LerpUnclamped(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void GetNextWaypoint()
    {
        waypointIndex++;
        target = WaypointController.alliesWaypoints[waypointIndex];
    }

    public void TakeDamage(float amount)
    {
        health -= 25f;

        imageHealth.fillAmount = health / startHealth;

        if (CharacterIsDead())
        {
            Die();
        }

    }

    public void Die()
    {
        a.Play("Die");
        Destroy(gameObject, 1.7f);
    }

    public bool CharacterIsDead()
    {
        return health <= 0;
    }
}
