using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler {

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
        itemImage.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData) { // 포인터를 뗄때 호출되는 메소드
        
        bool isUse = false;
        
        if(item != null) {
            isUse = item.Use();   
        }

        if(isUse) {
            Inventory.instance.RemoveItem(slotNum);
        }
    }

}