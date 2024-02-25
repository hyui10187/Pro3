using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour {

    public static AlertManager instance;

    private void Awake() {
        instance = this;
    }

    public void SaveMessageOn() {
        GameManager.instance.saveMessage.SetActive(true);
        CancelInvoke("SaveMessageOff");
        Invoke("SaveMessageOff", 2f);   
    }

    private void SaveMessageOff() {
        GameManager.instance.saveMessage.SetActive(false);
    }
    
    public void FullMessageOn() {
        GameManager.instance.fullMessage.SetActive(true);
        CancelInvoke("FullMessageOff");
        Invoke("FullMessageOff", 2f);
    }
    
    private void FullMessageOff() {
        GameManager.instance.fullMessage.SetActive(false);
    }
    
    public void AcquisitionMessageOn(String itemName) {
        Text acquisitionText = GameManager.instance.acquisitionMessage.GetComponentInChildren<Text>();
        acquisitionText.text = itemName + "\n아이템을 획득하였습니다.";
        GameManager.instance.acquisitionMessage.SetActive(true);
        CancelInvoke("AcquisitionMessageOff");
        Invoke("AcquisitionMessageOff", 2f); // 2초 뒤에 아이템을 획득했다는 알림 꺼주기
    }    

    private void AcquisitionMessageOff() {
        GameManager.instance.acquisitionMessage.SetActive(false);
    }
    
    public void PurchaseMessageOn(String itemName) {
        Text purchaseText = GameManager.instance.purchaseMessage.GetComponentInChildren<Text>();
        purchaseText.text = itemName + "\n아이템을 구매하였습니다.";
        GameManager.instance.purchaseMessage.SetActive(true);
        CancelInvoke("PurchaseMessageOff"); // 우선 현재 호출중인 모든 Invoke 메소드 취소
        Invoke("PurchaseMessageOff", 2f); // 2초 뒤에 아이템을 구매했다는 알림 꺼주기
    }
    
    private void PurchaseMessageOff() {
        GameManager.instance.purchaseMessage.SetActive(false);
    }
    
    public void CantPurchaseMessageOn(int idx) {

        Text cantPurchaseText = GameManager.instance.cantPurchaseMessage.GetComponentInChildren<Text>();
        
        if(idx == 0) {
            cantPurchaseText.text = "소지금이 부족하여" + "\n아이템을 구매할 수 없습니다.";
        } else {
            cantPurchaseText.text = "인벤토리가 가득차서" + "\n아이템을 구매할 수 없습니다.";
        }
        
        GameManager.instance.cantPurchaseMessage.SetActive(true);
        CancelInvoke("CantPurchaseMessageOff"); // 우선 현재 호출중인 모든 Invoke 메소드 취소
        Invoke("CantPurchaseMessageOff", 2f); // 2초 뒤에 아이템을 먹을 수 없다는 알림 꺼주기
    }
    
    private void CantPurchaseMessageOff() {
        GameManager.instance.cantPurchaseMessage.SetActive(false);
    }
    
    public void SellMessageOn(String itemName) {
        Text sellText = GameManager.instance.sellMessage.GetComponentInChildren<Text>();
        sellText.text = itemName + "\n아이템을 판매하였습니다.";
        GameManager.instance.sellMessage.SetActive(true);
        CancelInvoke("SellMessageOff"); // 우선 현재 호출중인 모든 Invoke 메소드 취소
        Invoke("SellMessageOff", 2f); // 2초 뒤에 아이템을 구매했다는 알림 꺼주기
    }
    
    private void SellMessageOff() {
        GameManager.instance.sellMessage.SetActive(false);
    }
    
    public void ConsumptionMessageOn(String itemName) {
        Text consumptionText = GameManager.instance.consumptionMessage.GetComponentInChildren<Text>();
        consumptionText.text = itemName + "\n아이템을 사용하였습니다.";
        GameManager.instance.consumptionMessage.SetActive(true);
        CancelInvoke("ConsumptionMessageOff"); // 우선 현재 호출중인 모든 Invoke 메소드 취소
        Invoke("ConsumptionMessageOff", 2f); // 2초 뒤에 아이템을 구매했다는 알림 꺼주기
    }
    
    private void ConsumptionMessageOff() {
        GameManager.instance.consumptionMessage.SetActive(false);
    }

    public void CantAttackMessageOn() {
        GameManager.instance.cantAttackMessage.SetActive(true); // 공격불가 알림메시지 켜주기
        CancelInvoke("CantAttackMessageOff");
        Invoke("CantAttackMessageOff", 2f);
    }

    private void CantAttackMessageOff() {
        GameManager.instance.cantAttackMessage.SetActive(false);
    }
    
    public void HealthMessageOn() {
        Animator anim = GameManager.instance.healthManaMessageText.GetComponent<Animator>();
        GameManager.instance.healthManaMessage.SetActive(true); // 닳은 체력을 띄워주는 메시지 켜주기
        anim.SetTrigger("health");
        
        CancelInvoke("HealthMessageOff");
        Invoke("HealthMessageOff", 2f);
    }

    private void HealthMessageOff() {
        GameManager.instance.healthManaMessage.SetActive(false);
    }
    
    public void ManaMessageOn() {
        Animator anim = GameManager.instance.healthManaMessageText.GetComponent<Animator>();
        GameManager.instance.healthManaMessage.SetActive(true); // 닳은 체력을 띄워주는 메시지 켜주기
        anim.SetTrigger("mana");
        CancelInvoke("ManaMessageOff");
        Invoke("ManaMessageOff", 2f);
    }

    private void ManaMessageOff() {
        GameManager.instance.healthManaMessage.SetActive(false);
    }
    
}