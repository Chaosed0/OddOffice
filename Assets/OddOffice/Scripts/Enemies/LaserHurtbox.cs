using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LaserHurtbox : MonoBehaviour
{
    public float damagePerSecond = 5.0f;
    public GameObject beamStartLoc;

    bool hurting = false;
    public UnityEvent OnStartHurting = new UnityEvent();
    public UnityEvent OnStopHurting = new UnityEvent();

    void Update()
    {
        RaycastHit hitInfo;
        bool hit = Physics.BoxCast(beamStartLoc.transform.position, new Vector3(0.5f, 0.5f, 0.5f), beamStartLoc.transform.forward, out hitInfo, beamStartLoc.transform.rotation);

        Debug.Log(hitInfo.collider.name);
        if (hit && hitInfo.collider && hitInfo.collider.gameObject.layer == 9)
        {
            hitInfo.collider.GetComponent<Health>().DealDamage(damagePerSecond * Time.deltaTime);
            if (!hurting)
            {
                hurting = true;
                OnStartHurting.Invoke();
            }
        }
        else
        {
            if (hurting)
            {
                hurting = false;
                OnStopHurting.Invoke();
            }
        }
    }
}
