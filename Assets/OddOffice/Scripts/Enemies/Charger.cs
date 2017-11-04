using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger : MonoBehaviour
{
    public float chargeSpeed = 10.0f;
    public float checkPathTime = 1.0f;
    public float maxChargeDuration = 4.0f;
    public float chargeRecoveryTime = 2.0f;
    public Hurtbox hurtbox;

    private GameObject player;
    private Targeter targeter;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isCharging = false;
    private Vector3 chargeDirection = Vector3.zero;
    private float chargeDurationTimer = 0.0f;

	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        targeter = GetComponent<Targeter>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        hurtbox.enabled = false;
    }

    IEnumerator CheckCanChargeCoroutine()
    {
        yield return new WaitForSeconds(checkPathTime);
        bool canCharge = Physics.BoxCast(transform.position, new Vector3(1.0f, 1.0f, 1.0f), player.transform.position - transform.position);
        chargeDirection = (player.transform.position - transform.position).normalized;

        if (canCharge && !isCharging)
        {
            StartCharging();
        }
    }

    void StartCharging()
    {
        isCharging = true;
        hurtbox.enabled = true;
        targeter.enabled = false;
        agent.enabled = false;
        StartCoroutine(ChargingCoroutine());
    }

    IEnumerator ChargingCoroutine()
    {
        while (true)
        {
            if (!isCharging)
            {
                break;
            }

            rb.velocity = chargeDirection * chargeSpeed;
            yield return new WaitForEndOfFrame();

            chargeDurationTimer += Time.deltaTime;
            if (chargeDurationTimer > maxChargeDuration)
            {
                StopCharging();
                break;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        StopCharging();
    }

    void StopCharging()
    {
        isCharging = false;
        hurtbox.enabled = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(RecoveryCoroutine());
    }

    IEnumerator RecoveryCoroutine()
    {
        yield return new WaitForSeconds(chargeRecoveryTime);
        targeter.enabled = true;
        agent.enabled = true;
    }
}
