using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{

    public static PanelManager instance;
    
    public int slotNum;
    public Item item;
    public int helpPanelPage;
    public int amount;
    public bool isStoreSlotClick;

    private void Awake()
    {
        instance = this;
        slotNum = -1;
    }

    private void Update()
    {
        GameManager.instance.entrustAmountText.text = amount.ToString(); // 창고에 맡길 갯수를 업데이트 해주기
        GameManager.instance.withdrawAmountText.text = amount.ToString(); // 창고에 맡길 갯수를 업데이트 해주기
        GameManager.instance.purchaseAmountText.text = amount.ToString(); // 상점에서 구매할 갯수를 업데이트 해주기
        GameManager.instance.sellAmountText.text = amount.ToString(); // 상점에 판매할 갯수를 업데이트 해주기
        
        if(helpPanelPage == 1) // 첫번째 페이지일 경우 왼쪽으로 가는 버튼을 비활성화 하기
        {
            GameManager.instance.leftPageButton.interactable = false;
            GameManager.instance.rightPageButton.interactable = true;
        }
        else if(AlertManager.instance.helpPanelData.Count <= helpPanelPage) // 마지막 페이지일 경우 오른쪽으로 가는 버튼을 비활성화 하기
        {
            GameManager.instance.rightPageButton.interactable = false;
        }
        else
        {
            GameManager.instance.leftPageButton.interactable = true;
            GameManager.instance.rightPageButton.interactable = true;
        }
    }

    public void TipTabClick() // HelpPanel의 도움말 탭을 클릭할 경우 호출할 메소드
    {
        helpPanelPage = 1; // Help 패널의 페이지 번호를 1로 변경
        SoundManager.instance.PlaySound(AudioClipName.Plus);
        GetHelpPanelText();
    }
    
    public void MiniMapTabClick() // HelpPanel의 미니맵 탭을 클릭할 경우 호출할 메소드
    {
        helpPanelPage = 5;
        SoundManager.instance.PlaySound(AudioClipName.Plus);
        GetHelpPanelText();
    }
    
    public void KeyTabClick() // HelpPanel의 조작키 탭을 클릭할 경우 호출할 메소드
    {
        helpPanelPage = 7;
        SoundManager.instance.PlaySound(AudioClipName.Plus);
        GetHelpPanelText();
    }

    public void TurnTablePanelOnOff()
    {
        if(!GameManager.instance.turnTablePanel.activeSelf)
        {
            GameManager.instance.turnTablePanel.SetActive(true);
        }
        else
        {
            GameManager.instance.turnTablePanel.SetActive(false);
            TurnTableManager.instance.StopTurnTable(); // 턴테이블 패널이 꺼지면 음반도 멈춰주기
        }
    }
    
    public void SellConfirmPanelOn(int slotNum, Item sellItem)
    {
        Text confirmText = GameManager.instance.sellConfirmPanel.GetComponentInChildren<Text>();
        confirmText.text = sellItem.itemName + " 아이템을\n정말로 판매하시겠습니까?";
        PurchaseAmountPanelOff();
        PurchaseConfirmPanelOff();
        SellAmountPanelOff();
        GameManager.instance.itemDescriptionPanel.SetActive(false);
        GameManager.instance.sellConfirmPanel.SetActive(true);
        this.slotNum = slotNum;
        item = sellItem;
    }

    public void SellConfirmPanelOff()
    {
        GameManager.instance.sellConfirmPanel.SetActive(false);
    }
    
    public void PurchaseConfirmPanelOn(Item purchaseItem)
    {
        Text confirmText = GameManager.instance.purchaseConfirmPanel.GetComponentInChildren<Text>();
        confirmText.text = purchaseItem.itemName + " 아이템을\n구매하시겠습니까?";
        SellAmountPanelOff();
        GameManager.instance.itemDescriptionPanel.SetActive(false);
        GameManager.instance.purchaseConfirmPanel.SetActive(true);
        item = purchaseItem;
        amount = 1;
    }

    public void PurchaseConfirmPanelOff()
    {
        GameManager.instance.purchaseConfirmPanel.SetActive(false);
    }

    public void PurchaseAmountPanelOn(Item purchaseItem) // 구매 수량을 선택하는 패널
    {
        item = purchaseItem;
        amount = 0;
        GameManager.instance.purchaseText.text = purchaseItem.itemName + " 아이템을\n얼마나 구매하시겠습니까?";
        SellAmountPanelOff();
        SellConfirmPanelOff();
        GameManager.instance.itemDescriptionPanel.SetActive(false);
        GameManager.instance.purchaseAmountPanel.SetActive(true);
    }

    public void PurchaseAmountPanelOff()
    {
        GameManager.instance.purchaseAmountPanel.SetActive(false);
    }

    public void ChatPanelOnOff(int flag)
    {
        bool isOn = GameManager.instance.chatPanel.activeSelf;

        switch(flag)
        {
            case 0: // 화면의 확성기 버튼을 클릭했을 경우
                GameManager.instance.chatPanel.SetActive(!isOn);
                break;

            case 1: // 채팅 패널을 꺼줘야 할 경우
                GameManager.instance.chatPanel.SetActive(false);
                Chatting.Instance.OnDeselect(); // 채팅 Input Field 지워주기
                break;
        }
    }
    
    public void SellAmountPanelOn(int slotNum, Item sellItem) // 판매 수량을 선택하는 패널
    {
        this.slotNum = slotNum;
        item = sellItem;
        amount = 0;
        GameManager.instance.sellText.text = sellItem.itemName + " 아이템을\n얼마나 판매하시겠습니까?";
        PurchaseAmountPanelOff();
        PurchaseConfirmPanelOff();
        SellConfirmPanelOff();
        GameManager.instance.itemDescriptionPanel.SetActive(false);
        GameManager.instance.sellAmountPanel.SetActive(true);
    }
    
    public void SellAmountPanelOff()
    {
        amount = 0;
        GameManager.instance.sellAmountPanel.SetActive(false);
    }
    
    public void WithdrawAmountPanelOn(int slotNum, Item withdrawItem) {
        this.slotNum = slotNum;
        item = withdrawItem;
        amount = 0;
        GameManager.instance.withdrawText.text = withdrawItem.itemName + " 아이템을\n얼마나 찾으시겠습니까?";
        EntrustAmountPanelOff();
        GameManager.instance.itemDescriptionPanel.SetActive(false);
        GameManager.instance.withdrawAmountPanel.SetActive(true);
    }

    public void WithdrawAmountPanelOff() {
        GameManager.instance.withdrawAmountPanel.SetActive(false);
    }

    public void WithdrawButtonClick() {
        WithdrawLogic();
    }
    
    public void WithdrawButtonClick(int slotNum, Item withdrawItem) {
        amount = 1;
        this.slotNum = slotNum;
        item = withdrawItem;
        WithdrawLogic();
    }

    private void WithdrawLogic() {
        
        if(item.itemCount < amount) {
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 21);
            return;
        } else if(amount == 0) {
            return;
        }
        
        bool canWithdraw = Inventory.instance.AddItem(item, amount); // 아이템을 찾을 수 있는지 없는지 = 플레이어의 인벤토리 슬롯이 비어있는지
        
        if(canWithdraw) {
            if(item.itemType == ItemType.Quest) {
                QuestManager.instance.questActionIndex++;
            } else if(item.itemType == ItemType.Record) {
                Inventory.instance.recordCnt++;
            }
            WithdrawAmountPanelOff();
            EntrustAmountPanelOff();
            SoundManager.instance.PlaySound(AudioClipName.Buy);
            StorageManager.instance.WithdrawItem(slotNum, item.itemCount, amount);
            AlertManager.instance.SmallAlertMessageOn(item.itemName, 10);
        } else {
            AlertManager.instance.SmallAlertMessageOn(ItemName.공백, 20);
        }
        GameManager.instance.withdrawAmountPanel.SetActive(false);
    }

    public void SellButtonClick() {
        SellLogic();
    }

    private void SellLogic() {
        
        SellConfirmPanelOff();
        
        if(item.isEquipped) { // 장착중인 아이템을 상점에 판매하려고 할 경우
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 14);
            return;
        }

        if(item.itemType == ItemType.Quest) { // 퀘스트 아이템을 상점에 판매하려고 할 경우
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 16);
            return;
        }

        if(item.itemType == ItemType.Record) { // 퀘스트 아이템을 상점에 판매했을 경우
            GameManager.instance.curGold += item.itemPrice; // 판매한 아이템의 금액만큼 플레이어의 소지금을 올려주기
            AlertManager.instance.SmallAlertMessageOn(item.itemName, 2); // 아이템을 판매하였다는 메시지를 띄워주기
            Inventory.instance.EntrustOrSellItem(slotNum, item.itemCount, item.itemCount);
            SoundManager.instance.PlaySound(AudioClipName.Buy);
            Inventory.instance.recordCnt--;
            TurnTableManager.instance.changeTurnTablePanel.Invoke();
            return;
        }

        if(item.itemCount < 2 && amount == 0) {
            GameManager.instance.curGold += item.itemPrice; // 판매한 아이템의 금액만큼 플레이어의 소지금을 올려주기
            AlertManager.instance.SmallAlertMessageOn(item.itemName, 2); // 아이템을 판매하였다는 메시지를 띄워주기
            SoundManager.instance.PlaySound(AudioClipName.Buy);
            Inventory.instance.EntrustOrSellItem(slotNum, item.itemCount, item.itemCount);
            return;
        }
        
        if(item.itemCount < amount) {
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 19);
            return;
        } else if(amount == 0) {
            return;
        }

        GameManager.instance.curGold += (amount * item.itemPrice); // 판매한 아이템의 금액만큼 플레이어의 소지금을 올려주기
        AlertManager.instance.SmallAlertMessageOn(item.itemName, 2); // 아이템을 판매하였다는 메시지를 띄워주기
        SoundManager.instance.PlaySound(AudioClipName.Buy);
        Inventory.instance.EntrustOrSellItem(slotNum, item.itemCount, amount);
        
        GameManager.instance.sellAmountPanel.SetActive(false);
        amount = 0;
    }

    public void PurchaseButtonClick() { // 구매 버튼 클릭 메소드

        if(amount == 0) { // 0개를 구매하려고 할 경우 되돌려 보내기
            return;
        }

        if(GameManager.instance.curGold < amount * item.itemPrice) { // 구매하려는 갯수보다 돈이 적으면
            AlertManager.instance.SmallAlertMessageOn(ItemName.공백, 7); // 소지금 부족 메시지 띄워주기
            return;
        }
        
        bool canPurchase = Inventory.instance.AddItem(item, amount);
        
        if(canPurchase && item != null) {
            GameManager.instance.curGold -= amount * item.itemPrice; // 구매한 금액만큼 소지금에서 까주기
            AlertManager.instance.SmallAlertMessageOn(item.itemName, 1); // 구매했다는 메시지 띄워주기

            if(item.itemName == ItemName.화살) { // 상점에서 구매한게 화살일 경우
                Inventory.instance.arrowCnt += amount; // 소지한 화살의 갯수를 구매한 갯수만큼 늘려주기
            }

            SoundManager.instance.PlaySound(AudioClipName.Buy);
            PurchaseAmountPanelOff(); // 구매하고 나면 패널 꺼주기
            PurchaseConfirmPanelOff();
            amount = 0; // 갯수 0으로 초기화 해주기
        } else if(!canPurchase) {
            AlertManager.instance.SmallAlertMessageOn(ItemName.공백, 8); // 인벤토리가 가득차서 구매 불가 메시지
        }
    }
    
    public void EntrustAmountPanelOn(int slotNum, Item entrustItem) { // 창고에 맡기는 아이템 갯수 설정하는 패널
        this.slotNum = slotNum;
        item = entrustItem; // 파라미터로 받아온 아이템으로 전역변수 item을 갱신해주기
        amount = 0;
        GameManager.instance.entrustText.text = entrustItem.itemName + " 아이템을\n얼마나 맡기시겠습니까?";
        WithdrawAmountPanelOff();
        GameManager.instance.itemDescriptionPanel.SetActive(false);
        GameManager.instance.entrustAmountPanel.SetActive(true);
    }
    
    public void EntrustAmountPanelOff() { // 창고에 맡기는 아이템 갯수 설정하는 패널
        amount = 0;
        GameManager.instance.entrustAmountPanel.SetActive(false);
    }

    public void EntrustButtonClick() { // 맡기기 버튼을 클릭했을때 호출되는 메소드
        EntrustLogic();
    }

    public void EntrustButtonClick(int slotNum, Item entrustItem) { // 맡기기 버튼을 클릭했을때 호출되는 메소드
        this.slotNum = slotNum;
        item = entrustItem; // 파라미터로 받아온 새로운 아이템으로 기존의 전역변수 아이템을 갱신해주기
        EntrustLogic();
    }

    private void EntrustLogic() {
        if(item.isEquipped) { // 장착중인 아이템을 창고에 맡기려고 할 경우
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 14); // 알림 메시지 띄워주기
            return;
        }

        bool canEntrust;

        if(item.itemCount < 2) { // 아이템의 갯수가 2개보다 적으면 = 소비 아이템이 1개 있거나 장비 아이템이거나
            canEntrust = StorageManager.instance.AddItem(item, item.itemCount); // 창고에 아이템을 넣어주기

            if(canEntrust && item != null) {

                if(item.itemType == ItemType.Quest) {
                    QuestManager.instance.questActionIndex--; // 창고에 퀘스트 아이템을 맡길 경우 퀘스트 인덱스 하나 내려주기
                }

                EntrustAmountPanelOff();
                WithdrawAmountPanelOff();
                SoundManager.instance.PlaySound(AudioClipName.Buy);
                AlertManager.instance.SmallAlertMessageOn(item.itemName, 3); // 창고에 맡기는 메시지
                Inventory.instance.EntrustOrSellItem(slotNum, item.itemCount, amount);
                
                if(item.itemType == ItemType.Record) {
                    Inventory.instance.recordCnt--; // 창고에 음반 아이템을 맡길 경우 보유중인 음반 갯수 1개 줄여주기
                    TurnTableManager.instance.changeTurnTablePanel.Invoke(); // 턴테이블 슬롯 다시 그려주기
                }
                
                return;
            }
        }

        if(item.itemCount < amount) {
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 17);
            return;
        } else if(amount == 0) {
            return;
        }
        
        canEntrust = StorageManager.instance.AddItem(item, amount); // 창고에 아이템을 넣어주기

        if(canEntrust && item != null) {

            if(item.itemType == ItemType.Quest) {
                QuestManager.instance.questActionIndex--; // 창고에 퀘스트 아이템을 맡길 경우 퀘스트 인덱스 하나 내려주기
            }
            SoundManager.instance.PlaySound(AudioClipName.Buy);
            AlertManager.instance.SmallAlertMessageOn(item.itemName, 3); // 창고에 맡기는 메시지
            Inventory.instance.EntrustOrSellItem(slotNum, item.itemCount,amount);
            GameManager.instance.entrustAmountPanel.SetActive(false); // 맡기고 나면 패널 꺼주기
            amount = 0;
        }
    }

    public void AmountPlus() { // + 버튼을 클릭했을때 호출되는 메소드
        amount++;
        SoundManager.instance.PlaySound(AudioClipName.Plus);
    }

    public void AmountMinus() { // - 버튼을 클릭했을때 호출되는 메소드
        amount--;
        SoundManager.instance.PlaySound(AudioClipName.Plus);
        if(amount < 0) { // - 버튼을 연속적으로 눌러도 최저 0까지만 내려가도록
            amount = 0;
        }
    }

    public void MenuOnOff() {
        
        if(Player.instance.isDead || !GameManager.instance.isLive) {
            return;
        }
        
        if(!GameManager.instance.menuPanel.activeSelf) {
            GameManager.instance.menuPanel.SetActive(true);
        } else {
            GameManager.instance.menuPanel.SetActive(false);
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }

    public void FPSOnOff() {

        if(Player.instance.isDead || !GameManager.instance.isLive) {
            return;
        }
        
        if(!GameManager.instance.fpsPanel.activeSelf) {
            GameManager.instance.fpsPanel.SetActive(true);
            FPS.instance.ControlFPSPanel();
        } else {
            GameManager.instance.fpsPanel.SetActive(false);
            FPS.instance.StopControlFPSPanel();
        }
    }
    
    public void StorageOnOff() { // 창고 패널
        if(!GameManager.instance.storagePanel.activeSelf) {
            GameManager.instance.storagePanel.SetActive(true);
            GameManager.instance.inventoryPanel.SetActive(true);
        } else {
            GameManager.instance.storagePanel.SetActive(false);
            GameManager.instance.inventoryPanel.SetActive(false);
            GameManager.instance.DetailPanelOff();
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }
    
    public void GroceryStoreOnOff() { // 잡화상점 패널
        if(!GameManager.instance.groceryStorePanel.activeSelf) {
            GameManager.instance.groceryStorePanel.SetActive(true);
            GameManager.instance.inventoryPanel.SetActive(true);
        } else {
            GameManager.instance.groceryStorePanel.SetActive(false);
            GameManager.instance.inventoryPanel.SetActive(false);
            GameManager.instance.DetailPanelOff();
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }
    
    public void EquipmentStoreOnOff() { // 장비상점 패널
        if(!GameManager.instance.equipmentStorePanel.activeSelf) {
            GameManager.instance.equipmentStorePanel.SetActive(true);
            GameManager.instance.inventoryPanel.SetActive(true);
        } else {
            GameManager.instance.equipmentStorePanel.SetActive(false);
            GameManager.instance.inventoryPanel.SetActive(false);
            GameManager.instance.DetailPanelOff();
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }
    
    public void VirtualOnOff() { // 가상 버튼을 켜거나 꺼주는 메소드

        if(Player.instance.isDead || !GameManager.instance.isLive) {
            return;
        }
        
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
    
    public void HelpOnOff() { // 좌측 상단의 물음표 버튼을 클릭했을때 실행할 메소드

        if(Player.instance.isDead || !GameManager.instance.isLive || GameManager.instance.chatPanel.activeSelf) {
            return;
        }
        
        helpPanelPage = 1; // Help 패널의 페이지 번호는 0로 항상 초기화
        GetHelpPanelText();
        
        if(!GameManager.instance.helpPanel.activeSelf) { // Help 창이 꺼져 있다면
            GameManager.instance.helpPanel.SetActive(true); // 켜주기
            GameManager.instance.expSlider.SetActive(false);
        } else {
            GameManager.instance.helpPanel.SetActive(false);
            GameManager.instance.expSlider.SetActive(true);
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }

    public void LeftPageButtonClick() { // Help 패널에서 왼쪽으로 가는 버튼을 클릭했을때 호출되는 메소드
        SoundManager.instance.PlaySound(AudioClipName.Plus);
        if(1 < helpPanelPage) {
            helpPanelPage--;
            GetHelpPanelText();
        }
    }
    
    public void RightPageButtonClick() { // Help 패널에서 왼쪽으로 가는 버튼을 클릭했을때 호출되는 메소드
        SoundManager.instance.PlaySound(AudioClipName.Plus);
        if(helpPanelPage < AlertManager.instance.helpPanelData.Count) {
            helpPanelPage++;
            GetHelpPanelText();   
        }
    }

    private void GetHelpPanelText() {
        string textData = AlertManager.instance.helpPanelData[helpPanelPage];
        GameManager.instance.helpPanelText.text = textData;
    }
    
    public void InventoryOnOff() {
        
        if(Player.instance.isDead || !GameManager.instance.isLive || GameManager.instance.chatPanel.activeSelf)
            return;

        if(!GameManager.instance.inventoryPanel.activeSelf) { // 인벤토리 패널이 꺼져있는 상태이면
            GameManager.instance.inventoryPanel.SetActive(true);
            GameManager.instance.inventoryPanel.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); // 먼저 인벤토리 패널의 크기를 줄여주기
            GameManager.instance.inventoryPanel.transform.DOScale(1, 1) // 첫번째 파라미터: 최종 크기, 두번째 파라미터: 걸리는 시간
                .SetEase(Ease.OutElastic);

        } else { // 인벤토리 패널이 켜져있는 상태이면
            GameManager.instance.inventoryPanel.transform.DOScale(0.2f, 0.15f)
                .SetEase(Ease.Linear)
                .OnComplete(() => { // 인벤토리 패널이 작아지는게 끝나면
                    GameManager.instance.inventoryPanel.SetActive(false); // 인벤토리 패널 꺼주기
                    GameManager.instance.inventoryPanel.transform.localScale = Vector3.one; // 인벤토리 패널의 크기를 원상복구 해주기
                });
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }
    
    public void EquipmentOnOff() {
        
        if(Player.instance.isDead || !GameManager.instance.isLive || GameManager.instance.chatPanel.activeSelf) {
            return;
        }
        
        if(!GameManager.instance.equipmentPanel.activeSelf) {
            GameManager.instance.equipmentPanel.SetActive(true);
        } else {
            GameManager.instance.equipmentPanel.SetActive(false);
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }
    
    public void StatsOnOff() {
        
        if(Player.instance.isDead || !GameManager.instance.isLive || GameManager.instance.chatPanel.activeSelf) {
            return;
        }
        
        if(!GameManager.instance.statsPanel.activeSelf) {
            GameManager.instance.statsPanel.SetActive(true);
        } else {
            GameManager.instance.statsPanel.SetActive(false);
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }
    
    public void QuestOnOff() {
        
        if(Player.instance.isDead || !GameManager.instance.isLive || GameManager.instance.chatPanel.activeSelf) {
            return;
        }
        
        if(!GameManager.instance.questPanel.activeSelf) {
            GameManager.instance.questPanel.SetActive(true);
        } else {
            GameManager.instance.questPanel.SetActive(false);
        }
        SoundManager.instance.PlaySound(AudioClipName.Panel);
    }
    
    public void SleepConfirmOn() {
        GameManager.instance.sleepConfirmPanel.SetActive(true);
    }

    public void SleepConfirmOff() {
        GameManager.instance.sleepConfirmPanel.SetActive(false);
        GameManager.instance.talkPanel.SetActive(false);
        GameManager.instance.isAction = false;
        GameManager.instance.expSlider.SetActive(true);
    }
    
    public void StatsUpButtonOn() {
        GameManager.instance.statsUpButton.SetActive(true);
    }

    private void StatsUpButtonOff() {
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
            StatsUpButtonOff();
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
        
        SellAmountPanelOff();
        SellConfirmPanelOff();
        PurchaseAmountPanelOff();
        PurchaseConfirmPanelOff();
        WithdrawAmountPanelOff();
        EntrustAmountPanelOff();

        if(slotNum == -1) { // 잡화상점이나 장비상점의 슬롯을 롱프레스 했을 경우에는 3개 버튼 모두 비활성화
            GameManager.instance.consumptionButton.interactable = false;
            GameManager.instance.equipButton.interactable = false;
            GameManager.instance.deleteButton.interactable = false;

            if(!GameManager.instance.itemDescriptionPanel.activeSelf) { // 아이템 설명창이 꺼져있을 경우
                GameManager.instance.itemDescriptionPanel.SetActive(true);
            } else {
                GameManager.instance.itemDescriptionPanel.SetActive(false);
            }
            return;
        }

        if(!GameManager.instance.itemDescriptionPanel.activeSelf) { // 아이템 설명창이 꺼져있을 경우
            switch(item.itemType) {
                case ItemType.Helmet:
                case ItemType.Necklace:
                case ItemType.Armor:
                case ItemType.Weapon: // 장비 아이템의 설명창을 띄울 경우 장착 버튼을 활성화
                case ItemType.Shield:
                case ItemType.Gloves:
                case ItemType.Ring:
                case ItemType.Boots:
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
                GameManager.instance.equipButtonText.text = "장착"; // 기본적으로는 장착 이라는 문구로
            }
            GameManager.instance.itemDescriptionPanel.SetActive(true);

        } else {
            GameManager.instance.itemDescriptionPanel.SetActive(false);
        }
    }
    
    public void ItemDescriptionOff() {
        GameManager.instance.itemDescriptionPanel.SetActive(false);
    }

    public void EquipButtonClick() { // 장착 버튼을 클릭했을때 호출되는 메소드
        if(!InventoryManager.instance.inventorySlots[slotNum].equipImage.activeSelf) { // 장착되었다는 E 문구가 꺼져있는 상태면
            InventoryManager.instance.inventorySlots[slotNum].equipImage.SetActive(true); // 장착되었다는 E 문구 켜주기
            SoundManager.instance.PlaySound(AudioClipName.Equip);
            Inventory.instance.possessItems[slotNum].isEquipped = true;

            for(int i = 0; i < EquipmentManager.instance.equipmentSlots.Length; i++) {
                if(EquipmentManager.instance.equipmentSlots[i].itemType == item.itemType) {
                    EquipmentManager.instance.equipmentSlots[i].RedrawSlot(item); // 장비창의 해당하는 슬롯에 장착한 아이템 이미지 넣어주기
                }
            }

            switch(item.itemType) {
                case ItemType.Weapon:
                    if(item.itemName == ItemName.소드) {
                        Inventory.instance.equipSword = true;
                        Inventory.instance.equipBow = false;
                        EquipImageOff();
                    } else if(item.itemName == ItemName.활) {
                        Inventory.instance.equipBow = true;
                        Inventory.instance.equipSword = false;
                        EquipImageOff();
                    }
                    GameManager.instance.itemAttackPower = item.itemAttackPower;
                    break;
                
                case ItemType.Ring:
                    GameManager.instance.maxMana += 10; // 마나 10 올려주기
                    break;
                
                case ItemType.Necklace:
                    GameManager.instance.dexPoint += 1; // 민첩 1 올려주기
                    break;
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
            Inventory.instance.possessItems[slotNum].isEquipped = false;

            switch(item.itemType) {
                
                case ItemType.Weapon:
                    if(item.itemName == ItemName.소드) {
                        Inventory.instance.equipSword = false;
                    } else if(item.itemName == ItemName.활) {
                        Inventory.instance.equipBow = false;
                    }
                    GameManager.instance.itemAttackPower = 0;
                    break;
                
                case ItemType.Ring:
                    GameManager.instance.maxMana -= 10; // 마나 10 빼주기
                    break;
                
                case ItemType.Necklace:
                    GameManager.instance.dexPoint -= 1; // 민첩 1 빼주기
                    break;
            }
            
            RedrawStatsPanel();
            ItemDescriptionOff();
        }
    }

    public void EquipImageOff() {

        int num = -1;
        
        if(Inventory.instance.equipSword) { // 새롭게 소드를 장착했다면
            for(int i = 0; i < Inventory.instance.possessItems.Count; i++) {
                if(InventoryManager.instance.inventorySlots[i].item.itemName == ItemName.활) {
                    num = i;
                }
            }
        } else if(Inventory.instance.equipBow) { // 새롭게 활을 장착했다면
            for(int i = 0; i < Inventory.instance.possessItems.Count; i++) {
                if(InventoryManager.instance.inventorySlots[i].item.itemName == ItemName.소드) {
                    num = i;
                }
            }
        }

        if(num == -1) {
            return;
        }
        
        Inventory.instance.possessItems[num].isEquipped = false;
        InventoryManager.instance.inventorySlots[num].equipImage.SetActive(false);
    }
    
    public void ConsumptionButtonClick() { // 아이템 설명 패널에서 사용 버튼을 클릭했을 경우 호출할 메소드
        
        if(slotNum != -1) { // 파라미터로 넘어온 slotNum이 존재할 경우
            if(!InventoryManager.instance.inventorySlots[slotNum].isCoolEnded) { // 쿨타임이 끝나지 않았으면 아이템이 먹어지지 않도록 돌려보내기
                return;
            }
            
            bool isUse = false;
            
            if(item != null) {
                isUse = item.Use();   
            }
            if(isUse && 1 < item.itemCount) { // 아이템이 2개 이상일 경우
                InventoryManager.instance.inventorySlots[slotNum].UseItem();
                AlertManager.instance.SmallAlertMessageOn(item.itemName, 4);
                Inventory.instance.RemoveItem(slotNum);

            } else if(isUse && item.itemCount == 1) { // 아이템이 1개만 있을 경우
                AlertManager.instance.SmallAlertMessageOn(item.itemName, 4);
                Inventory.instance.RemoveItem(slotNum);
                ItemDescriptionOnOff(-1, null); // 아이템 설명 패널을 꺼주기
            }
        }
    }
    
    public void DeleteButtonClick() { // 아이템 설명 패널에서 삭제 버튼을 클릭했을 경우 호출할 메소드

        if(item.isEquipped) { // 장착중인 아이템을 삭제하려고 할 경우
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 15);
            return;
        }
        if(slotNum != -1 && 1 < item.itemCount) { // 아이템이 2개 이상일 경우
            AlertManager.instance.SmallAlertMessageOn(item.itemName, 9);
            Inventory.instance.RemoveItem(slotNum);
            
        } else if(slotNum != -1 && item.itemCount == 1 || slotNum != -1 && item.itemCount == 0) { // 아이템이 1개만 있거나 0개 있을 경우(장비 아이템)
            AlertManager.instance.SmallAlertMessageOn(item.itemName, 9);
            Inventory.instance.RemoveItem(slotNum);
            ItemDescriptionOnOff(-1, null);
        }
    }
    
    public void LanguageButtonClick() {
        AlertManager.instance.SmallAlertMessageOn(ItemName.공백, 13);
    }

    public void GoToMainConfirmPanelOnOff() {
        if(!GameManager.instance.goToMainConfirmPanel.activeSelf) {
            GameManager.instance.goToMainConfirmPanel.SetActive(true);
        } else {
            GameManager.instance.goToMainConfirmPanel.SetActive(false);
        }
    }

    public void PanelOn() {

        // UI - Panel
        GameManager.instance.weatherPanel.SetActive(true);
        GameManager.instance.longPressBar.SetActive(true);
        
        // UI - UpLeft
        GameManager.instance.gaugePanel.SetActive(true);
        GameManager.instance.buffPanel.SetActive(true);
        GameManager.instance.questPreviewPanel.SetActive(true);
        GameManager.instance.helpButton.SetActive(true);
        GameManager.instance.fpsButton.SetActive(true);
        GameManager.instance.chatButton.SetActive(true);

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
        GameManager.instance.menuPanel.SetActive(false);
        GameManager.instance.deadPanel.SetActive(false);
        GameManager.instance.weatherPanel.SetActive(false);
        GameManager.instance.longPressBar.SetActive(false);
        
        // UI - UpLeft
        GameManager.instance.gaugePanel.SetActive(false);
        GameManager.instance.buffPanel.SetActive(false);
        GameManager.instance.frozenEffect.SetActive(false);
        GameManager.instance.speedEffect.SetActive(false);
        GameManager.instance.questPreviewPanel.SetActive(false);
        GameManager.instance.helpButton.SetActive(false);
        GameManager.instance.fpsButton.SetActive(false);
        GameManager.instance.fpsPanel.SetActive(false);
        GameManager.instance.chatButton.SetActive(false);

        // UI - UpMiddle
        GameManager.instance.goldPanel.SetActive(false);
        GameManager.instance.timePanel.SetActive(false);

        // UI - UpRight
        GameManager.instance.minimapPanel.SetActive(false);
        GameManager.instance.menuButton.SetActive(false);
        GameManager.instance.inventoryButton.SetActive(false);
        GameManager.instance.virtualButton.SetActive(false);
        GameManager.instance.equipmentButton.SetActive(false);

        // UI - MiddleLeft
        GameManager.instance.storagePanel.SetActive(false);
        GameManager.instance.equipmentPanel.SetActive(false);
        GameManager.instance.statsPanel.SetActive(false);
        GameManager.instance.statsUpButton.SetActive(false);
        GameManager.instance.questPanel.SetActive(false);
        GameManager.instance.groceryStorePanel.SetActive(false);
        GameManager.instance.equipmentStorePanel.SetActive(false);
        
        // UI - MiddleMiddle
        GameManager.instance.helpPanel.SetActive(false);
        GameManager.instance.saveMessage.SetActive(false);
        GameManager.instance.smallAlertMessage.SetActive(false);
        GameManager.instance.bigAlertMessage.SetActive(false);
        GameManager.instance.healthManaMessage.SetActive(false);
        GameManager.instance.goToMainConfirmPanel.SetActive(false);
        
        // UI - MiddleRight
        GameManager.instance.inventoryPanel.SetActive(false);
        GameManager.instance.monsterPanel.SetActive(false);
        
        // UI - BottomLeft
        GameManager.instance.virtualJoystick.SetActive(false);
        
        // UI - BottomBottom
        GameManager.instance.talkPanel.SetActive(false);
        GameManager.instance.expSlider.SetActive(false);
        GameManager.instance.chatPanel.SetActive(false);

        // UI - BottomRight
        GameManager.instance.attackButton.SetActive(false);
        GameManager.instance.actionButton.SetActive(false);
    }
    
}