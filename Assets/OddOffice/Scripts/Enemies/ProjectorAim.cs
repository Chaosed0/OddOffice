using System.Collections;
using UnityEngine;

public class ProjectorAim : MonoBehaviour {
    public GameObject target;
    public float aimDuration;
    public float turnSpeed;
    public float beamDuration;

    private ProjectorLaser laser;

    void Awake ()
    {
        laser = GetComponent<ProjectorLaser>();
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
        yield return new WaitForSeconds(beamDuration);

        laser.End();
        StartCoroutine(Aim());
    }
}
