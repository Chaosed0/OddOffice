using System.Collections;
using UnityEngine;

public class ProjectorLaser : MonoBehaviour {
    public ParticleSystem chargeEnergy;
    public ParticleSystem chargeBall;
    public ParticleSystem chargeCore;
    public ParticleSystem laserBeam;
    public ParticleSystem laserCore;

    public float chargeUpDuration;
    public float chargeBallStartScale;

    public float laserShrinkDuration;
    public float laserInitialScale;

    public float damagePerSecond;

    public LayerMask targetLayerMask;

    private bool firing = true;

    public void Fire()
    {
        firing = true;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        float chargeUpAccumulatedSeconds = 0;
        float laserScaleAccumulatedSeconds = 0;
        chargeEnergy.Play();

        chargeBall.transform.localScale = new Vector3();
        chargeCore.transform.localScale = new Vector3();

        chargeBall.Play();
        chargeCore.Play();

        while (chargeBall.transform.localScale.x < 1)
        {
            chargeUpAccumulatedSeconds += Time.deltaTime;
            float scale = Mathf.Clamp01(chargeUpAccumulatedSeconds / chargeUpDuration);

            chargeBall.transform.localScale = new Vector3(scale, scale, scale);
            chargeCore.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }

        chargeEnergy.Stop();

        laserBeam.transform.localScale = new Vector3(laserInitialScale, laserInitialScale, laserInitialScale);
        laserCore.transform.localScale = new Vector3(laserInitialScale, laserInitialScale, laserInitialScale);

        laserBeam.Play();
        laserCore.Play();

        while (laserBeam.transform.localScale.x > 1 && firing)
        {
            laserScaleAccumulatedSeconds += Time.deltaTime;
            float scale = Mathf.Max(0, laserInitialScale - (laserScaleAccumulatedSeconds / chargeUpAccumulatedSeconds) * (laserInitialScale - 1));

            laserBeam.transform.localScale = new Vector3(scale, scale, scale);
            laserCore.transform.localScale = new Vector3(scale, scale, scale);

            RaycastHit hitInfo;
            Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, targetLayerMask);

            if (hitInfo.collider)
            {
                // do damage
            }

            yield return null;
        }
    }

    public void End()
    {
        chargeBall.Stop();
        chargeCore.Stop();
        laserBeam.Stop();
        laserCore.Stop();

        firing = false;
    }
}
