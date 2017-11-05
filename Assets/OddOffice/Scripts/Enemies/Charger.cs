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

    public AudioSource audioSource;
    public AudioClip chargingClip;

    private GameObject player;
    private Targeter targeter;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isCharging = false;
    private Vector3 chargeDirection = Vector3.zero;
    private float chargeDurationTimer = 0.0f;
    public LayerMask ignoreLayer;
    private Animator anim;

	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        targeter = GetComponent<Targeter>();
        agent = GetComponentInChildren<NavMeshAgent>();
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
        anim.SetTrigger("ChargePrepare");
        audioSource.clip = chargingClip;
        audioSource.Play();
    }

    public void DoCharge()
    {
        isCharging = true;
        hurtbox.enabled = true;
        targeter.enabled = false;
        agent.enabled = false;
        rb.isKinematic = false;
        chargeDurationTimer = 0.0f;
        anim.SetTrigger("Charge");
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
        audioSource.Stop();
        PrepareRecovery();
    }

    void PrepareRecovery()
    {
        anim.SetTrigger("RecoverPrepare");
        rb.isKinematic = true;
    }

    void DoRecoveryAnimation()
    {
        anim.SetTrigger("Recover");
    }

    void ConcludeRecovery()
    {
        StartCoroutine(CheckCanChargeCoroutine());
        agent.enabled = true;
        targeter.enabled = true;
    }
}
