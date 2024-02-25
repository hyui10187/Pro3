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

    public float clickTime; // 클릭중인 시간
    public float minClickTime; // 롱프레스 여부를 판단하기 위한 기준시간
    public bool isClick;

    private void Update() {
        
        if(isClick) {
            clickTime += Time.deltaTime;
        } else {
            clickTime = 0;
        }
        
        if(minClickTime < clickTime && !GameManager.instance.itemDescriptionPanel.activeSelf) { // 롱프레스일 경우
            string itemDescription = ItemDescription.instance.GetDescription(item.itemName);
            GameManager.instance.itemDescriptionText.text = item.itemName + "\n\n" + itemDescription;
            PanelManager.instance.ItemDescriptionOnOff(slotNum, item);
        }
    }

    public void ButtonDown() {

        if(item != null) {
            isClick = true;
        }
    }

    public void ButtonUp() {
        isClick = false;

        if(minClickTime < clickTime) { // 롱프레스일 경우
            return;

        } else { // 일반 클릭일 경우
            if(GameManager.instance.storagePanel.activeSelf) { // 창고 패널이 켜져있는 상태면

                bool canEntrust = StorageManager.instance.AddItem(item); // 창고에 아이템을 넣어주기

                if(canEntrust && item != null) {
                    Inventory.instance.EntrustItem(slotNum);
                }
                return;
            }

            if(GameManager.instance.storePanel.activeSelf && item != null) { // 상점 패널이 켜져있는 상태라면
                GameManager.instance.curGold += item.itemPrice; // 판매한 아이템의 금액만큼 플레이어의 소지금을 올려주기
                AlertManager.instance.SellMessageOn(item.itemName); // 아이템을 판매하였다는 메시지를 띄워주기
                Inventory.instance.RemoveItem(slotNum);
                return;
            }
        
            bool isUse = false;
        
            if(item != null) {
                isUse = item.Use();   
            }

            if(isUse) {
                AlertManager.instance.ConsumptionMessageOn(item.itemName);
                Inventory.instance.RemoveItem(slotNum);
            }
        }
    }

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

}