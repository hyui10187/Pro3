using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour {

    public static AlertManager instance;

    public Dictionary<int, string> alertData;
    
    private void Awake() {
        alertData = new Dictionary<int, string>();
        GenerateData();
        instance = this;
    }

    private void GenerateData() {
        alertData.Add(0, " 획득");
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
        alertData.Add(11, "골드 획득");
        alertData.Add(12, "레벨이 올랐습니다.");
        alertData.Add(13, "아직 지원하지 않는 기능입니다.");
        alertData.Add(14, "장착중인 아이템은 창고에 맡길 수 없습니다.\n먼저 장착을 해제해주세요.");
        alertData.Add(15, "장착중인 아이템은 삭제하실 수 없습니다.\n먼저 장착을 해제해주세요.");
        alertData.Add(16, "퀘스트 아이템은 판매하실 수 없습니다.");
        alertData.Add(17, "소지한 아이템의 갯수보다 많이 맡기실 수 없습니다.");
        
        alertData.Add(21, "[도움말]\n- 집 밖에 있을 경우에는 추위로 인해\n체력이 지속적으로 감소합니다.\n\n- 인벤토리창에서 아이템을 길게 누를 경우\n아이템에 대한 설명창을 볼 수 있습니다.\n\n- 화면 왼쪽 상단에 표시된 퀘스트를 따라\n게임을 진행하세요.");
        alertData.Add(22, "[도움말]\n- 벽난로와 상호작용을 하면 체력을\n회복할 수 있습니다.\n\n- 침대에서 잠을 자게 되면 체력과 마나를\n모두 회복할 수 있습니다.\n\n- 집 밖에 있는 덤불이나 나무 밑동을\n공격하면 재료 아이템을 얻을 수 있습니다.");
        alertData.Add(23, "[도움말]\n- 광장 혹은 방에 있는 괘종시계와 상호\n작용을 하면 현재 시간을 알 수 있습니다.\n\n- 집 안에는 위험한 공간도 있으니 주의\n하세요.\n\n- 물약중에는 먹으면 즉사하는 것도\n있으니 아이템 설명을 잘 읽어보세요.");
        alertData.Add(24, "[미니맵]\n- 초록색 : 플레이어 캐릭터입니다.\n\n- 보라색 : 상호작용 할 수 있는 사물입니다.\n\n- 하얀색 : 기능을 가진 NPC 혹은 사물\n입니다.\n\n- 분홍색 : 이동할 수 있는 포인트입니다.");
        alertData.Add(25, "[미니맵]\n- 파란색 : 획득할 수 있는 아이템입니다.\n\n- 빨간색 : 플레이어에게 피해를 끼칠 수\n있는 몬스터 혹은 장애물입니다.\n\n- 노란색 : 대화할 수 있는 NPC 입니다.");
        alertData.Add(26, "[모바일 조작키]\n- HP MP 버튼 : 캐릭터의 스탯창을\n열거나 닫습니다.\n\n- 퀘스트 버튼 : 퀘스트창을 열거나 닫습\n니다.\n\n- 퀘스트 화살표 버튼 : 진행중인 퀘스트를\n펼치거나 접습니다.");
        alertData.Add(27, "[모바일 조작키]\n- FPS 버튼 : 현재 게임의 프레임 수를\n보여줍니다.\n\n- 톱니바퀴 버튼 : 메뉴창을 열거나 닫습\n니다.\n\n- 상자 버튼 : 인벤토리창을 열거나 닫습\n니다.");
        alertData.Add(28, "[모바일 조작키]\n- 조이스틱 버튼 : 모바일용 가상키를\n켜거나 끕니다.\n\n- 갑옷 버튼 : 장비창을 열거나 닫습니다.\n\n- 무기 버튼 : 장착중인 무기로 적을\n공격을 합니다.");
        alertData.Add(29, "[모바일 조작키]\n- 미니맵 버튼 : 지도를 펼쳐서 현재\n위치를 확인합니다.\n\n- 말풍선 버튼 : NPC 혹은 사물과 상호\n작용을 합니다.");
        alertData.Add(30, "[PC 조작키]\n- ESC 키 : 메뉴창을 켜거나 끄거나\n혹은 열려있는 모든 창을 닫습니다.\n\n- 스페이스 바 키 : NPC 혹은 사물과 상호\n작용을 합니다.\n\n- E 키 : 장비창을 열거나 닫습니다.");
        alertData.Add(31, "[PC 조작키]\n- I 키 : 인벤토리 창을 열거나 닫습니다.\n\n- A 키 : 장착중인 무기로 적을 공격합니다.\n\n- S 키 : 스탯창을 열거나 닫습니다.\n\n- Q 키 : 퀘스트창을 열거나 닫습니다.");
    }
    
    public void SaveMessageOn() {
        GameManager.instance.saveMessage.SetActive(true);
        CancelInvoke("SaveMessageOff");
        Invoke("SaveMessageOff", 2f);   
    }

    private void SaveMessageOff() {
        GameManager.instance.saveMessage.SetActive(false);
    }
    
    public void LevelUPMessageOn() {
        GameManager.instance.levelUpMessage.SetActive(true);
        CancelInvoke("LevelUPMessageOff");
        Invoke("LevelUPMessageOff", 2f);   
    }
    
    private void LevelUPMessageOff() {
        GameManager.instance.levelUpMessage.SetActive(false);
    }
    
    public void SmallAlertMessageOn(String itemName, int idx) {
        Text smallText = GameManager.instance.smallAlertMessage.GetComponentInChildren<Text>();
        smallText.text = itemName + alertData[idx];
        GameManager.instance.smallAlertMessage.SetActive(true);
        CancelInvoke("SmallAlertMessageOff");
        Invoke("SmallAlertMessageOff", 2f); // 2초 뒤에 아이템을 획득했다는 알림 꺼주기
    }

    private void SmallAlertMessageOff() {
        GameManager.instance.smallAlertMessage.SetActive(false);
    }

    public void AcquisitionMessageOn(String itemName, int idx) {
        Text acquisitionText = GameManager.instance.acquisitionMessage.GetComponentInChildren<Text>();
        acquisitionText.text = itemName + alertData[idx];
        GameManager.instance.acquisitionMessage.SetActive(true);
        CancelInvoke("AcquisitionMessageOff");
        Invoke("AcquisitionMessageOff", 2f); // 2초 뒤에 아이템을 획득했다는 알림 꺼주기
    }

    private void AcquisitionMessageOff() {
        GameManager.instance.acquisitionMessage.SetActive(false);
    }
    
    public void BigAlertMessageOn(String itemName, int idx) {
        Text bigAlertText = GameManager.instance.bigAlertMessage.GetComponentInChildren<Text>();
        bigAlertText.text = itemName + alertData[idx];
        GameManager.instance.bigAlertMessage.SetActive(true);
        CancelInvoke("BigAlertMessageOff");
        Invoke("BigAlertMessageOff", 2f); // 2초 뒤에 아이템을 획득했다는 알림 꺼주기
    }

    private void BigAlertMessageOff() {
        GameManager.instance.bigAlertMessage.SetActive(false);
    }
    
    public void LongAlertMessageOn(String itemName, int idx) { // 알림 문구가 긴것을 위한 긴 패널
        Text acquisitionText = GameManager.instance.longAlertMessage.GetComponentInChildren<Text>();
        acquisitionText.text = itemName + alertData[idx];
        GameManager.instance.longAlertMessage.SetActive(true);
        CancelInvoke("LongAlertMessageOff");
        Invoke("LongAlertMessageOff", 2f); // 2초 뒤에 아이템을 획득했다는 알림 꺼주기
    }

    private void LongAlertMessageOff() {
        GameManager.instance.longAlertMessage.SetActive(false);
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