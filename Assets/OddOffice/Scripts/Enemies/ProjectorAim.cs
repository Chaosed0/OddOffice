using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ProjectorAim : MonoBehaviour {
    public GameObject target;
    public float aimDuration;
    public float turnSpeed;
    public float beamCharge;
    public float beamDuration;
    public Hurtbox hurtbox;

    private ProjectorLaser laser;
    private Movement movement;

    void Awake ()
    {
        laser = GetComponent<ProjectorLaser>();
        movement = GetComponent<Movement>();

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
        if (movement != null)
        {
            movement.setCanMove(false);
        }
        yield return new WaitForSeconds(beamCharge);

        hurtbox.dealingDamage = true;
        yield return new WaitForSeconds(beamDuration);

        laser.End();
        hurtbox.dealingDamage = false;
        if (movement != null)
        {
            movement.setCanMove(true);
        }
        StartCoroutine(Aim());
    }
}
