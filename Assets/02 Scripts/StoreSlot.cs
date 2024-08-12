using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour {
    
    public int slotNum;
    public Item item;

    public float clickTime; // 클릭중인 시간
    public float minClickTime; // 롱프레스 여부를 판단하기 위한 기준시간
    public bool isStoreSlotClick; // 잡화상점 클릭 플래그

    private void Update() {

        if(isStoreSlotClick) {
            clickTime += Time.deltaTime;
        } else {
            clickTime = 0;
        }
        
        if(0.2f < clickTime && clickTime < 0.3f) { // 0.2초 이상 누르고 있으면 롱프레스바 띄워주기
            if(Input.GetMouseButton(0)) { // 마우스 왼쪽 버튼을 클릭중이라면
                GameManager.instance.longPressBar.transform.position = Input.mousePosition; // 롱프레스바의 위치를 클릭한 마우스 위치로 옮겨주기

            } else if(0 < Input.touchCount) { // 모바일에서 터치 중이라면
                Vector3 touchPosition = Input.GetTouch(0).position;
                GameManager.instance.longPressBar.transform.position = Camera.main.ScreenToWorldPoint(touchPosition + Vector3.up * 20);
            }
            
            GameManager.instance.longPressBarBackground.SetActive(true);
            GameManager.instance.longPressBarFillArea.SetActive(true);
        }
        
        if(minClickTime < clickTime && !GameManager.instance.itemDescriptionPanel.activeSelf) { // 롱프레스를 1초 이상 했을 경우
            GameManager.instance.longPressBarBackground.SetActive(false);
            GameManager.instance.longPressBarFillArea.SetActive(false); // 롱프레스바를 꺼주기

            string itemDescription = ItemDescription.instance.GetDescription(item.itemName);
            GameManager.instance.itemDescriptionText.text = item.itemName + "\n\n" + itemDescription;
            PanelManager.instance.ItemDescriptionOnOff(slotNum, item); // 아이템 설명창 띄워주기
        }
    }

    public void StoreSlotButtonDown() { // 상점의 슬롯을 클릭했을때 호출되는 메소드
        isStoreSlotClick = true;
        PanelManager.instance.isStoreSlotClick = true;
    }
    
    public void GroceryStoreSlotButtonUp() { // 잡화상점의 슬롯을 클릭했다가 떼었을때 호출되는 메소드
        isStoreSlotClick = false;
        PanelManager.instance.isStoreSlotClick = false;
        
        if(minClickTime < clickTime) { // 롱프레스일 경우
            return;
        } else {
            SoundManager.instance.PlaySound(AudioClipName.Plus);
            PanelManager.instance.PurchaseAmountPanelOn(item);   
        }
    }

    public void EquipmentStoreSlotButtonUp() { // 장비상점의 슬롯을 클릭했다가 떼었을때 호출되는 메소드
        isStoreSlotClick = false;
        PanelManager.instance.isStoreSlotClick = false;
        
        if(minClickTime < clickTime) { // 롱프레스일 경우
            return;
        } else {
            SoundManager.instance.PlaySound(AudioClipName.Plus);
            PanelManager.instance.PurchaseConfirmPanelOn(item);   
        }
    }

}