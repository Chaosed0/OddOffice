using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ArmAnimator : MonoBehaviour
{
    public Gun gun;
    public Animator rightHandAnimator;
    public Animator leftHandAnimator;
    public Animator staplerAnimator;

    private float hideShowTimer = 0.0f;
    Quaternion normalRotation;
    Quaternion hiddenRotation;

    void Start()
    {
        gun.OnFireSuccess.AddListener(() => StartCoroutine(StartAttack()));
        gun.OnFireFail.AddListener(() => StartCoroutine(StartAttack()));
        gun.OnReloadStart.AddListener(() => StartCoroutine(StartReloading()));

        normalRotation = this.transform.localRotation;
        hiddenRotation = normalRotation * Quaternion.AngleAxis(90, Vector3.right);
    }

    IEnumerator StartAttack()
    {
        rightHandAnimator.SetBool("isAttacking", true);
        staplerAnimator.SetBool("isAttacking", true);
        yield return new WaitForEndOfFrame();
        rightHandAnimator.SetBool("isAttacking", false);
        staplerAnimator.SetBool("isAttacking", false);
    }

    IEnumerator StartReloading()
    {
        rightHandAnimator.SetBool("isReloading", true);
        leftHandAnimator.SetBool("isReloading", true);
        staplerAnimator.SetBool("isReloading", true);
        yield return new WaitForEndOfFrame();
        rightHandAnimator.SetBool("isReloading", false);
        leftHandAnimator.SetBool("isReloading", false);
        staplerAnimator.SetBool("isReloading", false);
    }

    public IEnumerator HideArms(float time)
    {
        return HideShowArms(true, time);
    }

    public IEnumerator ShowArms(float time)
    {
        return HideShowArms(false, time);
    }

    public IEnumerator HideShowArms(bool hide, float time)
    {
        hideShowTimer = 0.0f;
        while (hideShowTimer < time)
        {
            hideShowTimer += Time.deltaTime;
            float lerp = hideShowTimer / time;
            if (hide)
            {
                transform.localRotation = Quaternion.Lerp(normalRotation, hiddenRotation, lerp);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(hiddenRotation, normalRotation, lerp);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
