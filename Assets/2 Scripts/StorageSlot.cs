using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageSlot : MonoBehaviour {
    
    public int slotNum; // 슬롯 번호
    public Item item;
    public Image itemImage; // 아이템 이미지
    public Text itemCount; // 보유한 아이템 갯수
    
    public void UpdateSlot() {
        itemImage.sprite = item.itemImage;
        itemImage.gameObject.SetActive(true); // 슬롯의 아이템 이미지 켜주기

        if(item.itemCount != 0) { // 아이템의 갯수가 0개가 아니면 = 장비 아이템이면
            itemCount.text = item.itemCount.ToString();
            itemCount.gameObject.SetActive(true); // 슬롯의 아이템 갯수 켜주기   
        }
    }

    public void RemoveSlot() {
        item = null;
        
        if(itemImage != null) {
            itemImage.gameObject.SetActive(false);
            itemCount.gameObject.SetActive(false);
        }
    }

    public void ButtonDown() {
        bool canWithdraw = Inventory.instance.AddItem(item); // 아이템을 찾을 수 있는지 없는지 = 플레이어의 인벤토리 슬롯이 비어있는지
        
        if(canWithdraw) {
            
            if(item.itemType == ItemType.Quest) {
                QuestManager.instance.questActionIndex++;
            }

            ItemName itemName = item.itemName;
            string itemNameStr = itemName.ToString();

            AlertManager.instance.SmallAlertMessageOn(itemNameStr, 10);
            StorageManager.instance.RemoveStorageItem(slotNum);
        }
    }

}