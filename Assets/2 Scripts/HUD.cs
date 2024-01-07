using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    
    public enum InfoType { Health, Mana, Exp }

    public InfoType type;

    private Text text;
    private Slider slider;

    private void Awake() {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }

    private void LateUpdate() {

        switch(type) {
            
            case InfoType.Health:
                float curHealth = GameManager.instance.curHealth;
                float maxHealth = GameManager.instance.maxHealth;
                slider.value = curHealth / maxHealth;
                break;
            
            case InfoType.Mana:
                float curMana = GameManager.instance.curMana;
                float maxMana = GameManager.instance.maxMana;
                slider.value = curMana / maxMana;
                break;
            
            case InfoType.Exp:
                break;
        }
    }
    
}