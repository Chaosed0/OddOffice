using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float attackDistance;
    public float damage = 5;
    public bool melee = false;

    Health health;
    GameObject target;
    Animator anim;
    NavMeshAgent agent;
    Targeter targeter;
    
    private void Start()
    {
        health = GetComponent<Health>();
        target = GameObject.Find("Player") as GameObject;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        health.OnDied.AddListener(() => Destroy(this.gameObject));

        if (anim)
        {
            anim.Play("Moving", 0, Random.Range(0f, 1f));
        }
    }

    void Update()
    {
        if (!melee) return;

        if ((transform.position - target.transform.position).sqrMagnitude < attackDistance * attackDistance)
        {
            anim.SetBool("Attacking", true);

            if (agent)
            {
                agent.updatePosition = false;
            }
        }
        else
        {
            if (agent)
            {
                agent.updatePosition = true;
            }
        }
    }

    public void Attack()
    {
        if ((transform.position - target.transform.position).sqrMagnitude < attackDistance * attackDistance)
        {
            target.GetComponent<Health>().DealDamage(damage);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }
    }
}
