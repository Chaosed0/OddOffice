using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Bullet bulletPrefab;
    public Transform muzzlePoint;

    public void Fire()
    {
        Instantiate(bulletPrefab, muzzlePoint.transform.position, muzzlePoint.transform.rotation, null);
    }
}
