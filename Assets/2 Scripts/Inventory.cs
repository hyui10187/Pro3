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
    public bool equipSword;
    public bool equipBow;
    public bool hasCandy;
    public bool hasStoreKey;
    public bool hasChestKey;
    public bool isDoorOpen;
    public bool isChestOpen;
    public bool isInventorySlotClick;

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

        if(eatItem == null) {
            return false;
        }
        
        bool isAdded = false;
        int index = -1;
        int itemCount = 0;
        
        if(possessItems.Count < CurSlotCnt) { // 현재 보유 슬롯보다 현재 보유중인 아이템의 갯수가 적으면
            if(possessItems.Count > 0) {
                for(int i = 0; i < possessItems.Count; i++) {
                    if(possessItems[i].itemName == eatItem.itemName) {
                        isAdded = true;
                        index = i;
                        itemCount = eatItem.itemCount;
                        break;
                    }
                }

                if(isAdded) { // 이미 보유중인 아이템일 경우

                    if(eatItem.itemCount == 0) { // 아이템의 갯수가 0개이면 = 장비아이템일 경우
                        possessItems.Add(eatItem.Clone()); // 새롭게 아이템을 추가해주기
                    } else {
                        possessItems[index].itemCount += itemCount; // 기존 아이템의 갯수를 먹은 아이템의 갯수만큼 늘려주기
                    }
                    
                } else { // 기존에 가지고 있지 않은 아이템일 경우
                    possessItems.Add(eatItem.Clone());
                }

            } else {
                possessItems.Add(eatItem.Clone()); // 새롭게 아이템을 추가해줌
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
    
    public bool AddItem(Item purchaseItem, int purchaseCount) {

        if(purchaseItem == null) {
            return false;
        }
        
        bool isAdded = false;
        int index = -1;

        if(possessItems.Count < CurSlotCnt) { // 현재 보유 슬롯보다 현재 보유중인 아이템의 갯수가 적으면
            if(possessItems.Count > 0) {
                for(int i = 0; i < possessItems.Count; i++) {
                    if(possessItems[i].itemName == purchaseItem.itemName) {
                        isAdded = true;
                        index = i;
                        break;
                    }
                }

                if(isAdded) { // 이미 보유중인 아이템일 경우
                    if(purchaseItem.itemCount == 0) { // 아이템의 갯수가 0개이면 = 장비아이템일 경우
                        possessItems.Add(purchaseItem.Clone()); // 새롭게 아이템을 추가해주기
                    } else {
                        possessItems[index].itemCount += purchaseCount; // 기존 아이템의 갯수를 구입한 아이템의 갯수만큼 늘려주기
                    }
                    
                } else { // 기존에 가지고 있지 않은 아이템일 경우
                    Item copyPurchaseItem = purchaseItem.Clone();
                    copyPurchaseItem.itemCount = purchaseCount; // 구입한 갯수만큼 아이템을 생성하여 인벤토리에 넣어주기
                    possessItems.Add(copyPurchaseItem);
                }

            } else {
                Item copyPurchaseItem = purchaseItem.Clone();
                copyPurchaseItem.itemCount = purchaseCount;
                possessItems.Add(copyPurchaseItem);
            }

            if(onChangeItem != null) {
                onChangeItem.Invoke(); // 인벤토리 다시 그려주기
            }
            
            return true; // 아이템 슬롯의 갯수에 여유가 있으면 아이템을 먹어서 인벤토리에 추가해주고 true 반환
            
        } else { // 현재 보유 슬롯과 현재 보유중인 아이템의 갯수가 같으면
            for(int i = 0; i < possessItems.Count; i++) {
                if(possessItems[i].itemName == purchaseItem.itemName) {
                    isAdded = true;
                    index = i;
                    break;
                }
            }

            if(isAdded) { // 기존에 가지고 있눈 아이템일 경우
                possessItems[index].itemCount += purchaseCount; // 기존 아이템의 갯수를 구입한 아이템의 갯수만큼 늘려주기
                if(onChangeItem != null) {
                    onChangeItem.Invoke();   
                }
                return true;
            } else {
                return false; // 아이템 슬롯에 여유가 없으면 아이템을 그대로 두고 false 반환
            }
        }
    }

    public void RemoveItem(int index, int sellAmount) {
        if(possessItems[index].itemCount > 1) {
            possessItems[index].itemCount--;
            
        } else {
            possessItems.RemoveAt(index); // 리스트에서 삭제할때는 RemoveAt 메소드 사용    
        }
        
        onChangeItem.Invoke();
    }
    
    public void RemoveItem(int index) {
        if(possessItems[index].itemCount > 1) {
            possessItems[index].itemCount--;
            
        } else {
            possessItems.RemoveAt(index); // 리스트에서 삭제할때는 RemoveAt 메소드 사용    
        }
        
        onChangeItem.Invoke();
    }
    
    public void EntrustItem(int index, int itemCount, int leaveAmount) {

        if(itemCount < 2 || itemCount == leaveAmount) { // 소모 아이템이 1개 있거나 장비 아이템이거나 아이템 갯수를 전부 맡길 경우
            possessItems.RemoveAt(index); // 인벤토리의 보유 아이템을 삭제해주기
            onChangeItem.Invoke();   
        } else {
            possessItems[index].itemCount -= leaveAmount;
            onChangeItem.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Item") || other.CompareTag("QuestItem")) { // 닿은 물체의 태그가 Item이거나 QuestItem 이라면
            
            ItemScript itemScript = other.GetComponent<ItemScript>();

            if(itemScript.item.itemName == ItemName.골드소) {
                GameManager.instance.curGold += 5;
                AlertManager.instance.AcquisitionMessageOn("5 ", 11);
                Destroy(itemScript.gameObject);
                return;
                
            } else if(itemScript.item.itemName == ItemName.골드중) {
                GameManager.instance.curGold += 10;
                AlertManager.instance.AcquisitionMessageOn("10 ", 11);
                Destroy(itemScript.gameObject);
                return;
                
            } else if(itemScript.item.itemName == ItemName.골드대) {
                GameManager.instance.curGold += 15;
                AlertManager.instance.AcquisitionMessageOn("15 ", 11);
                Destroy(itemScript.gameObject);
                return;
            }
            
            bool canEat = AddItem(itemScript.GetItem()); // 아이템을 먹을 수 있는지 판단하는 플래그값
            
            if(canEat) { // 아이템을 먹을 수 있는 조건이 충족되면(슬롯의 갯수가 남아있거나 슬롯이 갯수가 꽉 차 있더라도 기존에 보유한 아이템의 갯수를 늘릴 수 있으면)

                SoundManager.instance.PlayAcquisitionSound();
                
                if(itemScript.item.itemType == ItemType.Quest) { // 퀘스트 아이템을 먹을 경우 퀘스트 인덱스 올려주기
                    QuestManager.instance.CheckQuest(0);
                }
                
                if(itemScript.item.itemName == ItemName.상점열쇠) {
                    hasStoreKey = true;
                } else if(itemScript.item.itemName == ItemName.상자열쇠) {
                    hasChestKey = true;
                }

                ItemName itemName = itemScript.item.itemName;
                string itemNameStr = itemName.ToString();
                
                AlertManager.instance.AcquisitionMessageOn(itemNameStr, 0); // 아이템을 획득하였다는 메시지를 띄워주기
                Destroy(itemScript.gameObject);
                //fieldItems.gameObject.SetActive(false); // 필드에 떨어져 있는 아이템을 먹었으면 해당 아이템은 꺼줘서 안보이게 하기

            } else { // 인벤토리가 꽉 차있다면
                AlertManager.instance.SmallAlertMessageOn(ItemName.공백, 5);
            }
        }
    }
    
}