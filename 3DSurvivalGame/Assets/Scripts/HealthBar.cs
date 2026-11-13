using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public Text healthCounter;

    public GameObject playerState;
    private float currentHealth , maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerStatus>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerStatus>().maxHealth;

        float fillvalue = currentHealth / maxHealth;
        slider.value = fillvalue;
        healthCounter.text = currentHealth + " / " + maxHealth;
    }
}
