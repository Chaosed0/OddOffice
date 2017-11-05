using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fadeout : MonoBehaviour
{
    public float fadeOutTime = 0.5f;
    public float holdTime = 1.0f;
    public float fadeInTime = 0.5f;

    private CanvasGroup canvasGroup;
    private float timer = 0.0f;

    public UnityEvent OnFadedOut = new UnityEvent();
    public UnityEvent OnFadedIn = new UnityEvent();

    IEnumerator currentFade;

    void Start ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    public void DoFade()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        timer = 0.0f;
        currentFade = FadeCoroutine();
        StartCoroutine(currentFade);
    }

    IEnumerator FadeCoroutine()
    {
        while (timer < fadeOutTime)
        {
            float lerp = timer / fadeOutTime;
            canvasGroup.alpha = lerp;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        OnFadedOut.Invoke();
        timer = 0.0f;
        yield return new WaitForSeconds(holdTime);

        while (timer < fadeInTime)
        {
            float lerp = timer / fadeInTime;
            canvasGroup.alpha = 1.0f - lerp;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        OnFadedIn.Invoke();
        currentFade = null;
    }
}
