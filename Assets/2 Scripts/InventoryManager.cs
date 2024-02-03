using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance;
    
    public GameObject inventoryPanel;
    public Slot[] slots;
    public Transform slotHolder;
    public bool isActive;
    public int maxSlotNum; // 확장할 수 있는 최대 슬롯갯수
    public GameObject slotPrefab;

    private Inventory inventory;
    
    private void Awake() {
        MakeSlots();
        inventory = Inventory.instance;
        SlotChange();
        inventory.onChangeItem += RedrawSlotUI; // onChangeItem 대리자에 RedrawSlotUI 메소드 연결
//        inventory.onSlotCountChange += SlotChange;
    }

    private void MakeSlots() {

        for(int i = 0; i < maxSlotNum; i++) {
            Instantiate(slotPrefab, slotHolder);
        }

        slots = slotHolder.GetComponentsInChildren<Slot>();
    }
    
    private void RedrawSlotUI() { // 인벤토리 UI를 다시 그려주는 메소드

        for(int i = 0; i < slots.Length; i++) {
            slots[i].RemoveSlot(); // 처음에는 for 루프를 돌면서 모든 아이템 슬롯을 다 꺼주고
        }

        for(int i = 0; i < inventory.possessItems.Count; i++) {
            slots[i].item = inventory.possessItems[i];
            slots[i].UpdateSlot();
        }
    }

    private void SlotChange() { // 슬롯을 켜거나 꺼주는 메소드

        for(int i = 0; i < slots.Length; i++) { // 슬롯 전체 길이만큼 루프를 돈다

            slots[i].slotNum = i;
            
            if(i < inventory.CurSlotCnt) // SlotCnt의 get을 통해 현재 슬롯만큼만 가져와서 그 숫자만큼만 버튼을 활성화하고 나머지는 전부 비활성화
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void AddSlot() {
        inventory.CurSlotCnt++;
    }
    
    public void OnOffInventory() {
        isActive = !isActive; // 메소드가 실행될때마다 기존에 가지고 있던 플래그 값을 반전시켜줌
        inventoryPanel.SetActive(isActive); // 플래그 값에 따라 인벤토리 패널을 켜거나 꺼줌
    }

    public void CleanInventory() { // 죽었을때 실행해주는 메소드
        inventoryPanel.SetActive(false); // 혹시 인벤토리가 켜져 있는 상태로 죽었으면 꺼주기
    }
    
}