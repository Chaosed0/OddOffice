using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ArmAnimator : MonoBehaviour
{
    public Gun gun;
    public Animator rightHandAnimator;
    public Animator leftHandAnimator;

    void Start()
    {
        gun.OnFireSuccess.AddListener(() => StartCoroutine(StartAttack()));
        gun.OnFireFail.AddListener(() => StartCoroutine(StartAttack()));
        gun.OnReloadStart.AddListener(() => StartCoroutine(StartReloading()));
    }

    IEnumerator StartAttack()
    {
        rightHandAnimator.SetBool("isAttacking", true);
        yield return new WaitForEndOfFrame();
        rightHandAnimator.SetBool("isAttacking", false);
    }

    IEnumerator StartReloading()
    {
        rightHandAnimator.SetBool("isReloading", true);
        leftHandAnimator.SetBool("isReloading", true);
        yield return new WaitForEndOfFrame();
        rightHandAnimator.SetBool("isReloading", false);
        leftHandAnimator.SetBool("isReloading", false);
    }
}
