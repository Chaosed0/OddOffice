using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    public float alphaPerSecond;

    private Image fadeImage;

    void Awake()
    {
        Cursor.visible = false;
        fadeImage = GameObject.Find("TitlePanel").GetComponent<Image>();
    }

	void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeOut());
        }
	}

    IEnumerator FadeOut()
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * alphaPerSecond;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
