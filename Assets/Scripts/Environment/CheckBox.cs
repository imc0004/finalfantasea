﻿using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    WeatherPanel panel;
    Image image;
    [SerializeField] private int day = 0;
    // Start is called before the first frame update
    void Start()
    {
       panel = GetComponentInParent<WeatherPanel>(); 
       image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.dayPassed[day]) {
            if (image.enabled) {
                image.enabled = false;
            }
        } else  {
            if (!image.enabled) {
                image.enabled = true;
            }
        }
    }
}