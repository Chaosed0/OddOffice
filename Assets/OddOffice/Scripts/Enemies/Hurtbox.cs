using System.Collections;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public float damagePerSecond = 5.0f;
    public bool dealingDamage = false;
    Health playerHealth = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerHealth = other.GetComponent<Health>();
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
        if (playerHealth != null && dealingDamage)
        {
            playerHealth.DealDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
