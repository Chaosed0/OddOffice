using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class LightsManager : MonoBehaviour {

    public Color yellowColor;
    public Color whiteColor;

    public GameObject lightsWhite;
    public GameObject lightsYellow;

    public GameObject roof;
    private Material ceilingLightsMaterial;

    public GameObject mainCamera;
    private PostProcessingProfile profile;

    // Use this for initialization
    void Start () {
        profile = mainCamera.GetComponent<PostProcessingBehaviour>().profile;
        ceilingLightsMaterial = roof.GetComponent<Renderer>().materials[1];
        // SetNormalLighting();
        SetTrippyLighting();
    }

    [SubscribeGlobal]
    public void HandleTrip(ActivateTrip e)
    {
        SetTrippyLighting();
    }

    [SubscribeGlobal]
    public void HandleTrip(DeactivateTrip e)
    {
        SetNormalLighting();
    }

    private void SetNormalLighting()
    {
        profile.bloom.enabled = false;

        lightsWhite.SetActive(true);
        lightsYellow.SetActive(false);
        
        ceilingLightsMaterial.SetColor("_EmissionColor", whiteColor);

        RenderSettings.ambientIntensity = 1.75f;
    }

    private void SetTrippyLighting()
    {
        RenderSettings.ambientIntensity = 1.00f;

        lightsWhite.SetActive(false);
        lightsYellow.SetActive(true);

        ceilingLightsMaterial.SetColor("_EmissionColor", yellowColor);

        profile.bloom.enabled = true;
    }
}
