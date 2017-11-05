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
    public LayerMask ignoreLayer;

	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        targeter = GetComponent<Targeter>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        hurtbox.enabled = false;

        StartCoroutine(CheckCanChargeCoroutine());
    }

    IEnumerator CheckCanChargeCoroutine()
    {
        yield return new WaitForSeconds(checkPathTime);
        Vector3 towardsPlayer = player.transform.position - transform.position;
        towardsPlayer.y = 0.0f;

        RaycastHit hitInfo;
        //bool hit = Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), towardsPlayer, out hitInfo, Quaternion.LookRotation(towardsPlayer), ignoreLayer.value);
        bool hit = Physics.Raycast(transform.position,  towardsPlayer, out hitInfo, Mathf.Infinity, ignoreLayer.value);

        if (hit && hitInfo.collider.gameObject.layer == 9 && !isCharging)
        {
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
        rb.isKinematic = true;
        StartCoroutine(CheckCanChargeCoroutine());
    }
}
