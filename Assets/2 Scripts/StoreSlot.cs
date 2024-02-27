using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour {
    
    public Item item;

    public void ButtonDown() { // 마우스로 클릭했다가 뗄때 호출되는 메소드
        if(GameManager.instance.curGold < item.itemPrice) {
            AlertManager.instance.AlertMessageOn("", 7); // 소지금 부족 메시지
            return;
        }
        
        bool canPurchase = Inventory.instance.AddItem(item); // 아이템을 구매할 수 있는지 = 플레이어의 인벤토리 슬롯이 비어있는지

        if(canPurchase) {
            GameManager.instance.curGold -= item.itemPrice;
            AlertManager.instance.AlertMessageOn(item.itemName, 1); // 구매 메시지
        } else {
            AlertManager.instance.AlertMessageOn("", 8); // 인벤토리가 가득차서 구매 불가 메시지
        }
    }

}