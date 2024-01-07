using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler {

    public int slotNum;
    public Item item;
    public Image itemIcon;

    public void UpdateSlotUI() {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveSlot() {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData) { // 포인터를 뗄때 호출되는 메소드
        bool isUse = item.Use();

        if(isUse) {
            Inventory.instance.RemoveItem(slotNum);
        }
    }

}