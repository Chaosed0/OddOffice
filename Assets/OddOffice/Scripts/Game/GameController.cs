using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Phase
{
    public float phaseTime;
    public ColliderEventer colliderToStartPhase;
    public Spawner[] spawners;
    public Door[] doorToOpenAtEnd;
    public GameObject focusTarget;
}

public class GameController : MonoBehaviour
{
    public float TimeForIntro = 5.0f;
    public Phase[] phases;
    public float TimeForOutroLead = 5.0f;
    public float TimeForOutro = 5.0f;

    public Door[] doorsToOpenAfterIntro;

    public GameObject daylight;
    public Blink blink;
    public Fadeout fadeout;
    public GameObject player;
    public ArmAnimator arms;
    public CanvasGroup HUD;
    private Movement playerMovement;
    private Gun playerGun;

    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;

    public Door[] monsterDoors;

    void Start()
    {
        daylight.SetActive(true);

        playerMovement = player.GetComponent<Movement>();
        playerGun = player.GetComponent<Gun>();
        SetupForCinematic();

        StartCoroutine(IntroCoroutine());

        initialPlayerPosition = player.transform.position;
        initialPlayerRotation = player.transform.rotation;

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
        // This is where the boss talks for some time
        yield return new WaitForSeconds(TimeForIntro);

        // Fade out, make everything dark
        fadeout.OnFadedOut.AddListener(() => {
            daylight.SetActive(false);

            // Open cafeteria doors
            foreach (Door door in doorsToOpenAfterIntro)
            {
                door.Open();
            }

            fadeout.OnFadedOut.RemoveAllListeners();
        });
        fadeout.OnFadedIn.AddListener(() => {
            // Allow the player to walk when the fade-in finishes
            SetupForWalking();
            fadeout.OnFadedIn.RemoveAllListeners();
        });
        fadeout.DoFade();

        // TODO: Aaah, I need some coffee

        AllowEntryToPhase(0);
    }

    void AllowEntryToPhase(int index)
    {
        phases[index].colliderToStartPhase.OnPlayerEntered.AddListener(() => {
            EnterPhase(index);
            phases[index].colliderToStartPhase.OnPlayerEntered.RemoveAllListeners();
        });
    }

    void EnterPhase(int index)
    {
        // Freeze the player in place
        SetupForCinematic();

        // Do a blink
        blink.DoBlink();

        blink.OnBlinkClose.AddListener(() => {
            // While the screen is black, setup the scene
            SetupForPhase(index);
            blink.OnBlinkClose.RemoveAllListeners();
        });

        blink.OnBlinkOpen.AddListener(() => {
            // Once the blink finishes, play whatever cutscene for the scene
            CinematicForPhase(index);
            blink.OnBlinkOpen.RemoveAllListeners();
        });

        foreach (Spawner spawner in phases[index].spawners)
        {
            spawner.enabled = true;
        }

        StartCoroutine(PhaseCoroutine(index));
    }

    // Called when eyes are closed, open doors and stuff
    void SetupForPhase(int index)
    {
        if (phases[index].focusTarget != null)
        {
            Camera.main.transform.rotation = Quaternion.LookRotation(phases[index].focusTarget.transform.position - player.transform.position);
        }

        foreach (Door door in monsterDoors)
        {
            door.Open();
        }

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        // PHASE SPECIFIC SETUP GOES HERE by index
    }

    // Called right after eyes open
    void CinematicForPhase(int index)
    {
        // Cinematic goes here, but for now we just enable player to shoot enemies
        SetupForShooting();
    }

    // Things that should happen in the phase go here
    IEnumerator PhaseCoroutine(int index)
    {
        // Wait for the phase to finish
        yield return new WaitForSeconds(phases[index].phaseTime);

        if (index+1 < phases.Length)
        {
            // If there's a next phase, finish this one and allow the player to walk around again
            blink.DoBlink();
            blink.OnBlinkClose.AddListener(() => {
                CleanupPhase(index);
                SetupForWalking();
                AllowEntryToPhase(index+1);
                blink.OnBlinkClose.RemoveAllListeners();
            });
        }
        else
        {
            blink.DoBlink();
            blink.OnBlinkClose.AddListener(() => {
                CleanupPhase(index);
                SetupForWalking();
                StartCoroutine(StartOutroLead());
                blink.OnBlinkClose.RemoveAllListeners();
            });
        }
    }

    // Happens after the phase ends, while eyes are closed
    void CleanupPhase(int index)
    {
        foreach (Spawner spawner in phases[index].spawners)
        {
            spawner.enabled = false;
        }

        foreach (Door door in monsterDoors)
        {
            door.Close();
        }

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            // TODO: Enemy deaths here should not play sounds
            enemy.GetComponent<Health>().DealDamage(9999999999);
        }

        // TODO: Play per-phase voice lines/phone ringing

        foreach (Door door in phases[index].doorToOpenAtEnd)
        {
            door.Open();
        }
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
        player.transform.position = initialPlayerPosition;
        player.transform.rotation = initialPlayerRotation;
        SetupForCinematic();
    }

    IEnumerator PlayOutro()
    {
        yield return new WaitForSeconds(TimeForOutro);

        Debug.Log("IT'S OVER");
    }

    void SetupForShooting()
    {
        playerMovement.setCanMove(true);
        playerGun.enabled = true;
        StartCoroutine(arms.ShowArms(0.5f));
        HUD.alpha = 1.0f;
    }

    void SetupForWalking()
    {
        playerMovement.setCanMove(true);
        playerGun.enabled = true;
        StartCoroutine(arms.HideArms(0.5f));
        HUD.alpha = 0.0f;
    }

    void SetupForCinematic()
    {
        playerMovement.setCanMove(false);
        playerGun.enabled = false;
        StartCoroutine(arms.HideArms(0.5f));
        HUD.alpha = 0.0f;
    }
}
