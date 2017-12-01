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
    public GameObject[] thingsToActivate;
    public GameObject[] thingsToDeactivate;
    public AudioClip prePhaseTalk;
    public AudioClip prePhaseYawn;
    public AudioClip phaseBeginTalk;
    public AudioClip music;
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

    public ColliderEventer playerDeskCollider;
    public ColliderEventer exitCollider;

    public AudioSource voiceClipSource;
    public AudioSource musicSource;

    public AudioClip introTalk;
    public AudioClip introTalk2;
    public AudioClip preOutroTalk;
    public AudioClip outroTalk;

    public Phone phone;
    private Movement playerMovement;
    private Gun playerGun;
    private PlayerInput playerInput;

    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;

    public Door[] monsterDoors;
    public Door receptionDoor;

    public LightsManager lightsManager;

    public CanvasGroup credits;

    void Start()
    {
        daylight.SetActive(true);

        playerMovement = player.GetComponent<Movement>();
        playerGun = player.GetComponent<Gun>();
        playerInput = player.GetComponent<PlayerInput>();
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
        daylight.SetActive(false);

        // This is where the boss talks for some time
        PlayVoiceLine(introTalk);
        yield return new WaitForSeconds(TimeForIntro);
        PlayVoiceLine(introTalk2);

        // Fade out, make everything dark
        //fadeout.OnFadedOut.AddListener(() => {
            // Open cafeteria doors
            foreach (Door door in doorsToOpenAfterIntro)
            {
                door.Open();
            }

            //fadeout.OnFadedOut.RemoveAllListeners();
        //});
        //fadeout.OnFadedIn.AddListener(() => {
            // Allow the player to walk when the fade-in finishes
            SetupForWalking();
            //fadeout.OnFadedIn.RemoveAllListeners();
            //PlayVoiceLine(phases[0].phaseBeginTalk);
        //});
        //fadeout.DoFade();

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

        PlayVoiceLine(phases[index].prePhaseYawn);

        blink.OnBlinkClose.AddListener(() => {
            // While the screen is black, setup the scene
            SetupForPhase(index);
            blink.OnBlinkClose.RemoveAllListeners();
        });

        blink.OnBlinkOpen.AddListener(() => {
            // Once the blink finishes, do stuff
            PhaseBegin(index);
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

        foreach (GameObject activateMe in phases[index].thingsToActivate)
        {
            activateMe.SetActive(true);
        }

        foreach (GameObject activateMe in phases[index].thingsToDeactivate)
        {
            activateMe.SetActive(false);
        }

        lightsManager.SetTrippyLighting();

        if (index == 2)
        {
            EventBus.PublishEvent(new ActivateTrip());
        }
    }

    // Called right after eyes open
    void PhaseBegin(int index)
    {
        // Cinematic goes here, but for now we just enable player to shoot enemies
        SetupForShooting();
        musicSource.clip = phases[index].music;
        musicSource.loop = true;
        musicSource.Play();

        PlayVoiceLine(phases[index].phaseBeginTalk);
    }

    // Things that should happen in the phase go here
    IEnumerator PhaseCoroutine(int index)
    {
        // Wait for the phase to finish
        yield return new WaitForSeconds(phases[index].phaseTime);

        musicSource.Stop();

        if (index+1 < phases.Length)
        {
            phone.StartRinging();

            // If there's a next phase, finish this one and allow the player to walk around again
            blink.DoBlink();
            blink.OnBlinkClose.AddListener(() => {
                CleanupPhase(index);
                SetupForWalking();
                SetupBetweenPhase(index+1);
                blink.OnBlinkClose.RemoveAllListeners();
            });
        }
        else
        {
            blink.DoBlink();
            blink.OnBlinkClose.AddListener(() => {
                CleanupPhase(index);
                SetupForWalking();
                StartOutroLead();
                blink.OnBlinkClose.RemoveAllListeners();
            });

            blink.OnBlinkOpen.AddListener(() => {
                PlayVoiceLine(preOutroTalk);
                blink.OnBlinkOpen.RemoveAllListeners();
            });
        }
    }

    // This gets called when a phase ends, eyes are closed
    void SetupBetweenPhase(int index)
    {
        playerDeskCollider.OnPlayerEntered.AddListener(() => {
            AllowEntryToPhase(index);
            PlayVoiceLine(phases[index].prePhaseTalk);
            phone.StopRinging();
            playerDeskCollider.OnPlayerEntered.RemoveAllListeners();
        });
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

        foreach (Door door in phases[index].doorToOpenAtEnd)
        {
            door.Open();
        }

        lightsManager.SetNormalLighting();

        if (index == 2)
        {
            EventBus.PublishEvent(new DeactivateTrip());
        }
    }

    void StartOutroLead()
    {
        receptionDoor.Unmonster();
        receptionDoor.Open();

        exitCollider.OnPlayerEntered.AddListener(() => {
            StartOutro();
            exitCollider.OnPlayerEntered.RemoveAllListeners();
        });
    }

    void StartOutro()
    {
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
        PlayVoiceLine(outroTalk);
        yield return new WaitForSeconds(TimeForOutro);

        credits.alpha = 1.0f;
        credits.interactable = true;
        credits.blocksRaycasts = true;
        playerInput.UnlockCursorPermanent();
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
        playerGun.enabled = false;
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

    void PlayVoiceLine(AudioClip voiceLine)
    {
        voiceClipSource.clip = voiceLine;
        voiceClipSource.loop = false;
        voiceClipSource.Play();
    }
}
