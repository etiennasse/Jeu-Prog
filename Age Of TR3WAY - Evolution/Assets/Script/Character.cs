using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    public float speed = 5f;
    public float turnSpeed = 14f;
    public float range = 4f;

    public float startHealth = 100f;
    protected float health;

    protected float attackDelay = 1.25f;
    protected float attackTimer = 1.25f;
    public GameObject attackObject;
    public float attackDamage = 25f;

    protected float deathTimer = 1.7f;
    protected float deathTime = 0f;

    protected Transform waypointTarget;
    protected GameObject target;
    protected Animator animator;
    public Image imageHealth;
    public AudioSource audioDie;
    public AudioSource audioAttack;

    protected int waypointIndex;
    protected bool isStopped = false;

    public ParticleSystem ps;

    protected virtual void CheckClosestCharacterDistance() { }
    protected virtual void ResolveAttack() { }
    protected virtual void GetNextWaypoint() { }

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

    protected void AttackBase()
    {
        if (CanAttack())
        {
            attackTimer = 0f;
            PerformAttackOnBase();
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    protected void PerformAttackOnBase()
    {
        BaseHealth _base = target.GetComponent<BaseHealth>();
        if (attackObject != null && !_base.IsDead())
        {
            animator.Play("Right Throw");
            GameObject rangeAttackObject = (GameObject)Instantiate(attackObject, this.transform);
            rangeAttackObject.transform.Translate(new Vector3(0, 3f, 0));
            rangeAttackObject.GetComponent<RangeAttack>().Seek(this.target, this.attackDamage);
            audioAttack.Play();
        }
        else if (!_base.IsDead())
        {
            animator.Play("Melee Right Attack 01");
            _base.TakeDamage(this.attackDamage);
            audioAttack.Play();
        }
        else
        {
            attackTimer = 1.25f;
            target = null;
        }
    }

    protected void AttackEnnemy(Character ennemy)
    {
        if (CanAttack())
        {
            attackTimer = 0f;
            PerformAttackOnEnnemy(ennemy);
        }
        else
        {
            attackTimer += Time.deltaTime;
        }

        if (ennemy.IsAlive())
            UpdateRotation(ennemy.transform);
    }

    protected void PerformAttackOnEnnemy(Character ennemy)
    {
        if (attackObject != null && ennemy.IsAlive())
        {
            animator.Play("Right Throw");
            GameObject rangeAttackObject = (GameObject)Instantiate(attackObject, this.transform);
            rangeAttackObject.transform.Translate(new Vector3(0, 3f, 0));
            rangeAttackObject.GetComponent<RangeAttack>().Seek(this.target, this.attackDamage);
            audioAttack.Play();
        }
        else if (ennemy.IsAlive())
        {
            animator.Play("Melee Right Attack 01");
            ennemy.DealDamage(this.attackDamage);
            audioAttack.Play();
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
        imageHealth.fillAmount = health / startHealth;
        if (!IsAlive())
        {
            this.Die();
        }
    }

    public virtual void UpdateDeath()
    {
        if (deathTime <= deathTimer)
        {
            deathTime += Time.deltaTime;
        }
        else
        {
            Instantiate(ps, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    protected void Die()
    {
        animator.Play("Die");
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

    protected void AddTargetIfInRange(GameObject character, float distance)
    {
        if (distance <= range)
        {
            target = character;
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

    void UpdateRotation(Transform rotateTarget)
    {
        Vector3 direction = rotateTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.LerpUnclamped(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    protected bool HasTarget()
    {
        return this.target != null;
    }

    protected bool CanAttack()
    {
        return attackTimer >= attackDelay;
    }

    public bool IsAlive()
    {
        return this.health > 0;
    }
}
