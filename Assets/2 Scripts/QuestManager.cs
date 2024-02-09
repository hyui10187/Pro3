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
    
    private Dictionary<int, QuestData> questList;

    private void Awake() {
        questItem = new List<GameObject>();
        questList = new Dictionary<int, QuestData>();
        GenerateData();
        GenerateQuestItem();
        instance = this;
    }

    private void GenerateData() {
        questList.Add(10, new QuestData("콜린과 대화하기", new int[] { 10000, 20000 })); // 콜린, 루나
        questList.Add(20, new QuestData("루나의 동전 찾아주기", new int[] { 1000, 20000 })); // 코인, 루나
        questList.Add(30, new QuestData("마을의 근심거리 듣기", new int[] { 10000, 20000 })); // 콜린
        questList.Add(40, new QuestData("몬스터 처치하기", new int[] { 0 }));
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
            case 10:
                if(questActionIndex == 2) { // 대화를 2번 모두 마쳤을때
                    questItem[0].SetActive(true); // 동전 켜주기
                }
                break;
            
            case 20:
                if(questActionIndex == 0) { // 게임을 저장하고 로드했을 경우를 대비하여
                    questItem[0].SetActive(true); // 동전 켜주기
                }
                if(questActionIndex == 2) {

                    int num = Inventory.instance.possessItems.Count;
                    
                    for(int i = 0; i < num; i++) { // 퀘스트 아이템 coin 인벤토리에서 제거하기
                        if(Inventory.instance.possessItems[i].itemType == Item.ItemType.Quest) {
                            Inventory.instance.RemoveItem(i);
                            break;
                        }
                    }
                    
                    ring.SetActive(true); // 꺼져있던 반지를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;
                
            case 30:
                if(questActionIndex == 1) { // 콜린과 대화가 끝나면
                    SpawnManager.instance.GenerateEnemy(); // 몬스터를 전부 켜주기
                    GameManager.instance.isMonsterPanelOn = true; // 몬스터 패널을 켜주기
                    sword.SetActive(true); // 꺼져있던 무기를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                
                break;
            
            default:
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