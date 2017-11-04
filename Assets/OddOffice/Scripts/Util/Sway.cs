using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour {

    private Material mat;
    private float height;

    void Awake ()
    {
        mat = GetComponent<MeshRenderer>().material;
        height = GetComponent<BoxCollider>().size.y;
        mat.SetFloat("_Top", transform.position.y + height / 2);
        mat.SetFloat("_Bottom", transform.position.y - height / 2);
        mat.SetFloat("_ObjHeight", height);
        mat.SetFloat("_ObjWidth", GetComponent<BoxCollider>().size.x);
        mat.SetFloat("_MidPoint", GetComponent<BoxCollider>().center.y);
    }

    void Update()
    {
        mat.SetFloat("_MidPoint", GetComponent<BoxCollider>().center.y);
    }
}
