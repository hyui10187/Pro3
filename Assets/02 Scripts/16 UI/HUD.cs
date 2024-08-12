using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    
    public enum InfoType { Health, HealthText, Mana, ManaText, Time, Level , Exp, Monster, Gold, LongPressBar }

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

            case InfoType.HealthText:
                uiText.text = GameManager.instance.curHealth + " / " + GameManager.instance.maxHealth;
                break;
            
            case InfoType.ManaText:
                uiText.text = GameManager.instance.curMana + " / " + GameManager.instance.maxMana;
                break;
            
            case InfoType.Mana:
                float curMana = GameManager.instance.curMana;
                float maxMana = GameManager.instance.maxMana;
                slider.value = curMana / maxMana;
                break;

            case InfoType.Time:
                float curTime = GameManager.instance.curGameTime;

                // 시간을 24시간 형식으로 표시
                int hour = Mathf.FloorToInt(curTime / 3600) % 24;
                int min = Mathf.FloorToInt((curTime % 3600) / 60);

                // 날짜와 요일을 계산
                int day = Mathf.FloorToInt(curTime / 86400) + 1; // 하루는 86400초 (24시간 * 60분 * 60초)
                int month = (day - 1) / 30 + 1; // 1달은 30일
                int year = (day - 1) / 365 + 1; // 1년은 365일
                int weekday = (day - 1) % 7; // 요일 (월요일부터 시작)

                // 요일을 문자열로 변환
                string[] weekdays = { "월", "화", "수", "목", "금", "토", "일" };
                string weekdayStr = weekdays[weekday];

                // UI에 시간, 날짜, 요일 표시
                uiText.text = $"{year}년 {month}월 {day}일 {weekdayStr}요일  {hour:D2}시 {min:D2}분";
                break;
            
            case InfoType.Level:
                uiText.text = GameManager.instance.curLevel.ToString();
                break;
            
            case InfoType.Exp:
                float curExp = GameManager.instance.curExp;
                float maxExp = GameManager.instance.maxExp;
                slider.value = curExp / maxExp;
                break;
            
            case InfoType.Monster:
                uiText.text = "X " + SpawnManager.instance.enemyCount;
                break;
            
            case InfoType.Gold:
                uiText.text = GameManager.instance.curGold.ToString();
                break;
            
            case InfoType.LongPressBar:
                if(PanelManager.instance.isStoreSlotClick || Inventory.instance.isInventorySlotClick) {
                    slider.value += Time.deltaTime;
                } else {
                    slider.value = 0;
                }
                break;
        }
    }

}