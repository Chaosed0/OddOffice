using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraEffects : MonoBehaviour
{
    private PostProcessingProfile profile;

    void Awake()
    {
        profile = GetComponent<PostProcessingBehaviour>().profile;

        profile.chromaticAberration.enabled = false;
        profile.motionBlur.enabled = false;
    }
    
    void Update()
    {
        //for testing
        //if (Input.GetMouseButtonDown(0))
        //{
        //    EventBus.PublishEvent(new ActivateTrip());
        //}
    }

    [SubscribeGlobal]
    public void HandleTrip(ActivateTrip e)
    {
        profile.chromaticAberration.enabled = true;
        profile.motionBlur.enabled = true;
    }

    [SubscribeGlobal]
    public void DeactivateTrip(DeactivateTrip e)
    {
        profile.chromaticAberration.enabled = false;
        profile.motionBlur.enabled = false;
    }
}
