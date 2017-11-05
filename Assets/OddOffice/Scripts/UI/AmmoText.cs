using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{
    public Gun gun;
    
    public Image image;

    void Start ()
    {
        if (gun == null)
        {
            gun = FindObjectOfType<Gun>();
        }

        gun.OnAmmoChanged.AddListener(() => {
            image.fillAmount = (float)gun.GetAmmo() / gun.maxAmmo;
        });
    }
}
