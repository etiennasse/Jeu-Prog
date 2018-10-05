using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed = 5f;
    public float turnSpeed = 14f;
    private Transform waypointTarget;
    private GameObject allieTarget = null;
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
        if (HasEnnemyTarget())
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
        CheckIfTargetIsDead();
    }

    void CheckIfTargetIsDead()
    {
        if (allieTarget == null)
        {
            MakeEnemyMove();
        }
    }

    void MakeEnemyMove()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tagName);
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().StartMovement();
        }
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
        GameObject closestCharacter = GetClosestCharacter();

        if (closestCharacter)
        {
            float distanceToCharacter = Mathf.Abs(Vector3.Distance(transform.position, closestCharacter.transform.position));

            if (distanceToCharacter <= 4f)
            {
                if (IsFirstOfTheRow(gameObject))
                {
                    if (closestCharacter.tag == tagName)
                    {
                        allieTarget = closestCharacter;
                    }
                }
                StopMovement();
            }
            else if (distanceToCharacter > 4f && !isStopped)
            {
                StartMovement();
            }
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

    public GameObject GetClosestCharacter()
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
                    if (distanceToCharacter < closestDistance)
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

    private bool HasEnnemyTarget()
    {
        return allieTarget != null;
    }

    public bool IsFirstOfTheRow(GameObject character)
    {
        return character == GetFirstCharacterOfTheRow();
    }

    private void DeleteEnemyTarget()
    {
        allieTarget = null;
        isStopped = false;
    }
}
