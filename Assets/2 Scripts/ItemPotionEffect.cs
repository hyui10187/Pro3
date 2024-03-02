using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/PotionEft")]
public class ItemPotionEffect : ItemEffect {

    public int recoveryHealthPoint; // 회복시킬 HP양
    public int recoveryManaPoint; // 회복시킬 MP양
    public int increaseSpeedPoint; // 증가시킬 이동속도양
    
    public override bool ExecuteRole() {

        if(0 < recoveryHealthPoint) { // 빨강물약이면 + 되는 체력 표기
            GameManager.instance.healthManaMessageText.text = "+" + recoveryHealthPoint;
            GameManager.instance.curHealth += recoveryHealthPoint;
            AlertManager.instance.HealthMessageOn();
        } else if(recoveryHealthPoint < 0) { // 초록물약이면 - 되는 체력 표기
            GameManager.instance.healthManaMessageText.text = recoveryHealthPoint.ToString();
            GameManager.instance.curHealth += recoveryHealthPoint;
            AlertManager.instance.HealthMessageOn();
        }

        if(0 < recoveryManaPoint) { // 파랑물약이면 + 되는 마나 표기
            GameManager.instance.healthManaMessageText.text = "+" + recoveryManaPoint;
            GameManager.instance.curMana += recoveryManaPoint;
            AlertManager.instance.ManaMessageOn();
        }
        
        if(GameManager.instance.maxHealth < GameManager.instance.curHealth) {
            GameManager.instance.curHealth = GameManager.instance.maxHealth;
        } else if(GameManager.instance.curHealth < 0) {
            GameManager.instance.curHealth = 0;
        }

        if(GameManager.instance.maxMana < GameManager.instance.curMana) {
            GameManager.instance.curMana = GameManager.instance.maxMana;
        }

        if(0 < increaseSpeedPoint) { // 노랑 물약이면
            GameManager.instance.curMoveSpeed = GameManager.instance.originMoveSpeed; // 노랑물약을 중복으로 먹을 수 있으니 현재 이동속도를 기본 이동속도로 일단 초기화
            GameManager.instance.curMoveSpeed += increaseSpeedPoint; // 노랑물약의 이동속도 증가율만쿰 현재 이동속도 증가시킴
            GameManager.instance.itemMoveSpeed = increaseSpeedPoint;
            PanelManager.instance.SpeedEffectOn();
        }
        
        return true;
    }

}