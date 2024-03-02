using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour {

    public static PanelManager instance;
    
    public int slotNum;
    public Item item;
    
    private void Awake() {
        instance = this;
        slotNum = -1;
    }
    
    public void MenuOnOff() {

        if(GameManager.instance.storagePanel.activeSelf) {
            return;
        }
        
        if(!GameManager.instance.menuPanel.activeSelf) {
            GameManager.instance.menuPanel.SetActive(true);
            
        } else {
            GameManager.instance.menuPanel.SetActive(false);
        }
    }
    
    public void StorageOnOff() {
        if(!GameManager.instance.storagePanel.activeSelf) {
            GameManager.instance.storagePanel.SetActive(true);
        } else {
            GameManager.instance.storagePanel.SetActive(false);
        }
    }
    
    public void FPSOnOff() {
        
        if(!GameManager.instance.FPSPanel.activeSelf) {
            GameManager.instance.FPSPanel.SetActive(true);
            FPS.instance.ControlFPSPanel();
        } else {
            GameManager.instance.FPSPanel.SetActive(false);
            FPS.instance.StopControlFPSPanel();
        }
    }
    
    public void StoreOnOff() { // 상점 패널을 켜고 끄는 메소드
        if(!GameManager.instance.storePanel.activeSelf) {
            GameManager.instance.storePanel.SetActive(true);
        } else {
            GameManager.instance.storePanel.SetActive(false);
        }
    }
    
    public void VirtualOnOff() { // 가상 버튼을 켜거나 꺼주는 메소드

        if(!GameManager.instance.virtualJoystick.activeSelf) {
            GameManager.instance.virtualJoystick.SetActive(true); // 가상 조이스틱 켜주기
            GameManager.instance.attackButton.SetActive(true); // 가상 공격버튼 켜주기
            GameManager.instance.actionButton.SetActive(true); // 가상 대화버튼 켜주기
        } else {
            GameManager.instance.virtualJoystick.SetActive(false);
            GameManager.instance.attackButton.SetActive(false);
            GameManager.instance.actionButton.SetActive(false);
        }
    }
    
    public void SpeedEffectOn() {
        CancelInvoke("SpeedEffectOff"); // 기존에 이동속도 버프 꺼주는 메소드가 실행될 예정이었을 수 있으니 취소해주고 시작
        GameManager.instance.speedEffect.SetActive(true); // 이동속도 버프 켜주기
        Invoke("SpeedEffectOff", 5);
        RedrawStatsPanel();
    }

    public void SpeedEffectOff() {
        RedrawStatsPanel();
        GameManager.instance.speedEffect.SetActive(false); // 이동속도 버프 꺼주기
        GameManager.instance.curMoveSpeed = GameManager.instance.originMoveSpeed; // 현재 이동속도를 기본 이동속도로 초기화
        GameManager.instance.itemMoveSpeed = 0;
        RedrawStatsPanel();
    }
    
    public void HelpOnOff() { // 우측 상단의 물음표 버튼을 클릭했을때 실행할 메소드

        if(GameManager.instance.storagePanel.activeSelf) {
            return; 
        }
        
        if(!GameManager.instance.helpPanel.activeSelf) { // Help 창이 꺼져 있다면
            GameManager.instance.helpPanel.SetActive(true); // 켜주기
            GameManager.instance.expSlider.SetActive(false);
        } else {
            GameManager.instance.helpPanel.SetActive(false);
            GameManager.instance.expSlider.SetActive(true);
        }
    }
    
    public void InventoryOnOff() {
        if(!GameManager.instance.inventoryPanel.activeSelf) {
            GameManager.instance.inventoryPanel.SetActive(true);
        } else {
            GameManager.instance.inventoryPanel.SetActive(false);
        }
    }
    
    public void EquipmentOnOff() {
        if(!GameManager.instance.equipmentPanel.activeSelf) {
            GameManager.instance.equipmentPanel.SetActive(true);
        } else {
            GameManager.instance.equipmentPanel.SetActive(false);
        }
    }
    
    public void StatsOnOff() {
        if(!GameManager.instance.statsPanel.activeSelf) {
            GameManager.instance.statsPanel.SetActive(true);
        } else {
            GameManager.instance.statsPanel.SetActive(false);
        }
    }
    
    public void StatsUpOn() {
        GameManager.instance.statsUpButton.SetActive(true);
    }

    private void StatsUpOff() {
        GameManager.instance.statsUpButton.SetActive(false);
    }

    public void StatsUpButtonClick(int idx) { // 스탯을 올리는 버튼을 클릭했을때 메소드

        switch(idx) {
            
            case 0: // 힘
                GameManager.instance.strPoint++;
                break;
            
            case 1: // 민첩
                GameManager.instance.dexPoint++;
                break;
            
            case 2: // 체력
                GameManager.instance.conPoint++;
                break;
            
            case 3: // 지혜
                GameManager.instance.wisPoint++;
                break;
        }

        GameManager.instance.statsPoint--;
        
        if(GameManager.instance.statsPoint < 1) {
            StatsUpOff();
        }
        
        RedrawStatsPanel();
    }

    public void RedrawStatsPanel() {
        GameManager.instance.statsPointText.text = "스탯 포인트 " + GameManager.instance.statsPoint;
        GameManager.instance.strPointText.text = "근력   " + GameManager.instance.strPoint;
        GameManager.instance.dexPointText.text = "민첩   " + GameManager.instance.dexPoint;
        GameManager.instance.conPointText.text = "체력   " + GameManager.instance.conPoint;
        GameManager.instance.wisPointText.text = "지혜   " + GameManager.instance.wisPoint;

        GameManager.instance.playerAttackPower = GameManager.instance.strPoint * 2;
        GameManager.instance.attackPowerText.text = "공격력 " + GameManager.instance.playerAttackPower + " (+" + GameManager.instance.itemAttackPower + ")";
        
        GameManager.instance.originMoveSpeed = GameManager.instance.dexPoint == 0 ? 5 : GameManager.instance.dexPoint + 5;
        GameManager.instance.moveSpeedText.text = "이동속도 " + GameManager.instance.originMoveSpeed + " (+" + GameManager.instance.itemMoveSpeed + ")";
        
        GameManager.instance.curMoveSpeed = GameManager.instance.originMoveSpeed;
    }
    
    public void ItemDescriptionOnOff(int clickSlotNum, Item clickItem) {
        
        slotNum = clickSlotNum;
        item = clickItem;

        if(!GameManager.instance.itemDescriptionPanel.activeSelf) { // 아이템 설명창이 꺼져있을 경우
            switch(item.itemType) {
                case ItemType.Weapon: // 장비 아이템의 설명창을 띄울 경우 장착 버튼을 활성화
                case ItemType.Shield:
                case ItemType.Ring:
                case ItemType.Necklace:
                    GameManager.instance.consumptionButton.interactable = false;
                    GameManager.instance.equipButton.interactable = true;
                    break;
                
                case ItemType.Consumables: // 소비 아이템일 경우 사용 버튼을 활성화
                    GameManager.instance.consumptionButton.interactable = true;
                    GameManager.instance.equipButton.interactable = false;
                    break;
                
                default: // 나머지 아이템일 경우 사용, 장착을 모두 비활성화
                    GameManager.instance.consumptionButton.interactable = false;
                    GameManager.instance.equipButton.interactable = false;
                    break;
            }

            if(InventoryManager.instance.inventorySlots[slotNum].equipImage.activeSelf) { // 이미 장착중일 경우
                GameManager.instance.equipButtonText.text = "해제"; // 가운데 버튼의 문구를 해제로 바꿔주기
            } else {
                GameManager.instance.equipButtonText.text = "장착";
            }
            GameManager.instance.itemDescriptionPanel.SetActive(true);

        } else {
            GameManager.instance.itemDescriptionPanel.SetActive(false);
        }
    }
    
    public void ItemDescriptionOff() {
        GameManager.instance.itemDescriptionPanel.SetActive(false);
    }

    public void EquipButton() { // 장착 버튼을 클릭했을때 호출되는 메소드
        if(!InventoryManager.instance.inventorySlots[slotNum].equipImage.activeSelf) { // 장착되었다는 E 문구가 꺼져있는 상태면
            InventoryManager.instance.inventorySlots[slotNum].equipImage.SetActive(true); // 장착되었다는 E 문구 켜주기

            for(int i = 0; i < EquipmentManager.instance.equipmentSlots.Length; i++) {
                if(EquipmentManager.instance.equipmentSlots[i].itemType == item.itemType) {
                    EquipmentManager.instance.equipmentSlots[i].RedrawSlot(item); // 장비창의 해당하는 슬롯에 장착한 아이템 이미지 넣어주기
                }
            }

            if(item.itemType == ItemType.Weapon) {
                Inventory.instance.equipWeapon = true;
                GameManager.instance.itemAttackPower = item.itemAttackPower;
            }

            RedrawStatsPanel();
            ItemDescriptionOff();
            
        } else { // 장착되었다는 E 문구가 켜져있는 상태면
            
            for(int i = 0; i < EquipmentManager.instance.equipmentSlots.Length; i++) {
                if(EquipmentManager.instance.equipmentSlots[i].itemType == item.itemType) {
                    EquipmentManager.instance.equipmentSlots[i].RemoveSlot(); // 장비창의 해당하는 슬롯에 장착되어 있던 이미지를 지워주기
                }
            }
            
            InventoryManager.instance.inventorySlots[slotNum].equipImage.SetActive(false); // 인벤토리의 슬롯에서 장착되었다는 E 문구 꺼주기

            if(item.itemType == ItemType.Weapon) {
                Inventory.instance.equipWeapon = false;    
            }
            
            GameManager.instance.itemAttackPower = 0;
            ItemDescriptionOff();
        }
    }
    
    public void ConsumptionButton() { // 아이템 설명 패널에서 사용 버튼을 클릭했을 경우 호출할 메소드
        
        if(slotNum != -1) { // 파라미터로 넘어온 slotNum이 존재할 경우
            bool isUse = false;
        
            if(item != null) {
                isUse = item.Use();   
            }

            if(isUse && 1 < item.itemCount) { // 아이템이 2개 이상일 경우
                AlertManager.instance.AlertMessageOn(item.itemName, 4);
                Inventory.instance.RemoveItem(slotNum);

            } else if(isUse && item.itemCount == 1) { // 아이템이 1개만 있을 경우
                AlertManager.instance.AlertMessageOn(item.itemName, 4);
                Inventory.instance.RemoveItem(slotNum);
                ItemDescriptionOnOff(-1, null); // 아이템 설명 패널을 꺼주기
            }
        }
    }
    
    public void DeleteButton() { // 아이템 설명 패널에서 삭제 버튼을 클릭했을 경우 호출할 메소드
        
        if(slotNum != -1 && 1 < item.itemCount) { // 아이템이 2개 이상일 경우
            AlertManager.instance.AlertMessageOn(item.itemName, 9);
            Inventory.instance.RemoveItem(slotNum);
            
        } else if(slotNum != -1 && item.itemCount == 1) { // 아이템이 1개만 있을 경우
            AlertManager.instance.AlertMessageOn(item.itemName, 9);

            if(InventoryManager.instance.inventorySlots[slotNum].equipImage.activeSelf) { // 장착된 아이템을 삭제한거면
                InventoryManager.instance.inventorySlots[slotNum].equipImage.SetActive(false); // 장착되었다는 E 문구 꺼주기

                for(int i = 0; i < EquipmentManager.instance.equipmentSlots.Length; i++) {
                    if(EquipmentManager.instance.equipmentSlots[i].itemType == item.itemType) {
                        EquipmentManager.instance.equipmentSlots[i].RemoveSlot();
                        break;
                    }
                }

                if(item.itemType == ItemType.Weapon) { // 무기를 삭제한 것이면
                    Inventory.instance.equipWeapon = false;
                }
            }
            
            Inventory.instance.RemoveItem(slotNum);
            ItemDescriptionOnOff(-1, null);
        }
    }
    
    public void PanelOn() {

        GameManager.instance.weatherPanel.SetActive(true);
        
        // UI - UpLeft
        GameManager.instance.gaugePanel.SetActive(true);
        GameManager.instance.buffPanel.SetActive(true);
        GameManager.instance.questPanel.SetActive(true);
        GameManager.instance.helpButton.SetActive(true);
        GameManager.instance.FPSButton.SetActive(true);

        // UI - UpMiddle
        GameManager.instance.goldPanel.SetActive(true);

        // UI - UpRight
        GameManager.instance.minimapPanel.SetActive(true);
        GameManager.instance.menuButton.SetActive(true);
        GameManager.instance.inventoryButton.SetActive(true);
        GameManager.instance.virtualButton.SetActive(true);
        GameManager.instance.equipmentButton.SetActive(true);
        
        // UI - MiddleMiddle

        // UI - MiddleRight

        // UI - BottomLeft

        // UI - BottomBottom
        GameManager.instance.expSlider.SetActive(true);
        
        // UI - BottomRight

        VirtualOnOff();
    }
    
    public void PanelOff() {
        
        // UI - Panel
        GameManager.instance.startPanel.SetActive(false);
        GameManager.instance.menuPanel.SetActive(false);
        GameManager.instance.deadPanel.SetActive(false);
        GameManager.instance.weatherPanel.SetActive(false);
        
        // UI - UpLeft
        GameManager.instance.gaugePanel.SetActive(false);
        GameManager.instance.buffPanel.SetActive(false);
        GameManager.instance.frozenEffect.SetActive(false);
        GameManager.instance.speedEffect.SetActive(false);
        GameManager.instance.questPanel.SetActive(false);
        GameManager.instance.helpButton.SetActive(false);
        GameManager.instance.FPSButton.SetActive(false);
        GameManager.instance.FPSPanel.SetActive(false);

        // UI - UpMiddle
        GameManager.instance.goldPanel.SetActive(false);
        GameManager.instance.timePanel.SetActive(false);

        // UI - UpRight
        GameManager.instance.minimapPanel.SetActive(false);
        GameManager.instance.menuButton.SetActive(false);
        GameManager.instance.inventoryButton.SetActive(false);
        GameManager.instance.virtualButton.SetActive(false);
        GameManager.instance.equipmentButton.SetActive(false);

        // UI - MiddleMiddle
        GameManager.instance.helpPanel.SetActive(false);
        GameManager.instance.saveMessage.SetActive(false);
        GameManager.instance.alertMessage.SetActive(false);
        GameManager.instance.healthManaMessage.SetActive(false);
        
        // UI - MiddleRight
        GameManager.instance.inventoryPanel.SetActive(false);
        GameManager.instance.monsterPanel.SetActive(false);
        
        // UI - BottomLeft
        GameManager.instance.virtualJoystick.SetActive(false);
        
        // UI - BottomBottom
        GameManager.instance.talkPanel.SetActive(false);
        GameManager.instance.expSlider.SetActive(false);
        
        // UI - BottomRight
        GameManager.instance.attackButton.SetActive(false);
        GameManager.instance.actionButton.SetActive(false);
    }
    
}