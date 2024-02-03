using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    
    public enum InfoType { Health, Mana, Time, Level , Exp, Monster }

    public InfoType type;

    private Text uiText;
    private Slider slider;

    private void Awake() {
        uiText = GetComponent<Text>();
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

            case InfoType.Time:
                float curTime = GameManager.instance.curGameTime;
                int hour = Mathf.FloorToInt(curTime / 3600) % 24; // 시간을 24시간 형식으로 표현
                int min = Mathf.FloorToInt((curTime % 3600) / 60); // 분
                uiText.text = string.Format("{0:D2}:{1:D2}", hour, min);
                break;
            
            case InfoType.Level:
                uiText.text = GameManager.instance.level.ToString();
                break;
            
            case InfoType.Exp:
                float curExp = GameManager.instance.curExp;
                float maxExp = GameManager.instance.maxExp;
                slider.value = curExp / maxExp;
                break;
            
            case InfoType.Monster:
                uiText.text = "X " + SpawnManager.instance.enemyCount;
                break;
        }
    }
    
}