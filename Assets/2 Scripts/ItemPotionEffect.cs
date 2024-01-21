using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/PotionEft")]
public class ItemPotionEffect : ItemEffect {

    public int recoveryHealthPoint; // 회복시킬 HP양
    public int recoveryManaPoint; // 회복시킬 MP양
    public float increaseSpeedPercent; // 증가시킬 이동속도양
    
    public override bool ExecuteRole() {

        GameManager.instance.curHealth += recoveryHealthPoint;
        GameManager.instance.curMana += recoveryManaPoint;

        if(GameManager.instance.curHealth > GameManager.instance.maxHealth) {
            GameManager.instance.curHealth = GameManager.instance.maxHealth;
        }

        if(GameManager.instance.curMana > GameManager.instance.maxMana) {
            GameManager.instance.curMana = GameManager.instance.maxMana;
        }

        if(increaseSpeedPercent > 0) {
            GameManager.instance.SpeedEffectOn();

            GameManager.instance.curMoveSpeed = GameManager.instance.originMoveSpeed; // 물약을 중복으로 먹을 수 있으니 현재 이동속도를 기본 이동속도로 일단 초기화
            GameManager.instance.curMoveSpeed = GameManager.instance.curMoveSpeed + (GameManager.instance.curMoveSpeed * increaseSpeedPercent); // 물약의 이동속도 증가율만쿰 현재 이동속도 증가시킴
        }
        
        return true;
    }

}