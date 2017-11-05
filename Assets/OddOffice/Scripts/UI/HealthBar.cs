using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;
    private Slider slider;

    void Start()
    {
        if (health == null)
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        slider = GetComponent<Slider>();

        health.OnHealthChanged.AddListener(() => slider.value = health.GetHealth() / health.maxHealth);
    }
}
