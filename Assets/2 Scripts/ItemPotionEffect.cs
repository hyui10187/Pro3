using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/PotionEft")]
public class ItemPotionEffect : ItemEffect {

    public int recoveryHealthPoint;
    public int recoveryManaPoint;
    
    public override bool ExecuteRole() {

        GameManager.instance.curHealth += recoveryHealthPoint;
        GameManager.instance.curMana += recoveryManaPoint;

        if(GameManager.instance.curHealth > GameManager.instance.maxHealth) {
            GameManager.instance.curHealth = GameManager.instance.maxHealth;
        }

        if(GameManager.instance.curMana > GameManager.instance.maxMana) {
            GameManager.instance.curMana = GameManager.instance.maxMana;
        }
        
        return true;
    }

}