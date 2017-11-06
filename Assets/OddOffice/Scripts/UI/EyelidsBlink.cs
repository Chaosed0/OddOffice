using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyelidsBlink : MonoBehaviour
{
    public Image eyeLidUpper;
    public Image eyeLidLower;

    public Vector3 upperTopPos;
    public Vector3 upperBottomPos;
    public Vector3 lowerTopPos;
    public Vector3 lowerBottomPos;

    private float blinkDuration = 8f;

    bool isBlinking = false;
    float elapsedTime = 0f;

    float rate = 1.75f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BlinkEyes();
        }

        if (isBlinking)
        {
            elapsedTime += Time.deltaTime;
            float phase = 1f - (0.5f * Mathf.Cos(elapsedTime * Mathf.PI * rate) + 0.5f);

            if (elapsedTime > 5.0f / rate && elapsedTime < 6.66f / rate)
            {
                phase = 1.0f;
            }

            eyeLidUpper.rectTransform.position = Vector3.Lerp(upperTopPos, upperBottomPos, phase);
            eyeLidLower.rectTransform.position = Vector3.Lerp(lowerBottomPos, lowerTopPos, phase);
        }
        if (elapsedTime > blinkDuration / rate)
        {
            isBlinking = false;
        }
    }

    void BlinkEyes()
    {
        elapsedTime = 0f;
        isBlinking = true;
    }
	 
}
