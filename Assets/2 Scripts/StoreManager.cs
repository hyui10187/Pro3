using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour {

    public static StoreManager instance;
    
    public StoreSlot[] storeSlots;
    public Transform slotHolder;
    public int slotNum; // 상점 슬롯의 갯수
    public GameObject slotPrefab;
    private Inventory inventory;
    
    private void Awake() {
        instance = this;
        MakeSlots();
        Invoke("RedrawSlotUI", 1f); // ItemManager가 로딩될 수 있도록 1초 기다렸다가 호출하기
    }

    private void MakeSlots() { // 상점 패널의 슬롯들을 만들어주는 메소드

        for(int i = 0; i < slotNum; i++) {
            Instantiate(slotPrefab, slotHolder);
        }
        
        storeSlots = slotHolder.GetComponentsInChildren<StoreSlot>();
    }
    
    private void RedrawSlotUI() { // 상점 패널의 UI를 다시 그려주는 메소드
        
        for(int i = 0; i < slotNum; i++) {
            storeSlots[i].item = ItemManager.instance.itemDB[i];
            storeSlots[i].UpdateSlot();
        }
    }

}