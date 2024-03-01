using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour {
    
    public static EquipmentSlot instance;

    public ItemType itemType;
    public Image itemImage;
    public string itemName;

    private void Awake() {
        instance = this;
    }

    public void ButtonDown() { // 장비창의 장착된 아이템 슬롯을 클릭해서 장비를 해제하는 메소드

        for(int i = 0; i < Inventory.instance.possessItems.Count; i++) {
            if(InventoryManager.instance.inventorySlots[i].item.itemName == itemName) {
                InventoryManager.instance.inventorySlots[i].equipImage.SetActive(false); // 장착중이라는 E 아이콘을 꺼주기

                if(itemName == "소드") { // 소드를 해제했으면 equipSword 플래그값 내려주기
                    Inventory.instance.equipSword = false;
                }
                
                RemoveSlot();
                break;
            }
        }
    }

    public void RemoveSlot() {
        itemImage.gameObject.SetActive(false); // 슬롯의 아이템 이미지 꺼주기
        itemName = null; // 아이템 이름 지워주기
    }

    public void RedrawSlot(Item item) {
        itemImage.sprite = item.itemImage;
        itemImage.gameObject.SetActive(true);
        itemName = item.itemName;
    }
    
}