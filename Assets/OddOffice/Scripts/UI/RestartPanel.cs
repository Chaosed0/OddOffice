using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartPanel : MonoBehaviour
{
    public Health playerHealth;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        playerHealth.OnDied.AddListener(() => ShowRestart(1.0f));
    }

    IEnumerator ShowRestart(float time)
    {
        yield return new WaitForSeconds(time);
        canvasGroup.alpha = 1.0f;
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("DaScene");
    }
}
