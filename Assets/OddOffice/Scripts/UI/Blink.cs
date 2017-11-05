using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Blink : MonoBehaviour
{
    public UnityEvent OnBlinkClose = new UnityEvent();
    public UnityEvent OnBlinkOpen = new UnityEvent();

    public void DoBlink()
    {
        StartCoroutine(tempwait());
    }

    IEnumerator tempwait()
    {
        Debug.Log("Start blink");
        yield return new WaitForSeconds(2.0f);
        OnBlinkClose.Invoke();
        Debug.Log("Blink Close");

        yield return new WaitForSeconds(2.0f);
        OnBlinkOpen.Invoke();
        Debug.Log("Blink Open");
    }
}
