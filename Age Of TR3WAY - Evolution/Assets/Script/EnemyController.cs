using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public float speed = 5f;
    public float turnSpeed = 14f;
    public float range = 4f;

    public const float START_HEALTH = 100f;
    private float health;

    private float attackDelay = 1.25f;
    private float attackTimer = 1.25f;
    public GameObject attackObject;
    public float attackDamage = 25f;

    private float deathTimer = 1.7f;
    private float deathTime = 0f;

    private Transform waypointTarget;
    private GameObject target;
    private Animator animator;
    public Image imageHealth;
    public AudioSource audioDie;
    public AudioSource audioAttack;

    private int waypointIndex;
    private bool isStopped = false;

    public static string tagName = "Enemy";
    public const string BASE_NAME = "EnemiesBase";

    public ParticleSystem ps;
    public int moneyReward = 1000;


    void Start()
    {
        waypointTarget = WaypointController.ennemyWaypoints[waypointIndex];
        animator = GetComponent<Animator>();
        animator.Play("Walk");
        health = START_HEALTH;
    }

    void Update()
    {
        if (HasTarget() && IsAlive())
        {
            ResolveAttack();
        }
        else if (!IsAlive())
        {
            UpdateDeath();
        }
        else
        {
            UpdateMovement();
        }
    }

    private void ResolveAttack()
    {
        if (target.tag == "Allies")
        {
            CharacterController ennemy = target.GetComponent<CharacterController>();
            AttackEnnemy(ennemy);
        }
        else if (target.tag == "AlliesBase")
        {
            AttackBase();
        }
    }

    private void AttackBase()
    {
        if (CanAttack())
        {
            attackTimer = 0f;
            PerformAttackOnBase();
            audioAttack.Play();
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    private void PerformAttackOnBase()
    {
        BaseHealth _base = target.GetComponent<BaseHealth>();
        if (attackObject != null && !_base.IsDead())
        {
            animator.Play("Right Throw");
            GameObject rangeAttackObject = (GameObject)Instantiate(attackObject, this.transform);
            rangeAttackObject.transform.Translate(new Vector3(0, 3f, 0));
            rangeAttackObject.GetComponent<RangeAttack>().Seek(this.target, this.attackDamage);
        }
        else if (!_base.IsDead())
        {
            animator.Play("Melee Right Attack 01");
            _base.TakeDamage(this.attackDamage);
        }
        else
        {
            attackTimer = 1.25f;
            target = null;
        }
    }

    private void AttackEnnemy(CharacterController ennemy)
    {
        if (CanAttack())
        {
            attackTimer = 0f;
            PerformAttackOnEnnemy(ennemy);
            audioAttack.Play();
        }
        else
        {
            attackTimer += Time.deltaTime;
        }

        if (ennemy.IsAlive())
            UpdateRotation(ennemy.transform);
    }

    private void PerformAttackOnEnnemy(CharacterController ennemy)
    {
        if (attackObject != null && !ennemy.IsAlive())
        {
            animator.Play("Right Throw");
            GameObject rangeAttackObject = (GameObject)Instantiate(attackObject, this.transform);
            rangeAttackObject.transform.Translate(new Vector3(0, 2f, 0f));
            rangeAttackObject.GetComponent<RangeAttack>().Seek(this.target, this.attackDamage);
        }
        else if (ennemy.IsAlive())
        {
            animator.Play("Melee Right Attack 01");
            ennemy.DealDamage(this.attackDamage);
        }
        else
        {
            attackTimer = 1.25f;
            target = null;
        }
    }

    public void DealDamage(float damage)
    {
        this.health -= damage;
        imageHealth.fillAmount = health / START_HEALTH;
        if (!IsAlive())
        {
            this.Die();
        }
    }

    public void UpdateDeath()
    {
        if (deathTime <= deathTimer)
        {
            deathTime += Time.deltaTime;
        }
        else
        {
            Instantiate(ps, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void Die()
    {
        animator.Play("Die");
        GameController.AddMoney(this.moneyReward);
        if (!audioDie.isPlaying)
        {
            audioDie.Play();
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

        UpdateRotation(waypointTarget.transform);

        if (Vector3.Distance(transform.position, waypointTarget.position) <= .2f)
        {
            if (waypointIndex != WaypointController.lastWaypointIndex)
                GetNextWaypoint();
        }
    }

    private void AddTargetIfInRange(GameObject character, float distance)
    {
        if (distance <= range)
        {
            target = character;
        }
    }

    private void CheckClosestCharacterDistance()
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

    public GameObject GetClosestTargetInFront()
    {
        List<GameObject> gameCharacters = GetGameCharacters();
        GameObject closestCharacter = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject character in gameCharacters)
        {
            if (character != null)
            {
                if (character != this.gameObject && character.tag != tagName && character.tag != BASE_NAME)
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
                if (character != this.gameObject && character.tag != BASE_NAME)
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

    void UpdateRotation(Transform rotateTarget)
    {
        Vector3 direction = rotateTarget.position - transform.position;
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
        gameCharaters.Add(GameObject.FindGameObjectWithTag("EnemiesBase"));
        gameCharaters.Add(GameObject.FindGameObjectWithTag("AlliesBase"));
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

    public bool IsAlive()
    {
        return this.health > 0;
    }

    private bool CanAttack()
    {
        return attackTimer >= attackDelay;
    }
}
