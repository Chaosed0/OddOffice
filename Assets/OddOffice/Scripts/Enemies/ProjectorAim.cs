using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ProjectorAim : MonoBehaviour {
    public GameObject target;
    public float aimDuration;
    public float turnSpeed;
    public float beamCharge;
    public float beamDuration;
    public float chaseStopDistance;
    public LaserHurtbox hurtbox;
    public GameObject beamStartLoc;

    public AudioSource audioSource;
    public AudioClip chargeClip;
    public AudioClip shootingClip;

    private ProjectorLaser laser;
    private Targeter targeter;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private GameObject player;

    void Awake ()
    {
        laser = GetComponent<ProjectorLaser>();
        targeter = GetComponent<Targeter>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player") as GameObject;

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

    void Update ()
    {
        RaycastHit hitInfo;
        bool hit = Physics.BoxCast(beamStartLoc.transform.position, new Vector3(0.5f, 0.5f, 0.5f), player.transform.position - beamStartLoc.transform.position, 
            out hitInfo, beamStartLoc.transform.rotation, chaseStopDistance);
        if (hit)
        {
            targeter.SetNavigationActive(hitInfo.collider.gameObject.layer != 9 && hitInfo.collider.gameObject.layer != 8);
        }
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
            audioSource.clip = chargeClip;
            audioSource.Play();
        }
        yield return new WaitForSeconds(beamCharge);

        hurtbox.enabled = true;
        audioSource.clip = shootingClip;
        audioSource.Play();
        yield return new WaitForSeconds(beamDuration);

        laser.End();
        hurtbox.DeactivateImpactFlame();
        hurtbox.enabled = false;
        if (targeter != null)
        {
            targeter.enabled = true;
            agent.enabled = true;
        }
        StartCoroutine(Aim());
    }
}
