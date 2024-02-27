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
    }

    public void SpeedEffectOff() {
        GameManager.instance.speedEffect.SetActive(false); // 이동속도 버프 꺼주기
        GameManager.instance.curMoveSpeed = GameManager.instance.originMoveSpeed; // 현재 이동속도를 기본 이동속도로 초기화
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
    
    public void ItemDescriptionOnOff(int clickSlotNum, Item clickItem) {
        
        slotNum = clickSlotNum;
        item = clickItem;
        
        if(!GameManager.instance.itemDescriptionPanel.activeSelf) {
            GameManager.instance.itemDescriptionPanel.SetActive(true);
        } else {
            GameManager.instance.itemDescriptionPanel.SetActive(false);
        }
    }
    
    public void ItemDescriptionOff() {
        GameManager.instance.itemDescriptionPanel.SetActive(false);
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
            
        } else if(slotNum != -1 && 1 == item.itemCount) { // 아이템이 1개만 있을 경우
            AlertManager.instance.AlertMessageOn(item.itemName, 9);
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