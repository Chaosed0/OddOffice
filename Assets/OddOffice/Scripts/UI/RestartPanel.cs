using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartPanel : MonoBehaviour
{
    public Health playerHealth;
    public PlayerInput playerInput;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        playerHealth.OnDied.AddListener(() => StartCoroutine(ShowRestart(2.0f)));
    }

    IEnumerator ShowRestart(float time)
    {
        yield return new WaitForSeconds(time);
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        playerInput.UnlockCursorPermanent();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
