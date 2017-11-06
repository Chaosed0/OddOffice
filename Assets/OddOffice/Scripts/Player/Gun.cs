using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public Bullet bulletPrefab;
    public Transform muzzlePoint;
    public int maxAmmo = 30;
    public float reloadTime = 3.0f;
    public AudioSource bulletAudioSource;

    public UnityEvent OnReloadStart = new UnityEvent();
    public UnityEvent OnReloadEnd = new UnityEvent();
    public UnityEvent OnFireSuccess = new UnityEvent();
    public UnityEvent OnFireFail = new UnityEvent();
    public UnityEvent OnAmmoChanged = new UnityEvent();

    public AudioClip shootClip;
    public AudioClip shootFailClip;
    public AudioClip reloadClip;

    private int _ammo = 0;
    private bool _reloading = false;
    private Image crosshair;

    void Awake()
    {
        _ammo = maxAmmo;
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        crosshair.enabled = false;
    }

    public void Reload()
    {
        if (this.enabled)
        {
            SetReloading(true);
            bulletAudioSource.clip = reloadClip;
            bulletAudioSource.loop = false;
            bulletAudioSource.Play();
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        SetAmmo(maxAmmo);
        SetReloading(false);
    }

    public void Fire()
    {
        if (!_reloading && this.enabled)
        {
            if (_ammo > 0)
            {
                Instantiate(bulletPrefab, muzzlePoint.transform.position, muzzlePoint.transform.rotation, null);
                bulletAudioSource.clip = shootClip;
                bulletAudioSource.loop = false;
                bulletAudioSource.Play();
                SetAmmo(_ammo -1);
                OnFireSuccess.Invoke();
            }
            else
            {
                bulletAudioSource.clip = shootFailClip;
                bulletAudioSource.loop = false;
                bulletAudioSource.Play();
                OnFireFail.Invoke();
            }
        }
    }

    private void SetAmmo(int ammo)
    {
        _ammo = ammo;
        OnAmmoChanged.Invoke();
    }

    private void SetReloading(bool reloading)
    {
        _reloading = reloading;
        if (_reloading)
        {
            OnReloadStart.Invoke();
        }
        else
        {
            OnReloadEnd.Invoke();
        }
    }

    public int GetAmmo()
    {
        return _ammo;
    }

    void OnEnable()
    {
        crosshair.enabled = true;
    }

    void OnDisable()
    {
        crosshair.enabled = false;
    }
}
