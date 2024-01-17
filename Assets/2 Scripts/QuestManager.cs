using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuestManager : MonoBehaviour {

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
    
    private Dictionary<int, QuestData> questList;

    private void Awake() {
        questItem = new List<GameObject>();
        questList = new Dictionary<int, QuestData>();
        GenerateData();
        GenerateQuestItem();
    }

    private void GenerateData() {
        questList.Add(10, new QuestData("콜린과 대화하기", new int[] { 10000, 20000 }));
        questList.Add(20, new QuestData("루나의 동전 찾아주기", new int[] { 30000, 20000 }));
        questList.Add(30, new QuestData("퀘스트 올 클리어", new int[] { 0 }));
    }

    private void GenerateQuestItem() {
        GameObject coin = Instantiate(coinPrefab, questItemSpawnPos[0].position, quaternion.identity, questItemParent.transform);
        coin.SetActive(false);
        questItem.Add(coin);
    }
    
    public int GetQuestTalkIndex(int objId) {
        return questId + questActionIndex;
    }

    public string CheckQuest(int objId) { // CheckQuest 메소드가 호출되었다는 것은 이전 NPC와는 대화가 전부 끝난것
        
        // Next Talk Target
        if(objId == questList[questId].npcId[questActionIndex]) // 다음 퀘스트 대화상대로 넘겨줌
            questActionIndex++;
        
        // Control Quest Object
        ControlObject();

        // Talk Complete & Next Quest
        if(questActionIndex == questList[questId].npcId.Length) // 퀘스트 대화 상대와 전부 대화를 나눴으면 다음 퀘스트로 넘어감
            NextQuest();

        // Return Quest Name
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

    private void ControlObject() {

        switch(questId) {
            case 10:
                if(questActionIndex == 2) { // 대화를 2번 모두 마쳤을때
                    questItem[0].SetActive(true); // 동전 켜주기
                }
                break;
            
            case 20:
                if(questActionIndex == 1) { // 동전을 먹었으면
                    questItem[0].SetActive(false); // 동전 꺼주기
                } else if(questActionIndex == 2) {

                    int num = Inventory.instance.possessItems.Count;
                    
                    for(int i = 0; i < num; i++) {
                        if(Inventory.instance.possessItems[i].itemType == Item.ItemType.Quest) {
                            Inventory.instance.RemoveItem(i);
                        }
                    }
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