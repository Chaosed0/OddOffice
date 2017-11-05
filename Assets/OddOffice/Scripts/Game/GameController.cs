using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Phase
{
    public float phaseTime;
    public ColliderEventer colliderToStartPhase;
    public Spawner[] spawners;
    public GameObject doorToOpenAtEnd;
    public GameObject focusTarget;
}

public class GameController : MonoBehaviour
{
    public float TimeForIntro = 5.0f;
    public Phase[] phases;
    public float TimeForOutroLead = 5.0f;
    public float TimeForOutro = 5.0f;

    public GameObject daylight;
    public Blink blink;
    public Fadeout fadeout;
    public GameObject player;
    private Movement playerMovement;

    public GameObject[] monsterDoors;

    void Start()
    {
        daylight.SetActive(true);

        playerMovement = player.GetComponent<Movement>();
        playerMovement.setCanMove(false);
        StartCoroutine(IntroCoroutine());

        foreach (Phase phase in phases)
        {
            foreach (Spawner spawner in phase.spawners)
            {
                spawner.enabled = false;
            }
        }
    }
    
    IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(TimeForIntro);

        fadeout.OnFadedOut.AddListener(() => {
            daylight.SetActive(false);
            fadeout.OnFadedOut.RemoveAllListeners();
        });
        fadeout.OnFadedIn.AddListener(() => {
            playerMovement.setCanMove(true);
            fadeout.OnFadedIn.RemoveAllListeners();
        });
        fadeout.DoFade();

        PrepForPhase(0);
    }

    void PrepForPhase(int index)
    {
        phases[index].colliderToStartPhase.OnPlayerEntered.AddListener(() => StartPhase(0));
    }

    void StartPhase(int index)
    {
        playerMovement.setCanMove(false);
        blink.DoBlink();
        blink.OnBlinkClose.AddListener(() => {
            if (phases[index].focusTarget != null)
            {
                Camera.main.transform.rotation = Quaternion.LookRotation(phases[index].focusTarget.transform.position - player.transform.position);
            }
            foreach (GameObject go in monsterDoors)
            {
                go.SetActive(false);
            }

            // PHASE SPECIFIC SETUP GOES HERE

            blink.OnBlinkClose.RemoveAllListeners();
        });

        blink.OnBlinkOpen.AddListener(() => {
            playerMovement.setCanMove(true);
            blink.OnBlinkOpen.RemoveAllListeners();

            // PHASE SPECIFIC SETUP GOES HERE
        });

        foreach (Spawner spawner in phases[index].spawners)
        {
            spawner.enabled = true;
        }

        StartCoroutine(PhaseCoroutine(index));
    }

    IEnumerator PhaseCoroutine(int index)
    {
        yield return new WaitForSeconds(phases[index].phaseTime);
        if (index+1 < phases.Length)
        {
            blink.DoBlink();
            blink.OnBlinkClose.AddListener(() => {
                CleanupPhase(index);
                PrepForPhase(index+1);
                blink.OnBlinkClose.RemoveAllListeners();
            });
        }
        else
        {
            blink.DoBlink();
            blink.OnBlinkClose.AddListener(() => {
                CleanupPhase(index);
                StartCoroutine(StartOutroLead());
                blink.OnBlinkClose.RemoveAllListeners();
            });
        }
    }

    void CleanupPhase(int index)
    {
        foreach (Spawner spawner in phases[index].spawners)
        {
            spawner.enabled = true;
        }

        foreach (GameObject go in monsterDoors)
        {
            go.SetActive(true);
        }

        phases[index].doorToOpenAtEnd.SetActive(false);
    }

    IEnumerator StartOutroLead()
    {
        yield return new WaitForSeconds(TimeForOutroLead);

        fadeout.OnFadedOut.AddListener(() => {
            PrepOutro();
            fadeout.OnFadedOut.RemoveAllListeners();
        });
        fadeout.OnFadedIn.AddListener(() => {
            StartCoroutine(PlayOutro());
            fadeout.OnFadedIn.RemoveAllListeners();
        });
        fadeout.DoFade();
    }

    void PrepOutro()
    {
        daylight.SetActive(true);
    }

    IEnumerator PlayOutro()
    {
        yield return new WaitForSeconds(TimeForOutro);

        Debug.Log("IT'S OVER");
    }
}
