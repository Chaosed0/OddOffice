using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

    float defaultY;

    public float amplitude;
    public float rate;

    private void Start()
    {
        defaultY = transform.position.y;
    }

    // Update is called once per frame
    void Update () {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, defaultY + amplitude * Mathf.Sin(Time.time * rate), pos.z);
	}
}
