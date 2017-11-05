using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class ProjectorAim : MonoBehaviour {
    public GameObject target;
    public float aimDuration;
    public float turnSpeed;
    public float beamCharge;
    public float beamDuration;
    public LaserHurtbox hurtbox;

    private ProjectorLaser laser;
    private Targeter targeter;
    private NavMeshAgent agent;
    private Rigidbody rb;

    void Awake ()
    {
        laser = GetComponent<ProjectorLaser>();
        targeter = GetComponent<Targeter>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        hurtbox.enabled = false;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Start ()
    {
        StartCoroutine(Aim());
    }

    IEnumerator Aim()
    {
        float accumulatedTime = 0;

        while (accumulatedTime < aimDuration) {
            accumulatedTime += Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            yield return null;
        }

        laser.Fire();
        if (targeter != null)
        {
            targeter.enabled = false;
            agent.enabled = false;
        }
        yield return new WaitForSeconds(beamCharge);

        hurtbox.enabled = true;
        yield return new WaitForSeconds(beamDuration);

        laser.End();
        hurtbox.enabled = false;
        if (targeter != null)
        {
            targeter.enabled = true;
            agent.enabled = true;
        }
        StartCoroutine(Aim());
    }
}
