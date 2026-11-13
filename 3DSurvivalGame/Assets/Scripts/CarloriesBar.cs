using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarloriesBar : MonoBehaviour
{
    private Slider slider;
    public Text calorisCounter;

    public GameObject playerState;
    private float currentCaloties, maxCalories;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCaloties = playerState.GetComponent<PlayerStatus>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerStatus>().maxCalories;

        float fillvalue = currentCaloties / maxCalories;
        slider.value = fillvalue;
        calorisCounter.text = currentCaloties + " / " + maxCalories;
    }
}
