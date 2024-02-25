using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour {
    
    public static StorageManager instance;
    
    public StorageSlot[] slots;
    public Transform slotHolder;
    public int maxSlotNum; // 확장할 수 있는 최대 슬롯갯수
    public GameObject slotPrefab;
    
    public delegate void OnchangeItem();
    public OnchangeItem onChangeItem;

    public List<Item> possessItems;
    public int curSlotCnt; // 슬롯의 갯수
    private Inventory inventory;
    
    private void Awake() {
        instance = this;
        MakeSlots();
        onChangeItem += RedrawSlotUI; // onChangeItem 대리자에 RedrawSlotUI 메소드 연결
        RedrawSlotUI();
//        inventory.onSlotCountChange += SlotChange;
    }

    private void MakeSlots() {

        for(int i = 0; i < maxSlotNum; i++) {
            Instantiate(slotPrefab, slotHolder);
        }
        
        slots = slotHolder.GetComponentsInChildren<StorageSlot>();
    }
    
    private void RedrawSlotUI() { // 인벤토리 UI를 다시 그려주는 메소드

        for(int i = 0; i < slots.Length; i++) {
            slots[i].RemoveSlot(); // 처음에는 for 루프를 돌면서 모든 아이템을 다 제거해주기
        }

        for(int i = 0; i < possessItems.Count; i++) {
            slots[i].item = possessItems[i];
            slots[i].UpdateSlot();
        }
    }

    public int CurSlotCnt {
        get => curSlotCnt;
        set {
            curSlotCnt = value;
//            onSlotCountChange.Invoke(slotCnt);
        }
    }

    public bool AddItem(Item eatItem) {

        if(eatItem == null) {
            return false;
        }
        
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

    public void RemoveStorageItem(int index) {
        possessItems.RemoveAt(index);    
        onChangeItem.Invoke();
    }
    
}