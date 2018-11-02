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

        Vector3 direction = target.transform.position - transform.position + new Vector3(0, 2f, 0f);
        float distance = speed * Time.deltaTime;

        if (direction.magnitude <= distance)
        {
            HitTarget();
            return;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        transform.Translate(direction.normalized * distance, Space.World);
    }

    private void HitTarget()
    {
        if(target.tag == EnemyController.tagName) {
            EnemyController enemy = target.GetComponent<EnemyController>();
            enemy.DealDamage(this.attackDamage);
        }
        else
        {
            CharacterController allie = target.GetComponent<CharacterController>();
            allie.DealDamage(this.attackDamage);
        }
        Destroy(this.gameObject);
    }
}
