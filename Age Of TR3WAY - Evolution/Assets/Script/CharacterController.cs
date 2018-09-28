using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float speed = 5f;
    public float turnSpeed = 14f;


    private Transform waypointTarget;
    private GameObject enemyTarget;
    private int waypointIndex;
    private bool isStopped = false;
    public static string tagName = "Allies";
    Animator animator;

    void Start()
    {
        waypointTarget = WaypointController.alliesWaypoints[waypointIndex];
        animator = GetComponent<Animator>();
        animator.Play("Walk");
    }

    // Update is called once per frame
    void Update()
    {
        if (HasEnnemyTarget())
        {
            //TODO: Attack target
        }
        else if(!isStopped)
        {
            UpdateMovement();
        }
        else if(isStopped)
        {
            //TODO: Add check if object can move
        }
    }

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

        /*if (closestCharacter != null && closestCharacter != gameObject)
        {
            float distanceToCharacter = Vector3.Distance(transform.position, closestCharacter.transform.position);

            if (distanceToCharacter <= 4f)
            {
                isStopped = true;
            }
        }*/
    }

    public GameObject GetClosestCharacter()
    {
        GameObject[] gameCharacters = GetGameCharacters();
        Debug.Log(gameCharacters.Length);
        GameObject closestCharacter = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject character in gameCharacters)
        {
            float distanceToCharacter = Vector3.Distance(transform.position, character.transform.position);
            if (distanceToCharacter < closestDistance)
            {
                closestDistance = distanceToCharacter;
                closestCharacter = character;
            }
        }

        return closestCharacter;
    }

    public GameObject[] GetGameCharacters()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyController.tagName);
        GameObject[] allies = GameObject.FindGameObjectsWithTag(tagName);
        GameObject[] gameCharaters = new GameObject[enemies.Length + allies.Length];
        Array.Copy(enemies, gameCharaters, enemies.Length);
        if (enemies.Length - allies.Length >= 0)
        {
            Array.Copy(allies, gameCharaters, enemies.Length - allies.Length);
        }
        return (GameObject[])gameCharaters.Clone();
    }

    void UpdateRotation() {
        Vector3 direction = waypointTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.LerpUnclamped(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void GetNextWaypoint()
    {
        waypointIndex++;
        waypointTarget = WaypointController.alliesWaypoints[waypointIndex];
    }

    private bool HasEnnemyTarget()
    {
        return enemyTarget != null;
    }

    private void DeleteEnemyTarget()
    {
        enemyTarget = null;
        isStopped = false;
    }
}
