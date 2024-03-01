using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour {

    public static AlertManager instance;

    private Dictionary<int, string> alertData;
    
    private void Awake() {
        alertData = new Dictionary<int, string>();
        GenerateData();
        instance = this;
    }

    private void GenerateData() {
        alertData.Add(0, "\n아이템을 획득하였습니다.");
        alertData.Add(1, "\n아이템을 구매하였습니다.");
        alertData.Add(2, "\n아이템을 판매하였습니다.");
        alertData.Add(3, "\n아이템을 창고에 맡겼습니다.");
        alertData.Add(4, "\n아이템을 사용하였습니다.");
        alertData.Add(5, "인벤토리가 가득차서\n아이템을 획득할 수 없습니다.");
        alertData.Add(6, "무기를 장착하지 않아서\n공격할 수 없습니다.");
        alertData.Add(7, "소지금이 부족하여\n아이템을 구매할 수 없습니다.");
        alertData.Add(8, "인벤토리가 가득차서\n아이템을 구매할 수 없습니다.");
        alertData.Add(9, "\n아이템을 삭제하였습니다.");
        alertData.Add(10, "\n아이템을 창고에서 찾았습니다.");
        alertData.Add(11, "골드를 획득했습니다.");
        alertData.Add(12, "레벨이 올랐습니다.");
    }
    
    public void SaveMessageOn() {
        GameManager.instance.saveMessage.SetActive(true);
        CancelInvoke("SaveMessageOff");
        Invoke("SaveMessageOff", 2f);   
    }

    private void SaveMessageOff() {
        GameManager.instance.saveMessage.SetActive(false);
    }

    public void AlertMessageOn(String itemName, int idx) {
        Text acquisitionText = GameManager.instance.alertMessage.GetComponentInChildren<Text>();
        acquisitionText.text = itemName + alertData[idx];
        GameManager.instance.alertMessage.SetActive(true);
        CancelInvoke("AlertMessageOff");
        Invoke("AlertMessageOff", 2f); // 2초 뒤에 아이템을 획득했다는 알림 꺼주기
    }    

    private void AlertMessageOff() {
        GameManager.instance.alertMessage.SetActive(false);
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