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
    private LayerMask chargeLayerMask;

	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        targeter = GetComponent<Targeter>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        chargeLayerMask = ~LayerMask.NameToLayer("ChargeIgnore");

        hurtbox.enabled = false;

        StartCoroutine(CheckCanChargeCoroutine());
    }

    IEnumerator CheckCanChargeCoroutine()
    {
        yield return new WaitForSeconds(checkPathTime);
        Vector3 towardsPlayer = player.transform.position - transform.position;

        RaycastHit hitInfo;
        bool hit = Physics.BoxCast(transform.position, new Vector3(1.0f, 0.5f, 1.0f), towardsPlayer, out hitInfo, Quaternion.LookRotation(towardsPlayer));

        if (hit && hitInfo.collider.gameObject.layer == 9 && !isCharging)
        {
            towardsPlayer.y = 0.0f;
            chargeDirection = towardsPlayer.normalized;
            StartCharging();
        }
        else
        {
            StartCoroutine(CheckCanChargeCoroutine());
        }
    }

    void StartCharging()
    {
        Debug.Log("Start");
        isCharging = true;
        hurtbox.enabled = true;
        targeter.enabled = false;
        agent.enabled = false;
        rb.isKinematic = false;
        chargeDurationTimer = 0.0f;
    }

    void FixedUpdate()
    {
        if (isCharging)
        {
            rb.velocity = chargeDirection * chargeSpeed;

            chargeDurationTimer += Time.fixedDeltaTime;
            if (chargeDurationTimer > maxChargeDuration)
            {
                StopCharging();
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isCharging)
        {
            StopCharging();
        }
    }

    void StopCharging()
    {
        Debug.Log("Stop");
        isCharging = false;
        hurtbox.enabled = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(RecoveryCoroutine());
    }

    IEnumerator RecoveryCoroutine()
    {
        Debug.Log("Recover");
        yield return new WaitForSeconds(chargeRecoveryTime);
        Debug.Log("Recovered");
        targeter.enabled = true;
        agent.enabled = true;
        rb.isKinematic = true;
        StartCoroutine(CheckCanChargeCoroutine());
    }
}
