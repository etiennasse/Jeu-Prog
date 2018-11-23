using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    public static string tagName = "Enemy";
    public static string baseName = "EnemiesBase";

    public int rewardValue = 1000;

    void Start()
    {
        waypointTarget = WaypointController.ennemyWaypoints[waypointIndex];
        animator = GetComponent<Animator>();
        animator.Play("Walk");
        health = startHealth;
    }

    protected override void ResolveAttack()
    {
        if (target.tag != tagName && !target.tag.Contains("Base"))
        {
            Ally ennemy = target.GetComponent<Ally>();
            AttackEnnemy(ennemy);
        }
        else if (target.tag != baseName)
        {

            AttackBase();
        }
    }

    protected override void CheckClosestCharacterDistance()
    {
        GameObject closestCharacterInFront = GetClosestCharacterInFront();
        GameObject closestTargetInFront = GetClosestTargetInFront();
        float distanceToClosestTargetInFront = 0f;

        if (closestCharacterInFront)
        {
            float distanceToCharacter = Mathf.Abs(Vector3.Distance(transform.position, closestCharacterInFront.transform.position));
            if (closestTargetInFront)
                distanceToClosestTargetInFront = Mathf.Abs(Vector3.Distance(transform.position, closestTargetInFront.transform.position));

            if (distanceToCharacter <= 4f)
            {
                if (closestTargetInFront)
                {
                    if (closestTargetInFront.tag != tagName)
                        AddTargetIfInRange(closestTargetInFront, distanceToClosestTargetInFront);
                }
                StopMovement();
            }
            else if (distanceToCharacter > 4.5f && isStopped)
            {
                StartMovement();
            }
        }
        else if (!closestCharacterInFront && isStopped)
        {
            StartMovement();
        }
    }

    public GameObject GetClosestTargetInFront()
    {
        List<GameObject> gameCharacters = GetGameCharacters();
        GameObject closestCharacter = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject character in gameCharacters)
        {
            if (character != null)
            {
                if (character != this.gameObject && character.tag != tagName && character.tag != baseName)
                {
                    float distanceToCharacter = Vector3.Distance(transform.position, character.transform.position);
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

    public GameObject GetClosestCharacterInFront()
    {
        List<GameObject> gameCharacters = GetGameCharacters();
        GameObject closestCharacter = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject character in gameCharacters)
        {
            if (character != null)
            {
                if (character != this.gameObject && character.tag != baseName)
                {
                    float distanceToCharacter = Vector3.Distance(transform.position, character.transform.position);
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

    public List<GameObject> GetGameCharacters()
    {
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagName));
        List<GameObject> allies = new List<GameObject>(GameObject.FindGameObjectsWithTag(Ally.tagName));
        List<GameObject> gameCharaters = new List<GameObject>();
        gameCharaters.AddRange(enemies);
        gameCharaters.AddRange(allies);
        gameCharaters.Add(GameObject.FindGameObjectWithTag(baseName));
        gameCharaters.Add(GameObject.FindGameObjectWithTag(Ally.baseName));
        return gameCharaters;
    }

    protected override void GetNextWaypoint()
    {
        waypointIndex++;
        waypointTarget = WaypointController.ennemyWaypoints[waypointIndex];
    }

    public override void UpdateDeath()
    {
        if (deathTime <= deathTimer)
        {
            deathTime += Time.deltaTime;
        }
        else
        {
            GameController.AddMoney(rewardValue);
            Instantiate(ps, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
