using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    public Health health;
    private Text text;

    void Start()
    {
        if (health == null)
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        text = GetComponent<Text>();

        health.OnHealthChanged.AddListener(() => text.text = Mathf.RoundToInt(health.GetHealth()) + "/" + Mathf.RoundToInt(health.maxHealth));
    }
}
