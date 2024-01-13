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
        
        Debug.Log("Add Item 실행............................");
        
        if(possessItems.Count < CurSlotCnt) {

            Debug.Log("첫번째 if문 진입......................");
            
            if(possessItems.Count > 0) {
                
                Debug.Log("두번째 if문 진입..........................");
                
                for(int i = 0; i < possessItems.Count; i++) {

                    if(possessItems[i].itemName == eatItem.itemName) {
                        // 기존에 가지고 있눈 아이템일 경우
                        possessItems[i].itemCount++; // 기존 아이템의 갯수만 1개 늘려줌
                        
                    } else { // 기존에 가지고 있지 않은 아이템일 경우
                        possessItems.Add(eatItem); // 새롭게 아이템을 추가해줌
                    }
                }
            } else {

                Debug.Log("else if문 진입.......................");
                
                possessItems.Add(eatItem); // 새롭게 아이템을 추가해줌
            }

            if(onChangeItem != null) {
                onChangeItem.Invoke();   
            }
            
            return true; // 아이템 슬롯의 갯수에 여유가 있으면 아이템을 먹어서 인벤토리에 추가해주고 true 반환
        }

        return false; // 아이템 슬롯에 여유가 없으면 아이템을 그대로 두고 false 반환
    }

    public void RemoveItem(int index) {
        possessItems.RemoveAt(index); // 리스트에서 삭제할때는 RemoveAt 메소드 사용
        onChangeItem.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("FieldItem")) {
            FieldItems fieldItems = other.GetComponent<FieldItems>();

            if(AddItem(fieldItems.GetItem())) {
                
                Debug.Log("Field Item 비활성화........................");
                
                fieldItems.gameObject.SetActive(false); // 필드에 떨어져 있는 아이템을 먹었으면 해당 아이템은 꺼줘서 안보이게 하기
            }
        }
    }
    
}