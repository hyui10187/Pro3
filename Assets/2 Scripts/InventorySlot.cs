using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public int slotNum; // 슬롯 번호
    public Item item;
    public Image itemImage;
    public Text itemCount; // 보유한 아이템 갯수
    public GameObject equipImage;

    public float clickTime; // 클릭중인 시간
    public float minClickTime; // 롱프레스 여부를 판단하기 위한 기준시간
    public bool isClick;
    
    public Image coolImage;
    private float coolTime = 2;
    private float curTime;
    private bool isCoolEnded = true; // 아이템 사용 쿨타임이 끝났는지 여부

    private void Update() {

        if(!isCoolEnded) {
            CheckCoolTime();   
        }

        if(isClick) {
            clickTime += Time.deltaTime;
        } else {
            clickTime = 0;
        }

        if(0.2f < clickTime && clickTime < 0.3f) { // 0.2초 이상 누르고 있으면 롱프레스바 띄워주기
            if(Input.GetMouseButton(0)) { // 마우스 왼쪽 버튼을 클릭중이라면
                GameManager.instance.longPressBar.transform.position = Input.mousePosition; // 롱프레스바의 위치를 클릭한 마우스 위치로 옮겨주기

            } else if(0 < Input.touchCount) { // 모바일에서 터치 중이라면
                Vector3 touchPosition = Input.GetTouch(0).position;
                touchPosition.z = -Camera.main.transform.position.z; // 카메라가 위치한 z 좌표로 설정
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

    private void CheckCoolTime() {
        curTime += Time.deltaTime;
        if(curTime < coolTime) {
            SetFillAmount(curTime);
        } else {
            EndCoolTime();
        }
    }

    private void EndCoolTime() {
        SetFillAmount(0); // 쿨타임 이미지의 값을 0으로 초기화해서 안보이도록 해주기
        isCoolEnded = true;
    }

    private void UseItem() {
        if(isCoolEnded) {
            ResetCoolTime();
        }
    }

    private void ResetCoolTime() {
        curTime = 0;
        isCoolEnded = false;
    }
    
    private void SetFillAmount(float value) { // 쿨타임 이미지를 시계방향으로 회전하면서 채워주는 메소드
        coolImage.fillAmount = value / coolTime;
    }
    
    public void ButtonDown() {

        if(item != null) {
            isClick = true;
            Inventory.instance.isInventorySlotClick = true;
        }
    }

    public void ButtonUp() {
        isClick = false;
        Inventory.instance.isInventorySlotClick = false;
        GameManager.instance.longPressBarBackground.SetActive(false);
        GameManager.instance.longPressBarFillArea.SetActive(false); // 롱프레스바를 꺼주기

        if(minClickTime < clickTime) { // 롱프레스일 경우
            return;

        } else { // 일반 클릭일 경우
            if(GameManager.instance.storagePanel.activeSelf) { // 창고 패널이 켜져있는 상태면

                if(1 < item.itemCount) { // 아이템의 갯수가 2개 이상이면
                    PanelManager.instance.LeaveAmountPanelOnOff(); // 맡기는 갯수 설정하는 패널 띄워주기
                    return;
                }
                
                PanelManager.instance.LeaveButtonClick();
                return;
            }

            if(GameManager.instance.groceryStorePanel.activeSelf && item != null) { // 상점 패널이 켜져있는 상태라면

                if(item.isEquipped) { // 장착중인 아이템을 상점에 판매하려고 할 경우
                    AlertManager.instance.BigAlertMessageOn("", 14);
                    return;
                }

                if(item.itemType == ItemType.Quest) { // 퀘스트 아이템을 상점에 판매하려고 할 경우
                    AlertManager.instance.BigAlertMessageOn("", 16);
                    return;
                }
                
                GameManager.instance.curGold += item.itemPrice; // 판매한 아이템의 금액만큼 플레이어의 소지금을 올려주기
                AlertManager.instance.SmallAlertMessageOn(item.itemName, 2); // 아이템을 판매하였다는 메시지를 띄워주기
                Inventory.instance.RemoveItem(slotNum);
                return;
            }

            if(!isCoolEnded) { // curTime이 0이 아니라면 현재 쿨타임이 진행중인 상황인 것이니 아이템이 먹어지지 않도록 돌려보내기
                return;
            }
            
            bool isUse = false;
        
            if(item != null) {
                isUse = item.Use();   
            }

            if(isUse) {
                UseItem();
                AlertManager.instance.SmallAlertMessageOn(item.itemName, 4); // 소비 메시지
                Inventory.instance.RemoveItem(slotNum);
            }
        }
    }

    public void UpdateSlot() {
        itemImage.sprite = item.itemImage;
        itemImage.gameObject.SetActive(true); // 슬롯의 아이템 이미지 켜주기
        equipImage.SetActive(item.isEquipped); // 장착 되었는지 여부 켜주거나 꺼주거나

        if(item.itemCount != 0) {
            itemCount.text = item.itemCount.ToString();
            itemCount.gameObject.SetActive(true); // 슬롯의 아이템 갯수 켜주기   
        }
    }

    public void RemoveSlot() {
        item = null;
        itemImage.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
        equipImage.gameObject.SetActive(false);
    }

}