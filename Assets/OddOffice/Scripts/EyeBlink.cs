using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour {

    public Material eyeBlinkMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Time.time > 10.0f)
        {
            eyeBlinkMaterial.SetFloat("_StartTime", Time.time);
            Graphics.Blit(source, destination, eyeBlinkMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
