using System.Collections;
using UnityEngine;

public class MugShot : MonoBehaviour {
    public float height;
    public GameObject target;
    public StackPool globPool;
    public float maxMissAmount;
    public GameObject launchStartPoint;
    public float coffeeGravity = -.5f;

    private float adjustedHeight;
    private float trajectoryDuration;

    void Awake()
    {
        globPool = GameObject.Find("CoffeeProjectilePool").GetComponent<StackPool>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        StartCoroutine(Lob());
    }

    IEnumerator Lob()
    {
        GameObject glob = globPool.Pop();
        glob.transform.position = launchStartPoint.transform.position;
        glob.GetComponent<CoffeeProjectile>().stackPool = globPool;
        glob.GetComponent<CoffeeProjectile>().grav = coffeeGravity;

        glob.GetComponent<Rigidbody>().velocity = CalculateLaunchVelocity();
        glob.GetComponent<Expires>().pool = globPool;

        EventBus.PublishEvent(new TestEvent());

        yield return new WaitForSeconds(2);
        StartCoroutine(Lob());
    }

    Vector3 CalculateLaunchVelocity ()
    {
        Vector3 offset = Random.insideUnitSphere * Random.Range(0, maxMissAmount);
        offset.y = 0;

        Vector3 targetPosition = target.transform.position + offset;

        Vector3 transformedPoint = transform.InverseTransformPoint(targetPosition);
        adjustedHeight = Mathf.Max(0, transformedPoint.y) + height;

        float displacementUp = transformedPoint.y;
        Vector3 toTarget = targetPosition - launchStartPoint.transform.position;
        toTarget.y = 0;
        float displacementForward = toTarget.magnitude;

        float velUp = Mathf.Sqrt(-2 * coffeeGravity * adjustedHeight);
        float velForward = displacementForward / (Mathf.Sqrt(-2 * adjustedHeight / coffeeGravity) + Mathf.Sqrt(2 * (displacementUp - adjustedHeight) / coffeeGravity));

        float angle = Mathf.Atan2(toTarget.z, toTarget.x);
        return new Vector3(velForward * Mathf.Cos(angle), velUp, velForward * Mathf.Sin(angle));
    }
}
