using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Blink : MonoBehaviour
{
    public UnityEvent OnBlinkClose = new UnityEvent();
    public UnityEvent OnBlinkOpen = new UnityEvent();

    public Fadeout tempFadeout;
    int blinkIndex = 0;

    public void DoBlink()
    {
        blinkIndex = 0;

        tempFadeout.fadeOutTime = 0.1f;
        tempFadeout.fadeInTime = 0.1f;
        tempFadeout.holdTime = 0.4f;
        tempFadeout.DoFade();

        tempFadeout.OnFadedOut.AddListener(OnFadedOut);
        tempFadeout.OnFadedIn.AddListener(OnFadedIn);
    }

    public void OnFadedOut()
    {
        if (blinkIndex == 3)
        {
            OnBlinkClose.Invoke();
            tempFadeout.OnFadedOut.RemoveAllListeners();
        }
    }

    public void OnFadedIn()
    {
        if (blinkIndex < 2)
        {
            tempFadeout.DoFade();
        }
        else if (blinkIndex == 2)
        {
            tempFadeout.fadeOutTime = 1.0f;
            tempFadeout.holdTime = 1.0f;
            tempFadeout.DoFade();
        }
        else
        {
            OnBlinkOpen.Invoke();
            tempFadeout.OnFadedIn.RemoveAllListeners();
        }
        blinkIndex++;
    }
}
