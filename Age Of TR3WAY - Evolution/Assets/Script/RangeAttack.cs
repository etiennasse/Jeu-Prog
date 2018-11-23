using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour {

    public GameObject target;
    private float attackDamage;
    private float speed = 10f;
	
	public void Seek(GameObject target, float attackDamage)
    {
        this.target = target;
        this.attackDamage = attackDamage;
    }
	
	void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            UpdateAttack();
        }
    }

    private void UpdateAttack()
    {
        Vector3 direction = target.transform.position - transform.position + new Vector3(0, 2f, 0f);
        float distance = speed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        transform.Translate(direction.normalized * distance, Space.World);

        if (direction.magnitude <= distance)
            HitTarget();
    }

    private void HitTarget()
    {
        if(target.tag == "EnemiesBase" || target.tag == "AlliesBase")
        {
            BaseHealth _base = target.GetComponent<BaseHealth>();
            _base.TakeDamage(this.attackDamage);
        }
        else
        {
            Character character = target.GetComponent<Character>();
            character.DealDamage(this.attackDamage);
        }
        Destroy(this.gameObject);
    }
}
