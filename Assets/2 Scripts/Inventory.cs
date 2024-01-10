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

    private bool AddItem(Item eatItem) {
        
        if(possessItems.Count < CurSlotCnt) {
            possessItems.Add(eatItem);

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
                fieldItems.gameObject.SetActive(false); // 필드에 떨어져 있는 아이템을 먹었으면 해당 아이템은 꺼줘서 안보이게 하기
            }
        }
    }
    
}