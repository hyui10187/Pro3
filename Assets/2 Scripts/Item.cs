using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Helmet, Necklace, Armor, Weapon, Shield, Gloves, Ring, Boots, Consumables, Quest, Material, Gold, Key }

public enum ItemName {
    반지, 소드, 목걸이, 활, 골드소, 골드중, 골드대, 상점열쇠, 상자열쇠, 빨강물약, 파랑물약, 초록물약, 노랑물약, 보라물약, 
    목재, 돌멩이, 사탕, 은화, 가죽갑옷, 가죽장갑, 가죽모자, 가죽신발, 나무방패,
    붉은과일, 푸른과일, 보라과일, 공백
}

[System.Serializable]
public class Item {

    public ItemType itemType;
    public ItemName itemName;
    public Sprite itemImage;
    public int itemCount;
    public int itemPrice;
    public int itemAttackPower;
    public bool isEquipped;
    public List<ItemEffect> itemEffects;

    public bool Use() {
        bool isUsed = false; // 처음에는 아이템을 사용하지 않은 상태이니 false

        foreach(ItemEffect effect in itemEffects) {
            isUsed = effect.ExecuteRole();
        }
        
        return isUsed;
    }

    public Item Clone() {

        Item copyItem = new Item();
        copyItem.itemType = itemType;
        copyItem.itemName = itemName;
        copyItem.itemImage = itemImage;
        copyItem.itemCount = itemCount;
        copyItem.itemPrice = itemPrice;
        copyItem.itemAttackPower = itemAttackPower;
        copyItem.itemEffects = itemEffects;
        
        return copyItem;
    }
    
}