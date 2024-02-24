using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour, IPointerUpHandler {
    
    public Item item;
    public Image itemImage;

    public void UpdateSlot() {
        itemImage.sprite = item.itemImage;
        itemImage.gameObject.SetActive(true); // 슬롯의 아이템 이미지 켜주기
    }

    public void OnPointerUp(PointerEventData eventData) { // 마우스로 클릭했다가 뗄때 호출되는 메소드

        if(GameManager.instance.curGold < item.itemPrice) {
            AlertManager.instance.CantPurchaseMessageOn(0);
            return;
        }
        
        bool canPurchase = Inventory.instance.AddItem(item); // 아이템을 구매할 수 있는지 = 플레이어의 인벤토리 슬롯이 비어있는지

        if(canPurchase) {
            GameManager.instance.curGold -= item.itemPrice;
            AlertManager.instance.PurchaseMessageOn(item.itemName);
        } else {
            AlertManager.instance.CantPurchaseMessageOn(1);
        }
    }

}