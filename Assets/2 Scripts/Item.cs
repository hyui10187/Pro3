using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    
    public enum ItemType { Equipment, Consumables, Quest, Etc }

    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public int itemCount;
    public List<ItemEffect> itemEffects;

    public bool Use() {
        bool isUsed = false; // 처음에는 아이템을 사용하지 않은 상태이니 false

        foreach(ItemEffect effect in itemEffects) {
            isUsed = effect.ExecuteRole();
        }
        
        return isUsed;
    }
    
}