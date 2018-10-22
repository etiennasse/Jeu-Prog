using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed = 5f;
    public float turnSpeed = 14f;
    private Transform waypointTarget;
    private GameObject target = null;
    private int waypointIndex;
    Animator animator;
    public static string tagName = "Enemy";
    private bool isStopped = false;


    void Start()
    {
        waypointTarget = WaypointController.ennemyWaypoints[waypointIndex];
        animator = GetComponent<Animator>();
        animator.Play("Walk");
    }

    void Update()
    {
        if (HasTarget())
        {
            AttackTarget();
        }
        else
        {
            UpdateMovement();
        }
    }

    void AttackTarget()
    {
        //TODO
    }

    // Update is called once per frame
    void UpdateMovement()
    {
        CheckClosestCharacterDistance();
        if (!isStopped)
        {
            PerformMovementUpdate();
        }
    }

    void PerformMovementUpdate()
    {
        Vector3 direction = waypointTarget.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        UpdateRotation();

        if (Vector3.Distance(transform.position, waypointTarget.position) <= .2f)
        {
            if (waypointIndex != WaypointController.lastWaypointIndex)
                GetNextWaypoint();
        }
    }

    private void CheckClosestCharacterDistance()
    {
        GameObject closestCharacterInFront = GetClosestCharacterInFront();

        if (closestCharacterInFront)
        {
            float distanceToCharacter = Mathf.Abs(Vector3.Distance(transform.position, closestCharacterInFront.transform.position));

            if (distanceToCharacter <= 4f)
            {
                StopMovement();
            }
            else if (distanceToCharacter > 4.5f && isStopped)
            {
                StartMovement();
            }
        }
        else if(!closestCharacterInFront && isStopped)
        {
            StartMovement();
        }
    }

    public void StopMovement()
    {
        isStopped = true;
        animator.Play("Idle");
    }

    public void StartMovement()
    {
        isStopped = false;
        animator.Play("Walk");
    }

    public GameObject GetClosestCharacterInFront()
    {
        List<GameObject> gameCharacters = GetGameCharacters();
        GameObject closestCharacter = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject character in gameCharacters)
        {
            if (character != null)
            {
                if (character != this.gameObject)
                {
                    float distanceToCharacter = Mathf.Abs(Vector3.Distance(transform.position, character.transform.position));
                    bool isBehind = transform.position.x < character.transform.position.x;
                    if (distanceToCharacter < closestDistance && !isBehind)
                    {
                        closestDistance = distanceToCharacter;
                        closestCharacter = character;
                    }
                }
            }
        }
        return closestCharacter;
    }

    void UpdateRotation()
    {
        Vector3 direction = waypointTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.LerpUnclamped(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    public List<GameObject> GetGameCharacters()
    {
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(CharacterController.tagName));
        List<GameObject> allies = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagName)); ;
        List<GameObject> gameCharaters = new List<GameObject>();
        gameCharaters.AddRange(enemies);
        gameCharaters.AddRange(allies);
        return gameCharaters;
    }

    public GameObject GetFirstCharacterOfTheRow()
    {
        return GameObject.FindGameObjectsWithTag(tagName)[0];
    }

    private void GetNextWaypoint()
    {
        waypointIndex++;
        waypointTarget = WaypointController.ennemyWaypoints[waypointIndex];
    }

    private bool HasTarget()
    {
        return target != null;
    }
}
