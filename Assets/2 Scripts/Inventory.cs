using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory instance;
//    public delegate void OnSlotCountChange(int val);
//    public OnSlotCountChange onSlotCountChange;

    public delegate void OnchangeItem();
    public OnchangeItem onChangeItem;

    public List<Item> possessItems;
    public int curSlotCnt; // 슬롯의 갯수

    public int CurSlotCnt {
        get => curSlotCnt;
        set {
            curSlotCnt = value;
//            onSlotCountChange.Invoke(slotCnt);
        }
    }
    
    private void Awake() {
        instance = this;
    }

    public bool AddItem(Item eatItem) {

        bool isAdded = false;
        int index = -1;
        
        if(possessItems.Count < CurSlotCnt) { // 현재 보유 슬롯보다 현재 보유중인 아이템의 갯수가 적으면
            if(possessItems.Count > 0) {
                for(int i = 0; i < possessItems.Count; i++) {
                    if(possessItems[i].itemName == eatItem.itemName) {
                        isAdded = true;
                        index = i;
                        break;
                    }
                }

                if(isAdded) { // 기존에 가지고 있눈 아이템일 경우
                    possessItems[index].itemCount++; // 기존 아이템의 갯수만 1개 늘려줌
                    
                } else { // 기존에 가지고 있지 않은 아이템일 경우
                    possessItems.Add(eatItem);
                }

            } else {
                possessItems.Add(eatItem); // 새롭게 아이템을 추가해줌
            }

            if(onChangeItem != null) {
                onChangeItem.Invoke(); // 인벤토리 다시 그려주기
            }
            
            return true; // 아이템 슬롯의 갯수에 여유가 있으면 아이템을 먹어서 인벤토리에 추가해주고 true 반환
            
        } else { // 현재 보유 슬롯과 현재 보유중인 아이템의 갯수가 같으면
            for(int i = 0; i < possessItems.Count; i++) {
                if(possessItems[i].itemName == eatItem.itemName) {
                    isAdded = true;
                    index = i;
                    break;
                }
            }

            if(isAdded) { // 기존에 가지고 있눈 아이템일 경우
                possessItems[index].itemCount++; // 기존 아이템의 갯수만 1개 늘려줌
                if(onChangeItem != null) {
                    onChangeItem.Invoke();   
                }
                return true;
            } else {
                return false; // 아이템 슬롯에 여유가 없으면 아이템을 그대로 두고 false 반환
            }
        }
    }

    public void RemoveItem(int index) {

        if(possessItems[index].itemCount > 1) {
            possessItems[index].itemCount--;
        } else {
            possessItems.RemoveAt(index); // 리스트에서 삭제할때는 RemoveAt 메소드 사용    
        }
        
        onChangeItem.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("FieldItem")) {
            FieldItems fieldItems = other.GetComponent<FieldItems>();
            bool canEat = AddItem(fieldItems.GetItem()); // 아이템을 먹을 수 있는지 판단하는 플래그값

            if(canEat) { // 아이템을 먹을 수 있는 조건이 충족되면(슬롯의 갯수가 남아있거나 슬롯이 갯수가 꽉 차 있더라도 기존에 보유한 아이템의 갯수를 늘릴 수 있으면)
                fieldItems.gameObject.SetActive(false); // 필드에 떨어져 있는 아이템을 먹었으면 해당 아이템은 꺼줘서 안보이게 하기

            } else { // 인벤토리가 꽉 차있다면
                ItemMessageOn();
            }
        }
    }

    public void ItemMessageOn() {
        GameManager.instance.itemMessage.SetActive(true);
        CancelInvoke(); // 우선 현재 호출중인 모든 Invoke 메소드 취소
        Invoke("ItemMessageOff", 2f); // 2초 뒤에 아이템을 먹을 수 없다는 알림 꺼주기
    }
    
    private void ItemMessageOff() {
        GameManager.instance.itemMessage.SetActive(false);
    }
    
}