using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LaserHurtbox : MonoBehaviour
{
    public float damagePerSecond = 5.0f;
    public GameObject beamStartLoc;
    public ParticleSystem impactFlame;

    bool hurting = false;
    public UnityEvent OnStartHurting = new UnityEvent();
    public UnityEvent OnStopHurting = new UnityEvent();

    private void Awake ()
    {
        impactFlame.transform.parent.SetParent(null);
    }

    void Update()
    {
        RaycastHit hitInfo;
        bool hit = Physics.BoxCast(beamStartLoc.transform.position, new Vector3(0.5f, 0.5f, 0.5f), beamStartLoc.transform.forward, out hitInfo, beamStartLoc.transform.rotation);

        if (hit && hitInfo.collider)
        {
            if (hitInfo.collider.gameObject.layer != 8 && hitInfo.collider.gameObject.layer != 11)
            {
                impactFlame.transform.parent.SetPositionAndRotation(hitInfo.point - transform.forward * .35f + transform.up * .3f, Quaternion.identity);
                impactFlame.Play();
            }
            if (hitInfo.collider.gameObject.layer == 9)
            {
                hitInfo.collider.GetComponent<Health>().DealDamage(damagePerSecond * Time.deltaTime);
                if (!hurting)
                {
                    hurting = true;
                    OnStartHurting.Invoke();
                }
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

    public void DeactivateImpactFlame()
    {
        Invoke("StopFlame", .55f);
    }

    void StopFlame()
    {
        impactFlame.Stop();
    }
}
