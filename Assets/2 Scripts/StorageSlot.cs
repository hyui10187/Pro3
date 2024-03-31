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

        if(1 < item.itemCount) {
            PanelManager.instance.WithdrawAmountPanelOnOff(item);
            return;
        }
        
        PanelManager.instance.WithdrawButtonClick(item);
    }

}