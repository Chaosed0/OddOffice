using System;
using UnityEngine;

public class Screenshot : MonoBehaviour {
	void Update () {
		if (Input.GetMouseButtonDown(1))
        {
            string filename = "OddOfficeScreenshot" + DateTime.UtcNow.ToFileTimeUtc() + ".png";
            ScreenCapture.CaptureScreenshot("OddOfficeScreenshot" + filename + ".png");
            Debug.Log("captured screenshot with filename " + filename);
        }
	}
}
