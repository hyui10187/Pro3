using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public Transform inventorySlotHolder;
    public InventorySlot[] inventorySlots;
    public GameObject inventorySlotPrefab;
    public int maxSlotNum; // 확장할 수 있는 최대 슬롯갯수

    public Inventory inventory;
    
    private void Awake()
    {
        instance = this;
        MakeSlots();
        inventory = Inventory.instance;
        RedrawSlotUI();
        SlotChange();
        inventory.onChangeItem += RedrawSlotUI; // onChangeItem 대리자에 RedrawSlotUI 메소드 연결
//        inventory.onSlotCountChange += SlotChange;
    }

    private void MakeSlots()
    {
        for(int i = 0; i < maxSlotNum; i++)
            Instantiate(inventorySlotPrefab, inventorySlotHolder);

        inventorySlots = inventorySlotHolder.GetComponentsInChildren<InventorySlot>();
    }
    
    private void RedrawSlotUI() // 인벤토리 UI를 다시 그려주는 메소드
    {
        for(int i = 0; i < inventorySlots.Length; i++)
            inventorySlots[i].RemoveSlot(); // 처음에는 for 루프를 돌면서 모든 아이템 슬롯을 다 꺼주고

        for(int i = 0; i < inventory.possessItems.Count; i++)
        {
            inventorySlots[i].item = inventory.possessItems[i];
            inventorySlots[i].UpdateSlot();
        }
    }

    private void SlotChange() // 슬롯을 켜거나 꺼주는 메소드
    {
        for(int i = 0; i < inventorySlots.Length; i++) // 슬롯 전체 길이만큼 루프를 돈다
        {
            inventorySlots[i].slotNum = i;
            
            if(i < inventory.CurSlotCnt) // SlotCnt의 get을 통해 현재 슬롯만큼만 가져와서 그 숫자만큼만 버튼을 활성화하고 나머지는 전부 비활성화
                inventorySlots[i].GetComponent<Button>().interactable = true;
            else
                inventorySlots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void AddSlot()
    {
        inventory.CurSlotCnt++;
    }

}