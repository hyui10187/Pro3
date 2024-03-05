using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    public ItemType itemType;
    public string itemName;
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