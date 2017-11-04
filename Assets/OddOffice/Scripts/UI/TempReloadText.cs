using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempReloadText : MonoBehaviour
{
    public Gun gun;

    void Start()
    {
        if (gun == null)
        {
            gun = FindObjectOfType<Gun>();
        }

        gun.OnReloadStart.AddListener(() => this.gameObject.SetActive(true));
        gun.OnReloadEnd.AddListener(() => this.gameObject.SetActive(false));

        this.gameObject.SetActive(false);
    }
}
