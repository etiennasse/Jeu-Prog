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
	
	void Update ()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 direction = target.transform.position - transform.position + new Vector3(0, 3f, 0);
        float distance = speed * Time.deltaTime;

        if (direction.magnitude <= distance)
        {
            HitTarget();
            return;
        }

        transform.Rotate(new Vector3(15f, 15f, 0));
        transform.Translate(direction.normalized * distance, Space.World);
    }

    private void HitTarget()
    {
        EnemyController enemy = target.GetComponent<EnemyController>();
        enemy.DealDamage(this.attackDamage);
        Destroy(this.gameObject);
    }
}
