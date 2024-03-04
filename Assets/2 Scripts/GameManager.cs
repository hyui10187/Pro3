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
    public GameObject upStairPosParent;
    public GameObject downStairPosParent;
    public GameObject upLadderPosParent;
    public GameObject downLadderPosParent;
    public GameObject doorInPosParent;
    public GameObject doorOutPosParent;
    public Transform[] upStairPos;
    public Transform[] downStairPos;
    public Transform[] upLadderPos;
    public Transform[] downLadderPos;
    public Transform[] doorInPos;
    public Transform[] doorOutPos;

    [Header("UI - Panel")]
    public GameObject startPanel; // 게임 시작 패널
    public GameObject filterPanel;
    public GameObject menuPanel;
    public GameObject deadPanel;
    public GameObject weatherPanel; // 날씨
    public GameObject longPressBar;
    public GameObject longPressBarBackground;
    public GameObject longPressBarFillArea;
    
    [Header("UI - UpLeft")]
    public GameObject gaugePanel;
    public GameObject buffPanel;
    public GameObject frozenEffect;
    public GameObject speedEffect;
    public GameObject questPanel;
    public Text currentQuestText; // 현재 진행중인 퀘스트 이름
    public GameObject helpButton;
    public GameObject fpsButton;
    public GameObject fpsPanel;

    [Header("UI - UpMiddle")]
    public GameObject goldPanel;
    public GameObject timePanel; // 시계 패널

    [Header("UI - UpRight")]
    public GameObject minimapPanel;
    public GameObject menuButton;
    public GameObject inventoryButton;
    public GameObject virtualButton;
    public GameObject equipmentButton;

    [Header("UI - MiddleLeft")]
    public GameObject storagePanel;
    public GameObject groceryStorePanel;
    public GameObject equipmentPanel;
    public GameObject statsPanel;
    public GameObject statsUpButton;
    public Text statsPointText;
    public Text strPointText;
    public Text dexPointText;
    public Text conPointText;
    public Text wisPointText;
    public Text attackPowerText;
    public Text defensivePowerText;
    public Text attackSpeedText;
    public Text moveSpeedText;
    public Text jopText;
    
    [Header("UI - MiddleMiddle")]
    public GameObject helpPanel;
    public GameObject saveMessage;
    public GameObject alertMessage;
    public GameObject healthManaMessage;
    public Text healthManaMessageText;
    public GameObject itemDescriptionPanel;
    public Text itemDescriptionText;
    public Button consumptionButton;
    public Button equipButton;
    public Text equipButtonText;
    public GameObject confirmPanel;

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
    public int itemMoveSpeed;
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
    public int statsPoint;
    public int strPoint;
    public int dexPoint;
    public int conPoint;
    public int wisPoint;
    public int itemAttackPower;   // 장착중인 아이템의 공격력
    public int playerAttackPower; // 플레이어의 기본 공격력
    public int defensivePower;
    public int attackSpeed;

    [Header("Flag")]
    public bool isLive; // 게임이 진행중인지 체크하는 플래그
    public bool isAction; // 대화를 하는중인지 체크하기 위한 플래그값
    public bool isHouse; // 집에 들어갔는지 체크하기 위한 플래그값
    public bool hasQuestItem;
    public bool canEatQuestItem = true; // 퀘스트 아이템을 먹을 수 있는지 플래그값

    [Header("Etc")]
    public GameObject globalLight;
    public int talkIndex;
    public float frozenTime;
    public float frozenCoolTime;
    public int frozenDamage;
    public int obstacleDamage;
    public GameObject scanObject; // 스캔한 게임 오브젝트
    
    private void Awake() {
        instance = this;
        InitPos();
        Invoke("StartPanelOn", 0.5f);
    }

    private void StartPanelOn() {
        startPanel.SetActive(true);
        StopCoroutine("FilterPanelFadeIn");
        StartCoroutine("FilterPanelFadeIn");
    }

    private void InitPos() {
        upStairPos = upStairPosParent.GetComponentsInChildren<Transform>();
        downStairPos = downStairPosParent.GetComponentsInChildren<Transform>();
        upLadderPos = upLadderPosParent.GetComponentsInChildren<Transform>();
        downLadderPos = downLadderPosParent.GetComponentsInChildren<Transform>();
        doorInPos = doorInPosParent.GetComponentsInChildren<Transform>();
        doorOutPos = doorOutPosParent.GetComponentsInChildren<Transform>();
    }

    private void Update() {

        if(Player.instance.isDead || !isLive) {
            return;
        }

        ControlLevel();
        ControlConditionUI();
        curGameTime += Time.deltaTime;

        if(SpawnManager.instance.enemyCount == 0 && questManager.questId == 50) {
            questManager.CheckQuest(0);
        }
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
            
            if(Inventory.instance.possessItems.Count < Inventory.instance.CurSlotCnt) { // 인벤토리에 슬롯 여분이 있을 경우
                hasQuestItem = true;
                canEatQuestItem = true;
            
            } else { // 인벤토리가 꽉 차 있을 경우
                AlertManager.instance.AlertMessageOn("", 5);
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

        if(objData == null) {
            return;
        }
        
        Talk(objData.objId, objData.isNpc, scanObject.name);
        talkPanel.SetActive(isAction); // 대화창을 끄고 켜는 것은 isAction 플래그 값이랑 동일하다
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
                scanObject.GetComponent<BoxCollider2D>().isTrigger = true; // 퀘스트 아이템인 은화를 Trigger로 만들어주기
                scanObject.SetActive(false); // 퀘스트 아이템인 은화를 꺼주기
                scanObject.transform.position = player.transform.position; // 퀘스트 아이템인 은화를 플레이어의 위치로 옮겨줌
                scanObject.SetActive(true); // 퀘스트 아이템인 은화를 다시 켜줘서 바로 먹어지도록 하기
            }
            
            if(scanObject.CompareTag("Stone")) {
                ItemManager.instance.DropMaterial(player.transform.position, 1); // 돌멩이 재료 아이템과 대화가 끝나면 해당 아이템을 플레이어의 위치에 생성
                scanObject.SetActive(false); // 기존에 대화한 돌멩이 오브젝트는 꺼주기
            }

            if(isNpc && objId == 30000) { // 이동형 NPC 베르톨트
                NPC npc = scanObject.GetComponent<NPC>();
                npc.isCollision = false;
                npc.CancelInvoke();
                npc.Think();
            } else if(isNpc && objId == 130000) { // 이동형 NPC 생쥐
                Animal animal = scanObject.GetComponent<Animal>();
                animal.isCollision = false;
                animal.CancelInvoke();
                animal.Think();
            } else if(isNpc && objId == 160000) { // 기능형 NPC 카리나
                PanelManager.instance.StorageOnOff(); // 창고 패널 켜주기
                inventoryPanel.SetActive(true); // 인벤토리 패널도 같이 켜주기
            } else if(isNpc && objId == 170000) { // 기능형 NPC 린샹이면
                PanelManager.instance.StoreOnOff(); // 상점 패널 켜주기
                inventoryPanel.SetActive(true); // 인벤토리 패널도 같이 켜주기
            }

            if(objId == 6200) { // 열쇠로 문을 여는 대사가 끝나면
                scanObject.gameObject.SetActive(false);
                Inventory.instance.isDoorOpen = true;

            } else if(objId == 6300) { // 열쇠로 상자를 여는 대사가 끝나면
                scanObject.gameObject.SetActive(false);
                Transform parentTransform = scanObject.transform.parent;
                Transform[] childTransforms = parentTransform.GetComponentsInChildren<Transform>(true);

                foreach(Transform childTransform in childTransforms) {
                    if(childTransform.gameObject.name == "Candy") {
                        childTransform.gameObject.SetActive(true);
                        Inventory.instance.isChestOpen = true;
                    }
                }
            } else if(objId == 6400) { // 열려있는 상자에서 사탕을 발견하는 대사가 끝나면
                scanObject.gameObject.SetActive(false);
                Transform parentTransform = scanObject.transform.parent;
                Transform[] childTransforms = parentTransform.GetComponentsInChildren<Transform>(true);
                questManager.candy.SetActive(true);

                foreach(Transform childTransform in childTransforms) {
                    if(childTransform.gameObject.name == "Opened") {
                        childTransform.gameObject.SetActive(true);
                        questManager.candy.SetActive(true);
                    }
                }
            }

            expSlider.SetActive(true);
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

    private void ControlLevel() { // 레벨업을 담당하는 메소드

        if(maxExp <= curExp) {
            curExp -= maxExp;
            curLevel++;
            maxHealth += 10; // 레벨업하면 플레이어의 최대 체력 늘려주기
            maxMana += 5;    // 레벨업하면 플레이어의 최대 마나 늘려주기
            curHealth = maxHealth;
            curMana = maxMana;
            statsPoint++;

            PanelManager.instance.RedrawStatsPanel();
            PanelManager.instance.StatsOnOff(); // 레벨업 하면 자동으로 스탯창 켜주기
            PanelManager.instance.StatsUpButtonOn(); // 스탯 포인트를 올릴 수 있는 버튼도 켜주기
            AlertManager.instance.AlertMessageOn("", 12);
        }
    }
    
    private void ControlConditionUI() {
        
        if(isHouse) { // 집에 들어가 있으면
            frozenEffect.SetActive(false); // 추위 디버프 꺼주기
            WeatherManager.instance.SnowOff(); // 눈 내리는 것을 꺼주는 메소드 호출

        } else { // 얼음 필드로 나와있는 상태이면
            frozenEffect.SetActive(true); // 추위 디버프 켜주기
            WeatherManager.instance.SnowOn(); // 눈 내리는 것을 켜주는 메소드 호출
            
            frozenCoolTime += Time.deltaTime;

            if(curHealth > 0 && frozenCoolTime > frozenTime) {
                frozenCoolTime = 0;
                Player.instance.OnDamaged(Vector2.zero, frozenDamage);
            }
        }

        if(curHealth <= 0) {
            Player.instance.PlayerDead();
        }
    }

    public void SaveButtonClick() {
        SaveData();
        menuPanel.SetActive(false);
        AlertManager.instance.SaveMessageOn();
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

    public void LoadGameButtonClick() {

        if(!PlayerPrefs.HasKey("PlayerX")) { // 한번도 저장한 적이 없으면
            NewGameButtonClick(); // 새로운 게임을 시작
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
        isLive = true;
        
        NPC.instance.CancelInvoke(); // 모든 메소드의 invoke를 중지시킴
        NPC.instance.Think(); // NPC 생각 메소드 호출
        Animal.instance.CancelInvoke();
        Animal.instance.Think();
        
        startPanel.SetActive(false);
        
        filterPanel.SetActive(true);
        StopCoroutine("FilterPanelFade");
        StartCoroutine("FilterPanelFade");
        Invoke("GlobalLightOn", 0.1f);
        
        PanelManager.instance.RedrawStatsPanel();
    }
    
    public void GameExit() { // 게임을 종료하는 메소드
        Application.Quit();
    }

    public void GoToMainMenuButtonClick() { // 메인메뉴로 나가는 메소드
        isLive = false;
        PanelManager.instance.PanelOff(); // 모든 패널 꺼주기
        StopCoroutine("FilterPanelFadeOut");
        StartCoroutine("FilterPanelFadeOut");
    }

    public void NewGameButtonClick() { // New Game 버튼을 클릭했을때 실행하는 메소드

        startPanel.SetActive(false);
        
        isLive = true;
        Player.instance.isDead = false;
        isHouse = false; // 기본적으로 밖에서 시작하니까
        curGold = startGold;
        curMoveSpeed = originMoveSpeed;
        
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
        Animal.instance.CancelInvoke();
        Animal.instance.Think();

        player.transform.position = Vector3.zero;
        questManager.questId = 10;
        questManager.questActionIndex = 0;
        currentQuestText.text = questManager.CheckQuest();

        curHealth = maxHealth; // 체력 초기화
        Player.instance.anim.SetTrigger("start"); // 기본적으로 아래를 바라보는 애니메이션으로 변경

        if(Inventory.instance.possessItems.Count > 0) { // 현재 인벤토리에 보유중인 아이템이 있다면
            Inventory.instance.possessItems.Clear(); // 보유한 아이템 전부 삭제하기
            Inventory.instance.onChangeItem.Invoke(); // 인벤토리 다시 그려주기
        }

        PanelManager.instance.RedrawStatsPanel();
        
        filterPanel.SetActive(true);
        StopCoroutine("FilterPanelFadeIn");
        StartCoroutine("FilterPanelFadeIn");
        Invoke("GlobalLightOn", 0.1f);
    }

    private IEnumerator FilterPanelFadeIn() { // 검정색 필터를 서서히 투명하게 만들어줘서 Fade In 효과를 주는 코루틴
        
        Image filterPanelImage = filterPanel.GetComponent<Image>();
        float fadeTime = 1;

        while(0 < fadeTime) {
            fadeTime -= 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01 초마다 실행
            filterPanelImage.color = new Color(0, 0, 0, fadeTime);
        }
        
        filterPanel.SetActive(false);
    }
    
    private IEnumerator FilterPanelFadeOut() { // 검정색 필터를 서서히 투명하게 만들어줘서 Fade Out 효과를 주는 코루틴
        
        filterPanel.SetActive(true);
        
        Image filterPanelImage = filterPanel.GetComponent<Image>();
        float fadeTime = 0;

        while(fadeTime < 1) {
            fadeTime += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01 초마다 실행
            filterPanelImage.color = new Color(0, 0, 0, fadeTime);
        }
        
        filterPanel.SetActive(false);
        startPanel.SetActive(true);
    }
    
    private void GlobalLightOn() {
        globalLight.SetActive(true);
        PanelManager.instance.PanelOff();
        PanelManager.instance.PanelOn();
    }

    public bool IsNPCPanelOn() {
        if(storagePanel.activeSelf || groceryStorePanel.activeSelf) {
            return true;
        } else {
            return false;
        }
    }

}