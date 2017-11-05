using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public AudioSource ringSource;
    public Animator animator;


    public void StartRinging()
    {
        ringSource.Play();
        animator.SetBool("isRinging", true);
    }

    public void StopRinging()
    {
        ringSource.Stop();
        animator.SetBool("isRinging", false);
    }
}
