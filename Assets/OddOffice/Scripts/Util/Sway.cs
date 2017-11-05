using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour {

    private Material mat;
    private float height;
    private BoxCollider bc;
    float modifier = 0;

    void Awake ()
    {
        mat = GetComponent<MeshRenderer>().material;
        bc = GetComponent<BoxCollider>();
        height = bc.size.y;
        mat.SetFloat("_Top", transform.position.y + height / 2);
        mat.SetFloat("_Bottom", transform.position.y - height / 2);
        mat.SetFloat("_ObjHeight", height);
        mat.SetFloat("_ObjWidth", bc.size.x);
        mat.SetFloat("_MidPoint", bc.center.y);
        mat.SetFloat("_RandomOffset", Random.Range(0, Mathf.PI / 6));
    }

    void Update()
    {
        mat.SetFloat("_MidPoint", bc.center.y);
        mat.SetFloat("_Modifier", modifier);
    }

    [SubscribeGlobal]
    public void HandleTrip(ActivateTrip e)
    {
        modifier = 1;
    }

    [SubscribeGlobal]
    public void StopTrip(DeactivateTrip e)
    {
        modifier = 0;
    }
}

// set modifier to 1 to activate, 0 to deactivate.
public struct ActivateTrip
{}

public struct DeactivateTrip
{}