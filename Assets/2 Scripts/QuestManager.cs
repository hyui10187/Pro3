using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public static QuestManager instance;
    
    [Header("Quest Info")]
    public int questId;
    public int questActionIndex;

    [Header("UI")]
    public GameObject quest;
    public GameObject questText;
    public GameObject offButton;
    public GameObject onButton;
    
    [Header("Quest Item")]
    public List<GameObject> questItem;
    public GameObject coinPrefab;
    public Transform[] questItemSpawnPos;
    public GameObject questItemParent;
    public GameObject ring;
    public GameObject sword;
    public GameObject storeKey;
    public GameObject chestKey;
    public GameObject candy;
    
    private Dictionary<int, QuestData> questList;

    private void Awake() {
        questItem = new List<GameObject>();
        questList = new Dictionary<int, QuestData>();
        GenerateData();
        GenerateQuestItem();
        instance = this;
    }

    private void GenerateData() {
        questList.Add(10, new QuestData("카밀과 대화하기", new int[] { 10000 })); // 카밀
        questList.Add(20, new QuestData("루나와 대화하기", new int[] { 20000 })); // 루나
        questList.Add(30, new QuestData("루나의 은화 찾아주기", new int[] { 1000, 20000 })); // 코인, 루나
        questList.Add(40, new QuestData("촌장의 근심거리 듣기", new int[] { 110000 })); // 촌장
        questList.Add(50, new QuestData("몬스터 처치하기", new int[] { 0 }));
        questList.Add(60, new QuestData("촌장의 보답", new int[] { 110000 }));
        questList.Add(70, new QuestData("조니의 근황 듣기", new int[] { 180000 }));
        questList.Add(80, new QuestData("콜린의 선물 받기", new int[] { 80000 }));
        questList.Add(90, new QuestData("써니에게 사탕주기", new int[] { 120000 }));
        questList.Add(100, new QuestData("대니의 부탁 들어주기", new int[] { 140000 }));
        questList.Add(110, new QuestData("퀘스트", new int[] { 0 }));
    }

    private void GenerateQuestItem() {
        GameObject coin = Instantiate(coinPrefab, questItemSpawnPos[0].position, quaternion.identity, questItemParent.transform);
        coin.SetActive(false);
        questItem.Add(coin);
    }
    
    public int GetQuestTalkIndex() {
        return questId + questActionIndex;
    }

    public string CheckQuest(int objId) { // CheckQuest 메소드가 호출되었다는 것은 이전 NPC와는 대화가 전부 끝난것
        
        if(objId == questList[questId].npcId[questActionIndex] && GameManager.instance.canEatQuestItem) { // 퀘스트 아이템을 먹을 수 없는 상태이면 questActionIndex를 증가시키지 않도록
            questActionIndex++; // 다음 퀘스트 대화상대로 넘겨줌
        }

        // Control Quest Object
        ControlObject();

        // Talk Complete & Next Quest
        if(questActionIndex == questList[questId].npcId.Length) // 퀘스트 대화 상대와 전부 대화를 나눴으면 다음 퀘스트로 넘어감
            NextQuest();
        
        return questList[questId].questName; // 현재 진행중인 퀘스트명 반환
    }
    
    public string CheckQuest() { // 오버로딩

        // Return Quest Name
        return questList[questId].questName; // 현재 진행중인 퀘스트명 반환
    }

    private void NextQuest() {
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObject() {

        switch(questId) {

            case 20: // 루나와 대화하기
                if(questActionIndex == 1 && questItem[0] != null) { // 루나와 대화가 끝나고 나면
                    questItem[0].SetActive(true); // 동전 켜주기
                }
                break;
                
            case 30: // 루나의 은화 찾아주기
                if(questActionIndex == 0 && questItem[0] != null) { // 게임을 저장하고 로드했을 경우를 대비하여
                    questItem[0].SetActive(true); // 동전 켜주기
                }
                if(questActionIndex == 2) { // 코인, 루나와 대화를 마치고 나면
                
                    int num = Inventory.instance.possessItems.Count;
                    
                    for(int i = 0; i < num; i++) { // 퀘스트 아이템인 coin을 인벤토리에서 제거하기
                        if(Inventory.instance.possessItems[i].itemType == ItemType.Quest) {
                            Inventory.instance.RemoveItem(i);
                            break;
                        }
                    }
                    
                    ring.SetActive(true); // 루나 앞에 꺼져있던 반지를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;
            
            case 40: // 촌장의 근심거리 듣기
                if(questActionIndex == 1) { // 촌장과 대화가 끝나면
                    SpawnManager.instance.GenerateEnemy(); // 몬스터를 전부 켜주기
                    GameManager.instance.monsterPanel.SetActive(true); // 몬스터 패널을 켜주기
                    sword.SetActive(true); // 촌장 앞에 꺼져있던 무기를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;

            case 60: // 몬스터 처치하기
                if(questActionIndex == 1) {
                    storeKey.SetActive(true); // 촌장 앞에 꺼져있던 열쇠를 켜줘서 플레이어가 먹을 수 있도록 해주기
                    GameManager.instance.monsterPanel.SetActive(false); // 몬스터 패널을 꺼주기
                }
                break;
            
            case 80:
                if(questActionIndex == 1) {
                    chestKey.SetActive(true); // 콜린 앞에 꺼져있던 열쇠를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;
        }
    }

    public void OffQuest() {
        quest.SetActive(false);
        questText.SetActive(false);
        onButton.SetActive(true);
        offButton.SetActive(false);
    }

    public void OnQuest() {
        quest.SetActive(true);
        questText.SetActive(true);
        onButton.SetActive(false);
        offButton.SetActive(true);
    }
    
}