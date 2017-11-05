using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class DamageEffect : MonoBehaviour
{
    private PostProcessingProfile profile;

    public Health playerHealth;
    public float maxIntensity;
    public float vignetteSpeed;

    private VignetteModel.Settings vignette;
    private int vignetteDirection = -1;

    void Awake ()
    {
        profile = GetComponent<PostProcessingBehaviour>().profile;

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealth.OnHealthChanged.AddListener(() => DamagePlayer());
    }

    void DamagePlayer()
    {
        vignetteDirection = 1;
    }

    void Update()
    {
        if (vignetteDirection == -1 && vignette.intensity <= 0) return;

        vignette = profile.vignette.settings;

        vignette.intensity = Mathf.Clamp(vignette.intensity + vignetteDirection * vignetteSpeed * Time.deltaTime, 0, maxIntensity);
        if (vignette.intensity >= maxIntensity)
        {
            vignetteDirection = -1;
        }

        profile.vignette.settings = vignette;
    }
}