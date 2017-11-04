using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class DamageEffect : MonoBehaviour
{
    PostProcessingProfile profile;

    public float maxIntensity;
    public float vignetteSpeed;

    private int vignetteDirection = 1;

    void Awake ()
    {
        PostProcessingBehaviour behaviour = GetComponent<PostProcessingBehaviour>();

        if (behaviour.profile == null)
        {
            enabled = false;
            return;
        }

        profile = Instantiate(behaviour.profile);
        behaviour.profile = profile;
    }

    void Update()
    {
        // Test
        //if (Input.GetMouseButtonDown(0))
        //{
        //    DamagePlayer();
        //}
    }

    public void DamagePlayer()
    {
        vignetteDirection = 1;
        StopCoroutine(AnimateBlood());
        StartCoroutine(AnimateBlood());
    }

    IEnumerator AnimateBlood()
    {
        var vignette = profile.vignette.settings;

        while (true)
        {
            vignette.intensity = Mathf.Clamp(vignette.intensity + vignetteDirection * vignetteSpeed * Time.deltaTime, 0, maxIntensity);
            if (vignette.intensity == maxIntensity)
            {
                vignetteDirection = -1;
            }
            else if (vignette.intensity == 0 && vignetteDirection == -1)
            {
                break;
            }

            profile.vignette.settings = vignette;
            yield return null;
        }
    }
}