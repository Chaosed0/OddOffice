using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    public float damagePerSecond = 5.0f;
    public float damageOnHit = 0.0f;
    Health playerHealth = null;

    public UnityEvent OnHurtSomething = new UnityEvent();

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerHealth = other.GetComponent<Health>();
            if (enabled)
            {
                playerHealth.DealDamage(damageOnHit);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerHealth = null;
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            playerHealth.DealDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
