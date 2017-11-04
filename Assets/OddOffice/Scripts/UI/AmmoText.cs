using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{
    public Gun gun;
    
    private Text text;

    void Start()
    {
        if (gun == null)
        {
            gun = FindObjectOfType<Gun>();
        }

        text = GetComponent<Text>();

        gun.OnAmmoChanged.AddListener(() => text.text = gun.GetAmmo() + "/" + gun.maxAmmo);
    }
}
