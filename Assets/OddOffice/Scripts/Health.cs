﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 50.0f;
    private float health;
    private bool isDead = false;

    public UnityEvent OnDied = new UnityEvent();
    public UnityEvent OnHealthChanged = new UnityEvent();

    void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        OnHealthChanged.Invoke();

        if (health <= 0 && !isDead)
        {
            isDead = true;
            OnDied.Invoke();
        }
    }

    public float GetHealth()
    {
        return health;
    }
}
