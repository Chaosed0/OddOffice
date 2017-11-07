using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour {

    public Camera mainCam;
    public Camera laserCam;
	
	void Update ()
    {
        laserCam.transform.SetPositionAndRotation(mainCam.transform.position, mainCam.transform.rotation);
	}
}
