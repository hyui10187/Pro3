using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    
    [Header("Manager")]
    public TalkManager talkManager;
    public QuestManager questManager;
    public InventoryManager inventoryManager;

    [Header("Move Position")]
    public Transform housePos;
    public Transform winterFieldPos;
    public Transform[] upStairPos;
    public Transform[] upLadderPos;
    public Transform[] downStairPos;
    public Transform[] downLadderPos;

    [Header("UI")]
    public GameObject startPanel; // 게임 시작 패널
    public GameObject virtualPanel;
    public GameObject helpPanel;
    public GameObject menuPanel;
    public GameObject talkPanel; // 대화창
    public GameObject buffPanel;
    public GameObject deadPanel;
    public Text talkNpc; // 대화창의 NPC 이름
    public Text talkText; // 대화창의 대화 내용
    public GameObject timePanel; // 시계 패널
    public GameObject questPanel;
    public GameObject monsterPanel;
    public GameObject keyBoardButton;
    public GameObject helpButton;
    public Text currentQuestText; // 현재 진행중인 퀘스트 이름
    public GameObject gaugeUI;
    public GameObject weatherUI; // 날씨
    public GameObject frozenEffect;
    public GameObject speedEffect;
    public GameObject expSlider;
    public GameObject saveMessage;
    public GameObject fullMessage;

    [Header("Game Control")]
    public float curGameTime; // 현재 게임시간

    [Header("Player Info")]
    public GameObject player;
    public float curMoveSpeed;
    public float originMoveSpeed;
    public float curHealth;
    public float maxHealth;
    public float curMana;
    public float maxMana;
    public float curExp; // 플레이어의 현재 경험치
    public float maxExp; // 레벨업을 하기 위한 경험치
    public int level;

    [Header("Flag")]
    public bool isLive; // 게임이 진행중인지 체크하는 플래그
    public bool isAction; // 대화를 하는중인지 체크하기 위한 플래그값
    public bool isHouse; // 집에 들어갔는지 체크하기 위한 플래그값
    public bool hasQuestItem;
    public bool isMonsterPanelOn;
    public bool isMenuPanelOn;
    public bool isNewGame;
    public bool canEatQuestItem = true; // 퀘스트 아이템을 먹을 수 있는지 플래그값

    [Header("Etc")]
    public int talkIndex;
    public GameObject scanObject; // 스캔한 게임 오브젝트
    
    private void Awake() {
        instance = this;
    }

    private void Start() {

        if(!isNewGame) {
            GameLoad();
        }
        
        curHealth = maxHealth; // 게임 처음 시작시 현재 체력(health)을 최대 체력(maxHealth)으로 초기화
        curMoveSpeed = originMoveSpeed; // 게임 처음 시작시 현재 이동속도(curMoveSpeed)를 기본 이동속도(originMoveSpeed)로 초기화
        //curMana = maxMana; // 게임 처음 시작시 현재 마나(mana)를 최대 마나(maxMana)로 초기화
        currentQuestText.text = questManager.CheckQuest();
    }


    private void Update() {

        if(Player.instance.isDead || !isLive) {
            return;
        }

        ControlLevel();
        ControlConditionUI();
        curGameTime += Time.deltaTime;
    }

    public void Action(GameObject scanObj) {
        
        if(Player.instance.isDead) {
            return;
        }
        
        expSlider.SetActive(false);
        scanObject = scanObj;
        
        if(scanObject.CompareTag("Clock")) { // 괘종시계에 말을 걸었으면
            timePanel.SetActive(true); // 시계 패널을 켜주기
            
        }
        
        ObjData objData = scanObject.GetComponent<ObjData>();

        if(scanObject.CompareTag("Heal")) { // 모닥불에게 말을 걸었을 경우
            curHealth += 10; // 체력을 20 회복시키기
            
            if(curHealth > maxHealth) { // +20 된 체력이 최대 체력보다 크다면
                curHealth = maxHealth; // 플레이어의 현재 체력을 최대 체력으로 바꿔주기
            }
        } else if(scanObject.CompareTag("QuestItem") && !hasQuestItem) { // 퀘스트 coin 아이템을 먹었을 경우

            if(Inventory.instance.possessItems.Count < Inventory.instance.CurSlotCnt) { // 인벤토리에 슬롯 여분이 있을 경우
                FieldItems fieldItems = scanObject.GetComponent<FieldItems>();
                Inventory.instance.AddItem(fieldItems.GetItem());
                hasQuestItem = true;
                canEatQuestItem = true;

            } else { // 인벤토리가 꽉 차 있을 경우
                Inventory.instance.FullMessageOn();
                canEatQuestItem = false;
            }

        } else if(scanObject.layer == 10) { // layer 10번은 NPC == 이동형 NPC
            NPC npc = scanObject.GetComponent<NPC>();
            npc.isCollision = true;
            npc.CancelInvoke();
        }
        
        Talk(objData.objId, objData.isNpc, scanObject.name);
        talkPanel.SetActive(isAction); // 대화창을 끄고 켜는 것은 isAction 플래그 값이랑 동일하다
    }

    public void ControlInventory() {
        inventoryManager.OnOffInventory();
    }

    public void OffInventory() {
        inventoryManager.OffInventory();
    }

    private void Talk(int objId, bool isNpc, string npcName) {
        
        int questTalkIndex = questManager.GetQuestTalkIndex();
        string talkData = talkManager.GetTalk(objId + questTalkIndex, talkIndex); // 대상의 ID와 QuestTalkIndex를 더한 값을 첫번째 파라미터로 던져준다

        // End Talk
        if(talkData == null) { // 더이상 다음 대화가 없다면
            isAction = false; // isAction을 false로 줘서 대화창 끄기
            timePanel.SetActive(false); // 대화가 끝났을때는 시계 패널은 항상 꺼주는 것으로 처리
            talkIndex = 0; // 대화가 끝나면 talkIndex 초기화
            currentQuestText.text = questManager.CheckQuest(objId); // 다음에 진행할 퀘스트명을 UI에 뿌려줌

            if(hasQuestItem) {
                QuestManager.instance.questItem[0].SetActive(false); // coin 꺼주기
            }

            if(isNpc && objId == 30000) {
                NPC npc = scanObject.GetComponent<NPC>();
                npc.isCollision = false;
                npc.Think();
            }
            
            return;
        }
        
        if(isNpc) { // 대화를 한게 NPC일 경우에는 NPC의 이름도 같이 보여주기
            talkNpc.text = npcName;
            talkNpc.gameObject.SetActive(true); // NPC 표시창도 켜주기
            talkText.text = talkData;
        } else {
            talkNpc.gameObject.SetActive(false);
            talkText.text = talkData; // Talk Panel에 가져온 Talk 대사를 뿌려주기
        }

        isAction = true;
        talkIndex++;
    }

    public void ControlMenuPanel() {

        if(!menuPanel.activeSelf) {
            isMenuPanelOn = true;
            menuPanel.SetActive(true);
            gaugeUI.SetActive(false);

        } else {
            isMenuPanelOn = false;
            menuPanel.SetActive(false);
        }
    }

    private void ControlLevel() {

        if(curExp >= maxExp) {
            curExp -= maxExp;
            level++;
        }
    }
    
    private void ControlConditionUI() {

        if(isHouse) { // 집에 들어가 있으면
            gaugeUI.SetActive(false); // 플레이어 머리 위의 체력 UI를 꺼주기
            frozenEffect.SetActive(false); // 추위 디버프 꺼주기
            expSlider.SetActive(false);
            monsterPanel.SetActive(false);
            WeatherManager.instance.SnowOff(); // 눈 내리는 것을 꺼주는 메소드 호출

        } else { // 얼음 필드로 나와있는 상태이면
            if(!isMenuPanelOn) {
                gaugeUI.SetActive(true); // 플레이어 머리 위의 체력 UI를 켜주기   
            }

            if(!isAction) {
                expSlider.SetActive(true);
            }
            
            frozenEffect.SetActive(true); // 추위 디버프 켜주기
            WeatherManager.instance.SnowOn(); // 눈 내리는 것을 켜주는 메소드 호출

            if(isMonsterPanelOn) {
                monsterPanel.SetActive(true);
            }
            
            if(curHealth > 0) {
                curHealth -= 1 * Time.deltaTime;
                
            } else {
                Player.instance.PlayerDead();
            }
        }
    }

    public void SpeedEffectOn() {
        CancelInvoke("SpeedEffectOff"); // 기존에 이동속도 버프 꺼주는 메소드가 실행될 예정이었을 수 있으니 취소해주고 시작
        
        speedEffect.SetActive(true); // 이동속도 버프 켜주기
        Invoke("SpeedEffectOff", 5);
    }

    public void SpeedEffectOff() {
        speedEffect.SetActive(false); // 이동속도 버프 꺼주기
        curMoveSpeed = originMoveSpeed; // 현재 이동속도를 기본 이동속도로 초기화
    }

    public void GameStart() {
        isLive = true;
        
        ItemManager.instance.GenerateItem(); // 아이템 생성 메소드 호출
        NPC.instance.Think(); // NPC 생각 메소드 호출
        
        curHealth = maxHealth;
        Player.instance.anim.SetTrigger("start");
        
        deadPanel.SetActive(false);
        buffPanel.SetActive(true);
        weatherUI.SetActive(true);
        questPanel.SetActive(true);  // 퀘스트 패널 켜주기
        keyBoardButton.SetActive(true); // 키보드 버튼 켜주기
        helpButton.SetActive(true);
        startPanel.SetActive(false); // 시작 메뉴 패널 꺼주기
    }

    public void GameStop() {
        isLive = false;
    }

    public void GameSave() {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuPanel.SetActive(false);
        saveMessage.SetActive(true);
        Invoke("OffSaveMessage", 2f);
    }
    
    private void OffSaveMessage() {
        saveMessage.SetActive(false);
    }

    public void GameLoad() {

        if(!PlayerPrefs.HasKey("PlayerX")) {
            return;
        }
        
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
    }
    
    public void GameExit() { // 게임을 종료하는 메소드
        Application.Quit();
    }

    public void GoToMainMenu() { // 메인메뉴로 나가는 메소드

        isLive = false;
        
        helpButton.SetActive(false);
        keyBoardButton.SetActive(false);
        gaugeUI.SetActive(false);
        expSlider.SetActive(false);
        buffPanel.SetActive(false);
        helpPanel.SetActive(false);
        menuPanel.SetActive(false);
        monsterPanel.SetActive(false);
        questPanel.SetActive(false);
        virtualPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void NewGame() { // New Game 버튼을 클릭했을때 실행하는 메소드

        isNewGame = true;
        isLive = true;
        Player.instance.isDead = false;
        isHouse = false; // 기본적으로 밖에서 시작하니까

        if(ItemManager.instance.fieldItemParent.transform.childCount > 0) { // 이미 만들어진 필드 아이템이 있다면
            for(int i = 0; i < ItemManager.instance.fieldItemParent.transform.childCount; i++) {
                Transform targetObject = ItemManager.instance.fieldItemParent.transform.GetChild(i);
                Destroy(targetObject.gameObject);
            }
            ItemManager.instance.GenerateItem(); // 아이템 생성 메소드 호출
            
        } else { // 기존에 만들어진 필드 아이템이 없다면
            ItemManager.instance.GenerateItem(); // 아이템 생성 메소드 호출
        }
        
        NPC.instance.CancelInvoke(); // 모든 메소드의 invoke를 중지시킴
        NPC.instance.Think(); // NPC 생각 메소드 호출

        player.transform.position = Vector3.zero;
        questManager.questId = 10;
        questManager.questActionIndex = 0;

        curHealth = maxHealth; // 체력 초기화
        Player.instance.anim.SetTrigger("start"); // 기본적으로 아래를 바라보는 애니메이션으로 변경

        if(Inventory.instance.possessItems.Count > 0) { // 현재 인벤토리에 보유중인 아이템이 있다면
            Inventory.instance.possessItems.Clear(); // 보유한 아이템 전부 삭제하기
            Inventory.instance.onChangeItem.Invoke(); // 인벤토리 다시 그려주기
        }

        deadPanel.SetActive(false);
        buffPanel.SetActive(true);
        weatherUI.SetActive(true);
        questPanel.SetActive(true);  // 퀘스트 패널 켜주기
        keyBoardButton.SetActive(true); // 키보드 버튼 켜주기
        helpButton.SetActive(true);
        startPanel.SetActive(false); // 시작 메뉴 패널 꺼주기
        gaugeUI.SetActive(true);
    }

    public void VirtualPanelOnOff() { // 가상 키보드를 켜거나 끄는 메소드

        if(!virtualPanel.activeSelf) { // 가상 키보드가 꺼져있다면
            virtualPanel.SetActive(true); // 켜주기
            
        } else {
            virtualPanel.SetActive(false);
        }
    }

    public void HelpOnOff() { // 우측 상단의 물음표 버튼을 클릭했을때 실행할 메소드

        if(!helpPanel.activeSelf) { // Help 창이 꺼져 있다면
            helpPanel.SetActive(true); // 켜주기
            
        } else {
            helpPanel.SetActive(false);
        }
    }

}