using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour {
    
    public static StorageManager instance;
    
    public StorageSlot[] storageSlots;
    public Transform slotHolder;
    public int storageSlotCount; // 창고 슬롯의 갯수
    public GameObject slotPrefab;
    
    public delegate void OnchangeItem();
    public OnchangeItem onChangeItem;

    public List<Item> possessItems;
    private Inventory inventory;
    
    private void Awake() {
        MakeSlots();
        RedrawStorageSlots();
        onChangeItem += RedrawStorageSlots; // onChangeItem 대리자에 RedrawStorageSlots 메소드 연결
        instance = this;
    }

    private void MakeSlots() { // 창고 슬롯을 만들어내는 메소드

        for(int i = 0; i < storageSlotCount; i++) {
            Instantiate(slotPrefab, slotHolder);
        }
        
        storageSlots = slotHolder.GetComponentsInChildren<StorageSlot>();
    }
    
    private void RedrawStorageSlots() { // 창고 슬롯들을 다시 그려주는 메소드

        for(int i = 0; i < storageSlots.Length; i++) {
            storageSlots[i].RemoveSlot(); // 처음에는 for 루프를 돌면서 창고 슬롯의 item 변수를 비워주기
            storageSlots[i].slotNum = i; // 슬롯 번호를 순서대로 부여하기
        }

        for(int i = 0; i < possessItems.Count; i++) {
            storageSlots[i].item = possessItems[i];
            storageSlots[i].UpdateSlot();
        }
    }

    public bool AddItem(Item eatItem, int leaveAmount) { // 창고 슬롯에 아이템을 추가해주는 메소드

        if(eatItem == null) {
            return false;
        }
        
        bool isAdded = false;
        int index = -1;
        
        Debug.Log("possessItems.Count: " + possessItems.Count);
        
        if(possessItems.Count < storageSlotCount) { // 현재 보유 슬롯보다 현재 보유중인 아이템의 갯수가 적으면
            if(possessItems.Count > 0) {
                for(int i = 0; i < possessItems.Count; i++) {
                    if(possessItems[i].itemName == eatItem.itemName) {
                        isAdded = true;
                        index = i;
                        break;
                    }
                }
                
                Debug.Log("isAdded: " + isAdded);
                Debug.Log("index: " + index);
                
                if(isAdded) { // 기존에 가지고 있는 아이템일 경우
                    possessItems[index].itemCount += leaveAmount; // 기존 아이템의 갯수만 1개 늘려줌
                    
                } else { // 기존에 가지고 있지 않은 아이템일 경우
                    possessItems.Add(eatItem.Clone());
                }

            } else {
                
                Debug.Log("아이템 추가되는 else");
                possessItems.Add(eatItem.Clone()); // 새롭게 아이템을 추가해줌
            }

            if(onChangeItem != null) {
                onChangeItem.Invoke(); // 창고 슬롯 다시 그려주기
            }
            
            return true; // 창고 슬롯의 갯수에 여유가 있으면 창고 슬롯에 아이템을 추가해주고 true 반환
            
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
                return false; // 창고 슬롯이 꽉 찼으면 아이템을 그대로 두고 false 반환
            }
        }
    }

    public void RemoveStorageItem(int index) {
        possessItems.RemoveAt(index);    
        onChangeItem.Invoke();
    }
    
}