using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public Bullet bulletPrefab;
    public Transform muzzlePoint;
    public int maxAmmo = 30;
    public float reloadTime = 3.0f;

    public UnityEvent OnReloadStart = new UnityEvent();
    public UnityEvent OnReloadEnd = new UnityEvent();
    public UnityEvent OnFireSuccess = new UnityEvent();
    public UnityEvent OnFireFail = new UnityEvent();
    public UnityEvent OnAmmoChanged = new UnityEvent();

    private int _ammo = 0;
    private bool _reloading = false;

    void Awake()
    {
        _ammo = maxAmmo;
    }

    public void Reload()
    {
        SetReloading(true);
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        SetAmmo(maxAmmo);
        SetReloading(false);
    }

    public void Fire()
    {
        if (_ammo > 0)
        {
            Instantiate(bulletPrefab, muzzlePoint.transform.position, muzzlePoint.transform.rotation, null);
            SetAmmo(_ammo -1);
            OnFireSuccess.Invoke();
        }
        else if (!_reloading)
        {
            OnFireFail.Invoke();
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
}
