using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageSlot : MonoBehaviour, IPointerUpHandler {
    
    public int slotNum; // 슬롯 번호
    public Item item;
    public Image itemImage;
    public Text itemCount; // 보유한 아이템 갯수
    
    public void UpdateSlot() {
        itemImage.sprite = item.itemImage;
        itemImage.gameObject.SetActive(true); // 슬롯의 아이템 이미지 켜주기
        itemCount.text = item.itemCount.ToString();
        itemCount.gameObject.SetActive(true); // 슬롯의 아이템 갯수 켜주기
    }

    public void RemoveSlot() {
        item = null;
        
        if(itemImage != null) {
            itemImage.gameObject.SetActive(false);
            itemCount.gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData) { // 마우스로 클릭했다가 뗄때 호출되는 메소드

        bool canWithdraw = Inventory.instance.AddItem(item); // 아이템을 찾을 수 있는지 없는지 = 플레이어의 인벤토리 슬롯이 비어있는지
        
        if(canWithdraw) {
            StorageManager.instance.RemoveStorageItem(slotNum);
        }
    }

}