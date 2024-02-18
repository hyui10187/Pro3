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
    public Transform[] downStairPos;
    public Transform[] upLadderPos;
    public Transform[] downLadderPos;

    [Header("UI - Panel")]
    public GameObject startPanel; // 게임 시작 패널
    public GameObject menuPanel;
    public GameObject deadPanel;
    public GameObject weatherPanel; // 날씨
    
    [Header("UI - UpLeft")]
    public GameObject gaugePanel;
    public GameObject questPanel;
    public Text currentQuestText; // 현재 진행중인 퀘스트 이름
    public GameObject keyBoardButton;

    [Header("UI - UpMiddle")]
    public GameObject timePanel; // 시계 패널
    public GameObject buffPanel;
    public GameObject frozenEffect;
    public GameObject speedEffect;

    [Header("UI - UpRight")]
    public GameObject goldPanel;
    public GameObject helpButton;
    public GameObject menuButton;
    public GameObject inventoryButton;
    
    [Header("UI - MiddleLeft")]
    public GameObject storagePanel;
    
    [Header("UI - MiddleMiddle")]
    public GameObject helpPanel;
    public GameObject saveMessage;
    public GameObject fullMessage;
    public GameObject acquisitionMessage;
    public GameObject cantAttackMessage;

    [Header("UI - MiddleRight")]
    public GameObject inventoryPanel;
    public GameObject monsterPanel;
    
    [Header("UI - BottomLeft")]
    public GameObject virtualJoystick;
    
    [Header("UI - BottomBottom")]
    public GameObject talkPanel; // 대화창
    public Text talkText; // 대화창의 대화 내용
    public GameObject expSlider;
    
    [Header("UI - BottomRight")]
    public GameObject attackButton;
    public GameObject actionButton;
    
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
    public int curGold;
    public int startGold;
    public int curLevel;

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
    public float frozenTime;
    public float frozenCoolTime;
    public GameObject scanObject; // 스캔한 게임 오브젝트
    
    private void Awake() {
        instance = this;
    }

    private void Start() {

        if(!isNewGame) {
            GameLoad();
        }
        
        curHealth = maxHealth; // 게임 처음 시작시 현재 체력(health)을 최대 체력(maxHealth)으로 초기화
        curGold = startGold;
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
        } else if(scanObject.CompareTag("QuestItem") && !hasQuestItem) { // 퀘스트 아이템인 Coin한테 말을 걸었을 경우
            
            //scanObject.GetComponent<BoxCollider2D>().isTrigger = true; // 퀘스트 아이템인 Coin을 Trigger로 만들어줘서 먹을 수 있게 해주기

            //else if(scanObject.CompareTag("QuestItem") && !hasQuestItem) { // 퀘스트 아이템인 Coin을 먹었을 경우
            if(Inventory.instance.possessItems.Count < Inventory.instance.CurSlotCnt) { // 인벤토리에 슬롯 여분이 있을 경우
                //FieldItems fieldItems = scanObject.GetComponent<FieldItems>();
                //Inventory.instance.AddItem(fieldItems.GetItem());
                // scanObject.GetComponent<BoxCollider2D>().isTrigger = true; // 퀘스트 아이템인 Coin을 
                // scanObject.transform.position = player.transform.position; // 반지 아이템을 플레이어의 위치로 옮겨줌
            
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
        } else if(scanObject.layer == 11) {
            Animal animal = scanObject.GetComponent<Animal>();
            animal.isCollision = true;
            animal.CancelInvoke();
        }

        Talk(objData.objId, objData.isNpc, scanObject.name);
        talkPanel.SetActive(isAction); // 대화창을 끄고 켜는 것은 isAction 플래그 값이랑 동일하다
    }

    public void ControlInventory() {
        inventoryManager.OnOffInventory();
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

            if(hasQuestItem && QuestManager.instance.questItem[0] != null) {
                scanObject.GetComponent<BoxCollider2D>().isTrigger = true; // 퀘스트 아이템인 Coin을 Trigger로 만들어주기
                scanObject.SetActive(false); // 퀘스트 아이템인 Coin을 꺼주기
                scanObject.transform.position = player.transform.position; // 퀘스트 아이템인 Coin을 플레이어의 위치로 옮겨줌
                scanObject.SetActive(true); // 퀘스트 아이템인 Coin을 다시 켜줘서 바로 먹어지도록 하기
            }
            
            if(scanObject.CompareTag("Stone")) {
                ItemManager.instance.DropMaterial(player.transform.position, 1); // 돌맹이 재료 아이템과 대화가 끝나면 해당 아이템을 플레이어의 위치에 생성
                scanObject.SetActive(false); // 그리고 기존에 대화한 돌맹이 오브젝트는 꺼주기
            }

            if(isNpc && objId == 30000) {
                NPC npc = scanObject.GetComponent<NPC>();
                npc.isCollision = false;
                npc.CancelInvoke();
                npc.Think();
            } else if(isNpc && objId == 130000) {
                Animal animal = scanObject.GetComponent<Animal>();
                animal.isCollision = false;
                animal.CancelInvoke();
                animal.Think();
            } else if(isNpc && objId == 160000) {
                StorageManager.instance.OnOffStoragePanel();
            }

            return;
        }
        
        if(isNpc) { // 대화를 한게 NPC일 경우에는 NPC의 이름도 같이 보여주기
            talkText.text = npcName + " : " + talkData;
        } else {
            talkText.text = talkData; // Talk Panel에 가져온 Talk 대사를 뿌려주기
        }

        isAction = true;
        talkIndex++;
    }

    public void ControlMenuPanel() {

        if(!menuPanel.activeSelf) {
            isMenuPanelOn = true;
            menuPanel.SetActive(true);

        } else {
            isMenuPanelOn = false;
            menuPanel.SetActive(false);
        }
    }

    private void ControlLevel() {

        if(curExp >= maxExp) {
            curExp -= maxExp;
            curLevel++;
        }
    }
    
    private void ControlConditionUI() {

        if(!isAction) {
            expSlider.SetActive(true);
        }
        
        if(isHouse) { // 집에 들어가 있으면
            frozenEffect.SetActive(false); // 추위 디버프 꺼주기
            WeatherManager.instance.SnowOff(); // 눈 내리는 것을 꺼주는 메소드 호출

        } else { // 얼음 필드로 나와있는 상태이면
            frozenEffect.SetActive(true); // 추위 디버프 켜주기
            WeatherManager.instance.SnowOn(); // 눈 내리는 것을 켜주는 메소드 호출

            if(isMonsterPanelOn) {
                monsterPanel.SetActive(true);
            }
            
            frozenCoolTime += Time.deltaTime;

            if(curHealth > 0 && frozenCoolTime > frozenTime) {
                frozenCoolTime = 0;
                curHealth -= 10;
            }
            
            if(curHealth <= 0) {
                curHealth = 0;
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

        PanelOff();
        PanelOn();
    }

    public void GameStop() {
        isLive = false;
    }

    public void GameSave() {
        SaveData();
        menuPanel.SetActive(false);
        saveMessage.SetActive(true);
        Invoke("OffSaveMessage", 2f);
    }

    private void SaveData() {
        int tempIsHouse = isHouse ? 1 : 0;
        
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.SetInt("isHouse", tempIsHouse);
        PlayerPrefs.SetInt("Level", curLevel);
        PlayerPrefs.SetInt("Gold", curGold);
        PlayerPrefs.SetFloat("Health", curHealth);
        PlayerPrefs.SetFloat("Mana", curMana);
        PlayerPrefs.Save();
    }
    
    private void OffSaveMessage() {
        saveMessage.SetActive(false);
    }

    public void GameLoad() {

        if(!PlayerPrefs.HasKey("PlayerX")) { // 한번도 저장한 적이 없으면
            NewGame(); // 새로운 게임을 시작
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
        PanelOff(); // 모든 패널 꺼주기
        startPanel.SetActive(true);
    }

    public void NewGame() { // New Game 버튼을 클릭했을때 실행하는 메소드

        isNewGame = true;
        isLive = true;
        Player.instance.isDead = false;
        isHouse = false; // 기본적으로 밖에서 시작하니까
        curGold = startGold;
        
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
        Animal.instance.CancelInvoke(); // 모든 메소드의 invoke를 중지시킴
        Animal.instance.Think(); // NPC 생각 메소드 호출

        player.transform.position = Vector3.zero;
        questManager.questId = 10;
        questManager.questActionIndex = 0;

        curHealth = maxHealth; // 체력 초기화
        Player.instance.anim.SetTrigger("start"); // 기본적으로 아래를 바라보는 애니메이션으로 변경

        if(Inventory.instance.possessItems.Count > 0) { // 현재 인벤토리에 보유중인 아이템이 있다면
            Inventory.instance.possessItems.Clear(); // 보유한 아이템 전부 삭제하기
            Inventory.instance.onChangeItem.Invoke(); // 인벤토리 다시 그려주기
        }

        PanelOff();
        PanelOn();
    }

    public void VirtualPanelOnOff() { // 가상 키보드를 켜거나 꺼주는 메소드

        if(!virtualJoystick.activeSelf) { // 가상 조이스틱이 꺼져있다면
            virtualJoystick.SetActive(true); // 가상 조이스틱 켜주기
            attackButton.SetActive(true); // 가상 키 켜주기
            actionButton.SetActive(true); // 가상 키 켜주기
        } else {
            virtualJoystick.SetActive(false);
            attackButton.SetActive(false);
            actionButton.SetActive(false);
        }
    }

    public void HelpOnOff() { // 우측 상단의 물음표 버튼을 클릭했을때 실행할 메소드

        if(!helpPanel.activeSelf) { // Help 창이 꺼져 있다면
            helpPanel.SetActive(true); // 켜주기
        } else {
            helpPanel.SetActive(false);
        }
    }

    private void PanelOn() {

        // UI - UpLeft
        gaugePanel.SetActive(true);
        questPanel.SetActive(true);
        keyBoardButton.SetActive(true);
        
        // UI - UpMiddle
        buffPanel.SetActive(true);

        // UI - UpRight
        goldPanel.SetActive(true);
        helpButton.SetActive(true);
        menuButton.SetActive(true);
        inventoryButton.SetActive(true);
        
        // UI - MiddleMiddle

        // UI - MiddleRight

        // UI - BottomLeft

        // UI - BottomBottom
        expSlider.SetActive(true);
        
        // UI - BottomRight

    }
    
    private void PanelOff() {
        
        // UI - Panel
        startPanel.SetActive(false);
        menuPanel.SetActive(false);
        deadPanel.SetActive(false);
        weatherPanel.SetActive(false);
        
        // UI - UpLeft
        gaugePanel.SetActive(false);
        frozenEffect.SetActive(false);
        speedEffect.SetActive(false);
        questPanel.SetActive(false);
        keyBoardButton.SetActive(false);
        
        // UI - UpMiddle
        timePanel.SetActive(false);
        buffPanel.SetActive(false);
        
        // UI - UpRight
        goldPanel.SetActive(false);
        helpButton.SetActive(false);
        menuButton.SetActive(false);
        inventoryButton.SetActive(false);
        
        // UI - MiddleMiddle
        helpPanel.SetActive(false);
        saveMessage.SetActive(false);
        fullMessage.SetActive(false);
        acquisitionMessage.SetActive(false);
        cantAttackMessage.SetActive(false);
        
        // UI - MiddleRight
        inventoryPanel.SetActive(false);
        monsterPanel.SetActive(false);
        
        // UI - BottomLeft
        virtualJoystick.SetActive(false);
        
        // UI - BottomBottom
        talkPanel.SetActive(false);
        expSlider.SetActive(false);
        
        // UI - BottomRight
        attackButton.SetActive(false);
        actionButton.SetActive(false);
    }

}