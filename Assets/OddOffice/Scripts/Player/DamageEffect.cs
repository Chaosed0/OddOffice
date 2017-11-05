using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class DamageEffect : MonoBehaviour
{
    PostProcessingProfile profile;

    public Health playerHealth;
    public float maxIntensity;
    public float vignetteSpeed;

    private VignetteModel.Settings vignette;
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
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealth.OnHealthChanged.AddListener(() => DamagePlayer());

        vignette = profile.vignette.settings;
    }

    void DamagePlayer()
    {
        vignetteDirection = 1;
    }

    void Update()
    {
        if (vignetteDirection == -1 && vignette.intensity <= 0) return;

        vignette.intensity = Mathf.Clamp(vignette.intensity + vignetteDirection * vignetteSpeed * Time.deltaTime, 0, maxIntensity);
        if (vignette.intensity >= maxIntensity)
        {
            vignetteDirection = -1;
        }

        profile.vignette.settings = vignette;
    }
}