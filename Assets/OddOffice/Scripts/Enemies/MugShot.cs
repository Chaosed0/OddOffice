using System.Collections;
using UnityEngine;

public class MugShot : MonoBehaviour {
    public float height;
    public GameObject target;
    public StackPool globPool;

    private float adjustedHeight;
    private float trajectoryDuration;

    void Awake()
    {
        globPool = GameObject.Find("CoffeeProjectilePool").GetComponent<StackPool>();
        StartCoroutine(Lob());
    }

    IEnumerator Lob()
    {
        GameObject glob = globPool.Pop();

        glob.transform.position = transform.position;
        glob.GetComponent<Rigidbody>().velocity = CalculateLaunchVelocity();
        glob.GetComponent<Expires>().pool = globPool;

        EventBus.PublishEvent(new TestEvent());

        yield return new WaitForSeconds(2);
        StartCoroutine(Lob());
    }

    Vector3 CalculateLaunchVelocity ()
    {
        Vector3 transformedPoint = transform.InverseTransformPoint(target.transform.position);
        adjustedHeight = Mathf.Max(0, transformedPoint.y) + height;

        float displacementUp = transformedPoint.y;
        Vector3 toTarget = target.transform.position - transform.position;
        toTarget.y = 0;
        float displacementForward = toTarget.magnitude;

        float velUp = Mathf.Sqrt(-2 * Physics.gravity.y * adjustedHeight);
        float velForward = displacementForward / (Mathf.Sqrt(-2 * adjustedHeight / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementUp - adjustedHeight) / Physics.gravity.y));

        float angle = Mathf.Atan2(toTarget.z, toTarget.x);
        return new Vector3(velForward * Mathf.Cos(angle), velUp, velForward * Mathf.Sin(angle));
    }
}
