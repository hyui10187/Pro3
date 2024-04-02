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
    public GameObject questHeader;
    public GameObject curQuestName;
    public GameObject questPanelButton;
    public GameObject offButton;
    public GameObject onButton;
    public GameObject exclamationPanel;

    [Header("Quest Item")]
    public GameObject coin;
    public GameObject ring;
    public GameObject sword;
    public GameObject necklace;
    public GameObject storeKey;
    public GameObject chestKey;
    public GameObject candy;
    public GameObject questPot;
    
    public Transform[] npcTransforms;
    
    private Dictionary<int, QuestData> questList;
    private Dictionary<int, string> questText;

    private void Awake() {
        questList = new Dictionary<int, QuestData>();
        questText = new Dictionary<int, string>();
        GenerateQuestList();
        GenerateQuestText();
        instance = this;
    }

    private void GenerateQuestList() {
        questList.Add(10, new QuestData("카밀과 대화하기", new int[] { 10000 })); // 카밀
        questList.Add(20, new QuestData("루나와 대화하기", new int[] { 20000 })); // 루나
        questList.Add(30, new QuestData("루나의 은화 찾아주기", new int[] { 0, 20000 })); // 은화, 루나
        questList.Add(40, new QuestData("촌장의 근심거리 듣기", new int[] { 110000 })); // 촌장
        questList.Add(50, new QuestData("몬스터 처치하기", new int[] { 0 })); // 몬스터
        questList.Add(60, new QuestData("촌장의 보답", new int[] { 110000 })); // 촌장
        questList.Add(70, new QuestData("조니의 근황 듣기", new int[] { 180000 })); // 조니
        questList.Add(80, new QuestData("콜린의 선물 받기", new int[] { 80000 })); // 콜린
        questList.Add(90, new QuestData("써니에게 사탕주기", new int[] { 0, 120000 })); // 상자, 써니
        questList.Add(100, new QuestData("대니의 부탁 들어주기", new int[] { 140000, 7300 })); // 대니, 항아리
        questList.Add(110, new QuestData("대니에게 알려주기", new int[] { 140000 })); // 대니
        questList.Add(120, new QuestData("퀘스트", new int[] { 0 }));
    }

    private void GenerateQuestText() {
        questText.Add(10, " 이 마을은 처음이라 무척\n낯설어. 우선 그린 수도사\n님이 말씀하신 카밀이라는\n사람을 찾아보자. 그 사람과\n먼저 대화를 해봐야겠어.");
        questText.Add(20, " 벽난로 옆에 있는 루나라는\n여자에게 걱정이 있는것 같\n다고 카밀이 알려줬어. 무슨\n일인지 한번 물어봐야겠어.\n걱정을 들어주다보면 친밀\n감을 쌓을 수 있을지도 몰라.");
        questText.Add(30, " 루나에게 물어보니 귀중한\n은화를 잃어버렸다고 하네.\n뭔가 사연이 있는 은화인것\n같으니 어서 찾아줘야겠어.\n석상이 있는 방에서 잃어버\n린것 같다고하니 잘 찾아보\n자.");
        questText.Add(40, " 이 마을의 촌장님에게 근심\n거리가 있다고 하네. 무슨\n근심인지 한번 들어봐야겠\n어. 촌장님은 이 집에 들어\n오는 입구에 서 계셨던것\n같아.");
        questText.Add(50, " 촌장님을 근심하게 만드는\n마을의 몬스터들을 처치하\n자. 밖의 추위가 매서운데\n그런 상황에서 몬스터까지\n전부 쓰러뜨리려면 쉽지\n않은 전투가 될것 같아.\n중간중간 물약을 사용하며\n버텨야겠어.");
        questText.Add(60, " 마을을 위협하는 몬스터들\n을 모두 쓰러뜨렸어. 쉽지\n않은 전투였지만 보람이 있\n네. 촌장님이 보답을 하고 싶\n다고 하니 촌장님을 찾아가\n보자.");
        questText.Add(70, " 촌장님이 잠겨있는 문을 열\n수 있는 열쇠를 주셨어. 아무\n나 들어갈 수 없는 공간이라\n록 허락해 주셨다는 것은 어\n느정도 신임을 얻었다는 것\n이겠지. 그곳에 있는 촌장님\n의 조카인 조니의 근황도 물\n어보자.");
        questText.Add(80, " 여관의 숙박을 담당하는 콜\n린이 나에게 보답을 하고 싶\n다고 하네. 내가 콜린에게\n뭔가 해준것은 없는데 무슨\n보답을 해주려는 것일까?\n우선 콜린을 찾아가보자.");
        questText.Add(90, " 복도처럼 생긴 방에 있는\n꼬마 여자아이 써니는 사탕\n을 먹고 싶다고 했었지. 콜\n린이 준 열쇠로 상자를 열다\n보면 사탕을 발견할 수도 있\n지 않을까? 써니가 그토록\n사탕을 먹고 싶어하니 하나\n선물로 주는 것도 좋을것\n같네.");
        questText.Add(100, " 마을의 주방장인 대니가 나\n에게 부탁을 할것이 있다고\n하네. 예전에 봤을때는 재료\n준비로 인해 엄청 바빠보였\n는데 요리에 관한 부탁을\n하려는 것일까? 우선 대니를\n찾아가보자.");
        questText.Add(110, " 대니의 부탁으로 카리나 옆\n에 있는 항아리에 담긴 정향\n을 가지러 갔어. 그런데 항아\n리가 깨져있잖아? 우선 지나\n다니는 사람들이 다치지 않\n도록 조각을 치우긴 했어.\n대니에게 이 사실을 말하주\n러 가야겠어.");
        questText.Add(120, "\n\n\n\n\n\n\n\n\n\n");
    }

    public int QuestIdPlusQuestActionIndex() {
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
        GameManager.instance.questTitle.text = questList[questId].questName;
        GameManager.instance.questText.text = questText[questId];
        return questList[questId].questName; // 현재 진행중인 퀘스트명 반환
    }

    private void NextQuest() {
        questId += 10;
        questActionIndex = 0;
        GameManager.instance.questTitle.text = questList[questId].questName;
        GameManager.instance.questText.text = questText[questId];
    }

    private void ControlExclamationPanel() {
        exclamationPanel.transform.position = npcTransforms[questActionIndex].position + Vector3.up;
    }

    public void ControlObject() {

        switch(questId) {
            
            case 10: // 카밀과 대화하기
                if(questActionIndex == 0) {
                    ControlExclamationPanel();
                    exclamationPanel.SetActive(true);
                } else { // 카밀과 대화가 끝나면 루나 머리위로 느낌표를 옮겨주기
                    ControlExclamationPanel();
                }
                break;
            
            case 20: // 루나와 대화하기
                if(questActionIndex == 1 && coin != null) { // 루나와 대화가 끝나고 나면
                    coin.SetActive(true); // 동전 켜주기
                    exclamationPanel.transform.position = coin.transform.position + Vector3.up;
                }
                break;
                
            case 30: // 루나의 은화 찾아주기
                if(questActionIndex == 0 && coin != null) { // 게임을 저장하고 로드했을 경우를 대비하여
                    coin.SetActive(true); // 동전 켜주기
                }

                if(questActionIndex == 1) {
                    ControlExclamationPanel();
                }
                if(questActionIndex == 2) { // 코인, 루나와 대화를 마치고 나면
                    int num = Inventory.instance.possessItems.Count;
                    
                    for(int i = 0; i < num; i++) { // 퀘스트 아이템인 은화를 인벤토리에서 제거하기
                        if(Inventory.instance.possessItems[i].itemType == ItemType.Quest) {
                            Inventory.instance.RemoveItem(i);
                            break;
                        }
                    }
                    
                    GameManager.instance.curExp += 50;
                    ControlExclamationPanel();
                    ring.SetActive(true); // 루나 앞에 꺼져있던 반지를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;
            
            case 40: // 촌장의 근심거리 듣기
                if(questActionIndex == 1) { // 촌장과 대화가 끝나면
                    exclamationPanel.SetActive(false);
                    SpawnManager.instance.GenerateEnemy(); // 몬스터를 전부 켜주기
                    GameManager.instance.monsterPanel.SetActive(true); // 몬스터 패널을 켜주기
                    sword.SetActive(true); // 촌장 앞에 꺼져있던 무기를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;

            case 50: // 촌장의 근심거리 듣기
                if(questActionIndex == 1) { // 촌장과 대화가 끝나면
                    exclamationPanel.SetActive(true);
                    exclamationPanel.transform.position = npcTransforms[3].position + Vector3.up;
                }
                break;
            
            case 60: // 촌장의 보답
                if(questActionIndex == 1) {
                    storeKey.SetActive(true); // 촌장 앞에 꺼져있던 열쇠를 켜줘서 플레이어가 먹을 수 있도록 해주기
                    GameManager.instance.monsterPanel.SetActive(false); // 몬스터 패널을 꺼주기
                    exclamationPanel.transform.position = npcTransforms[4].position + Vector3.up;
                }
                break;
            
            case 80: // 콜린의 선물받기
                if(questActionIndex == 1) {
                    chestKey.SetActive(true); // 콜린 앞에 꺼져있던 열쇠를 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;
            
            case 90: // 써니에게 사탕주기
                if(questActionIndex == 2) {
                    
                    int num = Inventory.instance.possessItems.Count;
                    
                    for(int i = 0; i < num; i++) { // 퀘스트 아이템인 사탕을 인벤토리에서 제거하기
                        if(Inventory.instance.possessItems[i].itemType == ItemType.Quest) {
                            Inventory.instance.RemoveItem(i);
                            break;
                        }
                    }
                    GameManager.instance.curExp += 50;
                    necklace.SetActive(true); // 써니 앞에 꺼져있던 목걸이 켜줘서 플레이어가 먹을 수 있도록 해주기
                }
                break;
            
            case 100: // 대니의 부탁 들어주기
                if(questActionIndex == 1) {
                    questPot.transform.GetChild(0).gameObject.SetActive(false); // 멀쩡한 항아리 꺼주기
                    questPot.transform.GetChild(1).gameObject.SetActive(true); // 깨진 항아리 켜주기
                } else if(questActionIndex == 2) {
                    questPot.SetActive(false); // 깨진 항아리와 대화가 끝나면 꺼주기
                }
                break;
            
            case 110:
                break;
        }
    }

    public void OffQuest() {
        questHeader.SetActive(false);
        curQuestName.SetActive(false);
        questPanelButton.SetActive(false);
        offButton.SetActive(false);
        onButton.SetActive(true);
    }

    public void OnQuest() {
        questHeader.SetActive(true);
        curQuestName.SetActive(true);
        questPanelButton.SetActive(true);
        offButton.SetActive(true);
        onButton.SetActive(false);
    }
    
}