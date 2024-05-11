using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    public static WeatherManager instance;

    public GameObject snow;
    
    private void Awake() {
        instance = this;
    }

    public void SnowOn() {
        snow.SetActive(true);
    }

    public void SnowOff() {
        snow.SetActive(false);
    }
    
}