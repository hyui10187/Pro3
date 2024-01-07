using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public GameObject inventoryPanel;
    public Slot[] slots;
    public Transform slotHolder;
    public bool isActive;

    private Inventory inventory;
    
    private void Awake() {
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory = Inventory.instance;
        SlotChange();
        inventory.onChangeItem += RedrawSlotUI; // onChangeItem 대리자에 RedrawSlotUI 메소드 연결
//        inventory.onSlotCountChange += SlotChange;
    }

    private void RedrawSlotUI() {

        for(int i = 0; i < slots.Length; i++) {
            slots[i].RemoveSlot(); // 처음에는 for 루프를 돌면서 모든 아이템 슬롯을 다 꺼주고
        }

        for(int i = 0; i < inventory.possessItems.Count; i++) {
            slots[i].item = inventory.possessItems[i];
            slots[i].UpdateSlotUI();
        }
    }

    private void SlotChange() {

        for(int i = 0; i < slots.Length; i++) {

            slots[i].slotNum = i;
            
            if(i < inventory.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void AddSlot() {
        inventory.SlotCnt++;
    }
    
    public void OnOffInventory() {
        isActive = !isActive; // 메소드가 실행될때마다 기존에 가지고 있던 플래그 값을 반전시켜줌
        inventoryPanel.SetActive(isActive); // 플래그 값에 따라 인벤토리 패널을 켜거나 꺼줌
    }
    
}