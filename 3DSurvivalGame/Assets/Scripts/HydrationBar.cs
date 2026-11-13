using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public Text calorisCounter;

    public GameObject playerState;
    private float currentHydration, maxHydration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerStatus>().currentHydrationPercent;
        maxHydration = playerState.GetComponent<PlayerStatus>().maxHydrationPercent;

        float fillvalue = currentHydration / maxHydration;
        slider.value = fillvalue;
        calorisCounter.text = currentHydration + "%";
    }
}

