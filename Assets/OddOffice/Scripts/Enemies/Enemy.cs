using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Health health;
    
    private void Start()
    {
        health = GetComponent<Health>();

        health.OnDied.AddListener(() => Destroy(this.gameObject));
    }
}
