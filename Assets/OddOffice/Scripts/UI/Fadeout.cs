using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fadeout : MonoBehaviour
{
    public float fadeTime = 0.5f;
    public float holdTime = 1.0f;

    private CanvasGroup canvasGroup;
    private float timer = 0.0f;

    public UnityEvent OnFadedOut = new UnityEvent();
    public UnityEvent OnFadedIn = new UnityEvent();

    void Start ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    public void DoFade()
    {
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        while (timer < fadeTime)
        {
            float lerp = timer / fadeTime;
            canvasGroup.alpha = lerp;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        OnFadedOut.Invoke();
        timer = 0.0f;
        yield return new WaitForSeconds(holdTime);

        while (timer < fadeTime)
        {
            float lerp = timer / fadeTime;
            canvasGroup.alpha = 1.0f - lerp;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        OnFadedIn.Invoke();
        timer = 0.0f;
    }
}
