using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MarksAssets.VibrationWebGL;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    public GameObject upHolePosParent;
    public GameObject downHolePosParent;
    public GameObject doorInPosParent;
    public GameObject doorOutPosParent;
    public Transform[] upStairPos;
    public Transform[] downStairPos;
    public Transform[] upLadderPos;
    public Transform[] downLadderPos;
    public Transform[] upHolePos;
    public Transform[] downHolePos;
    public Transform[] doorInPos;
    public Transform[] doorOutPos;

    [Header("UI - Panel")]
    public GameObject npcNamePanel;
    public Text npcNamePanelText;
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
    public GameObject questPreviewPanel;
    public Text currentQuestText; // 현재 진행중인 퀘스트 이름
    public GameObject helpButton;
    public GameObject fpsButton;
    public GameObject fpsPanel;
    public GameObject chatButton;

    [Header("UI - UpMiddle")]
    public GameObject goldPanel;
    public GameObject timePanel; // 시계 패널

    [Header("UI - UpRight")]
    public GameObject minimapPanel;
    public GameObject menuButton;
    public GameObject inventoryButton;
    public GameObject virtualButton;
    public GameObject equipmentButton;
    public GameObject acquisitionMessage;

    [Header("UI - MiddleLeft")]
    public GameObject storagePanel;
    public GameObject equipmentPanel;
    public GameObject statsPanel;
    public GameObject statsUpButton;
    public GameObject questPanel;
    public Text questTitle;
    public Text questText;
    public GameObject groceryStorePanel;
    public GameObject equipmentStorePanel;
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
    public Text helpPanelText;
    public Button leftPageButton;
    public Button rightPageButton;
    public GameObject levelUpMessage;
    public GameObject saveMessage;
    public GameObject smallAlertMessage;
    public GameObject bigAlertMessage;
    public GameObject longAlertMessage;
    public GameObject healthManaMessage;
    public Text healthManaMessageText;
    public GameObject itemDescriptionPanel;
    public Text itemDescriptionText;
    public Button consumptionButton;
    public Button equipButton;
    public Button deleteButton;
    public Text equipButtonText;
    public GameObject goToMainConfirmPanel;
    public GameObject sleepConfirmPanel;
    public GameObject entrustAmountPanel;
    public Text entrustText;
    public Text entrustAmountText;
    public GameObject withdrawAmountPanel;
    public Text withdrawText;
    public Text withdrawAmountText;
    public GameObject purchaseAmountPanel;
    public Text purchaseText;
    public Text purchaseAmountText;
    public GameObject sellAmountPanel;
    public Text sellText;
    public Text sellAmountText;
    public GameObject turnTablePanel;
    public GameObject sellConfirmPanel;
    public GameObject purchaseConfirmPanel;

    [Header("UI - MiddleRight")]
    public GameObject inventoryPanel;
    public GameObject monsterPanel;
    
    [Header("UI - BottomLeft")]
    public GameObject virtualJoystick;

    [Header("UI - BottomBottom")]
    public GameObject talkInformMessage;
    public GameObject chatPanel;
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
    public GameObject levelUpEffect;

    [Header("Flag")]
    public bool isLive; // 게임이 진행중인지 체크하는 플래그
    public bool isAction; // 대화를 하는중인지 체크하기 위한 플래그값
    public bool isHouse; // 집에 들어갔는지 체크하기 위한 플래그값
    public bool isHealing;
    public bool canEatQuestItem = true; // 퀘스트 아이템을 먹을 수 있는지 플래그값
    
    [Header("Tile")]
    public GameObject[] iceInfinite;
    public GameObject[] treeInfinite;

    [Header("Etc")]
    public GameObject globalLight;
    public int talkIndex;
    public float frozenTime;
    public float frozenCoolTime;
    public int frozenDamage;
    public int obstacleDamage;
    public SpriteAtlas spriteAtlas;
    public GameObject scanObject; // 스캔한 게임 오브젝트
    //public AdManager adManager; // 구글 애드몹

    public int objId;
    public bool isNpc;
    public string npcName;

    private void Awake() 
    {
        instance = this;
        InitPos();
        Invoke("StartPanelOn", 0.5f);

        BindEvent();
    }

    private void BindEvent()
    {
        Player.instance.OnSleepHandler += PlayerSleep;
    }
    
    private void PlayerSleep()
    {
        int sleepHour = Mathf.FloorToInt(curGameTime / 3600) % 24;  // 플레이어가 잠에 든 시간
        int sleepMin = Mathf.FloorToInt((curGameTime % 3600) / 60); // 플레이어가 잠에 든 분

        int wakeUpHour = 7 + 24; // 다음날 오전 7시에 일어나도록
        int wakeUpMin = 0;

        float timeToNextMorning = ((wakeUpHour - sleepHour) * 3600) + ((wakeUpMin - sleepMin) * 60);
        
        curGameTime += timeToNextMorning;
        curHealth = maxHealth; // 체력을 전부 채워주기
        curMana = maxMana;     // 마나를 전부 채워주기
        
        PanelManager.instance.SleepConfirmOff();
        StopCoroutine(FilterPanelFadeOutAndIn());
        StartCoroutine(FilterPanelFadeOutAndIn());
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
        downHolePos = downHolePosParent.GetComponentsInChildren<Transform>();
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

        if(SpawnManager.instance.enemyCount == 0 && questManager.questId == 50) { // 몬스터 처지 이벤트 완료되면
            questManager.CheckQuest(0);
        }
    }

    public void Action(GameObject scanObj) {
        
        if(Player.instance.isDead || isHealing || turnTablePanel.activeSelf) {
            return;
        }

        if(!isAction) {
            SoundManager.instance.PlaySound(AudioClipName.Talk);
        }
        
        expSlider.SetActive(false);
        scanObject = scanObj;
        
        if(scanObject.CompareTag("Clock")) { // 괘종시계에 말을 걸었으면
            timePanel.SetActive(true); // 시계 패널을 켜주기
            SoundManager.instance.PlaySound(AudioClipName.Clock, true);
        }
        
        ObjData objData = scanObject.GetComponent<ObjData>();
        
        if(scanObject.CompareTag("QuestItem")) { // 퀘스트 아이템인 Coin한테 말을 걸었을 경우
            
            if(Inventory.instance.possessItems.Count < Inventory.instance.CurSlotCnt) { // 인벤토리에 슬롯 여분이 있을 경우
                canEatQuestItem = true;
                
            } else { // 인벤토리가 꽉 차 있을 경우
                AlertManager.instance.SmallAlertMessageOn(ItemName.공백, 5);
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

        this.objId = objId;
        this.isNpc = isNpc;
        this.npcName = npcName;
        
        int questIdPlusQuestActionIndex = questManager.QuestIdPlusQuestActionIndex();
        string talkData = talkManager.GetTalk(objId, questIdPlusQuestActionIndex, isNpc, talkIndex); // 대상의 ID와 QuestTalkIndex를 더한 값을 첫번째 파라미터로 던져준다
        
        if(talkData == null) { // 더이상 다음 대화가 없다면
            EndTalk();
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

    public void SkipTalk() {
        EndTalk();
        talkPanel.SetActive(false);
    }

    private void EndTalk() {
        isAction = false; // isAction을 false로 줘서 대화창 끄기

        if(timePanel.activeSelf)
        {
            timePanel.SetActive(false); // 대화가 끝났을때는 시계 패널은 항상 꺼주는 것으로 처리
            SoundManager.instance.StopSound();   
        }
        talkIndex = 0; // 대화가 끝나면 talkIndex 초기화
        currentQuestText.text = questManager.CheckQuest(objId); // 다음에 진행할 퀘스트명을 UI에 뿌려주기
        
        if(scanObject.CompareTag("Heal")) {
            FadeOutAndInEffect();
            SoundManager.instance.PlaySound(AudioClipName.Heal);
            curHealth = maxHealth;
            return;
        }
        
        if(scanObject.CompareTag("Bed")) {
            isAction = true;
            PanelManager.instance.SleepConfirmOn();
            return;
        }

        if(scanObject.CompareTag("QuestItem") && QuestManager.instance.coin != null) {
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
        } else if(isNpc && objId == 160000) { // 기능형 NPC 카리나(창고)
            PanelManager.instance.StorageOnOff();
        } else if(isNpc && objId == 170000) { // 기능형 NPC 린샹(잡화상점)
            PanelManager.instance.GroceryStoreOnOff();
        } else if(isNpc && objId == 210000) { // 기능형 NPC 밀로(장비상점)
            PanelManager.instance.EquipmentStoreOnOff();
        }

        if(objId == 5100) { // 턴테이블에 말하는 대사가 끝나면
            PanelManager.instance.TurnTablePanelOnOff();
        }

        if(objId == 6200) { // 열쇠로 문을 여는 대사가 끝나면
            scanObject.gameObject.SetActive(false);
            Inventory.instance.isDoorOpen = true;
            SoundManager.instance.PlaySound(AudioClipName.Key);
            VibrationWebGL.Vibrate(100); // 문이 열리면 진동 피드백을 주기

        } else if(objId == 6300) { // 열쇠로 상자를 여는 대사가 끝나면
            scanObject.gameObject.SetActive(false);
            Transform parentTransform = scanObject.transform.parent;
            Transform[] childTransforms = parentTransform.GetComponentsInChildren<Transform>(true);
            SoundManager.instance.PlaySound(AudioClipName.Key);
            VibrationWebGL.Vibrate(100); // 상자가 열리면 진동 피드백을 주기

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
            questManager.CheckQuest(0);

            foreach(Transform childTransform in childTransforms) {
                if(childTransform.gameObject.name == "Opened") {
                    childTransform.gameObject.SetActive(true);
                    questManager.candy.SetActive(true);
                }
            }
        }

        expSlider.SetActive(true);
    }

    private void FadeOutAndInEffect() {
        StopCoroutine("FilterPanelFadeOutAndIn");
        StartCoroutine("FilterPanelFadeOutAndIn");
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
            SoundManager.instance.PlaySound(AudioClipName.LevelUp);
            
            PanelManager.instance.RedrawStatsPanel();
            PanelManager.instance.StatsOnOff(); // 레벨업 하면 자동으로 스탯창 켜주기
            PanelManager.instance.StatsUpButtonOn(); // 스탯 포인트를 올릴 수 있는 버튼도 켜주기
            LevelUpEffectOn();
            AlertManager.instance.LevelUPMessageOn();
        }
    }

    private void LevelUpEffectOn() {
        levelUpEffect.SetActive(true);
        Invoke("LevelUpEffectOff", 2);
    }

    private void LevelUpEffectOff() {
        levelUpEffect.SetActive(false);
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
                Player.instance.PlayerDamaged(Vector2.zero, frozenDamage);
            }
        }

        if(curHealth <= 0) {
            curHealth = 0;
            Player.instance.PlayerDead();
        }
    }

    public void SaveButtonClick() // 저장하기 버튼을 클릭했을때 호출되는 메소드
    {
        SoundManager.instance.PlaySound(AudioClipName.Click);
        SavePlayerData();
        SaveInventoryData();
        menuPanel.SetActive(false);
        AlertManager.instance.SaveMessageOn();
    }

    private void SavePlayerData() // 플레이어의 정보를 저장하는 메소드
    {
        PlayerData playerData = new PlayerData();
        playerData.playerX = player.transform.position.x;
        playerData.playerY = player.transform.position.y;
        playerData.questId = questManager.questId;
        playerData.questActionIndex = questManager.questActionIndex;
        playerData.isHouse = isHouse;
        playerData.level = curLevel;
        playerData.exp = curExp;
        playerData.gold = curGold;
        playerData.hp = curHealth;
        playerData.mp = curMana;
        
        JsonManager.Instance.SavePlayerData(playerData);
    }

    private void SaveInventoryData() // 인벤토리에 보유한 아이템 정보를 저장하는 메소드
    {
        InventorySlot[] inventorySlots = InventoryManager.instance.inventorySlots;
        List<InventoryData> inventoryDataList = new List<InventoryData>();

        for(int i = 0; i < InventoryManager.instance.inventory.possessItems.Count; i++)
        {
            InventoryData inventoryData = new InventoryData();
            inventoryData.index = inventorySlots[i].item.itemIndex;
            inventoryData.itemCount = inventorySlots[i].item.itemCount;
            inventoryData.slotNum = inventorySlots[i].slotNum;
            inventoryData.isEquipped = inventorySlots[i].item.isEquipped;
            
            inventoryDataList.Add(inventoryData);
        }

        JsonManager.Instance.SaveInventoryData(inventoryDataList);
    }

    public void LoadGameButtonClick()
    {
        SoundManager.instance.PlaySound(AudioClipName.Click);
        PlayerData playerData = JsonManager.Instance.LoadPlayerData();
        List<InventoryData> inventoryDataList = JsonManager.Instance.LoadInventoryData();

        if(playerData == null) // 한번도 저장한 적이 없으면
        {
            NewGameButtonClick(); // 새로운 게임을 시작
            return;
        }
        
        SetLoadPlayerData(playerData);
        SetLoadInventoryData(inventoryDataList);
    }

    private void SetLoadPlayerData(PlayerData playerData)
    {
        float x = playerData.playerX;
        float y = playerData.playerY;

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = playerData.questId;
        questManager.questActionIndex = playerData.questActionIndex;
        curHealth = playerData.hp;
        curMana = playerData.mp;
        curLevel = playerData.level;
        curExp = playerData.exp;
        curGold = playerData.gold;
        isHouse = playerData.isHouse;

        InitGame();
    }

    private void SetLoadInventoryData(List<InventoryData> inventoryDataList)
    {
        if(Inventory.instance.possessItems.Count > 0) { // 현재 인벤토리에 보유중인 아이템이 있다면
            Inventory.instance.possessItems.Clear(); // 보유한 아이템 전부 삭제하기
            Inventory.instance.onChangeItem.Invoke(); // 인벤토리 다시 그려주기
        }
        InventorySlot[] inventorySlots = InventoryManager.instance.inventorySlots;

        string folderPath = Path.Combine(Application.dataPath, "10 Data").Replace("\\", "/");
        string filePath = Path.Combine(folderPath, "ItemData.xlsx").Replace("\\", "/"); // 저장해줄 JSON 파일명
        List<Dictionary<string, object>> excelDataList = JsonManager.Instance.LoadExcel(filePath);

        for(int i = 0; i < inventoryDataList.Count; i++) // 저장되었던 아이템의 갯수만큼 루프를 돌아주기
        {
            for(int j = 0; j < excelDataList.Count; j++)
            {
                int index = Convert.ToInt32(excelDataList[j]["index"]); // 아이템의 인덱스 번호
                
                if(inventoryDataList[i].index == index) // 일치하는 아이템 데이터를 찾았으면
                {
                    string spriteName = excelDataList[j]["itemSpriteName"].ToString();

                    if(inventorySlots[i].item == null)
                        inventorySlots[i].item = new Item();

                    inventorySlots[i].item.itemImage = spriteAtlas.GetSprite(spriteName);
                    break;
                }
            }

            inventorySlots[i].item.itemCount = inventoryDataList[i].itemCount;
            inventorySlots[i].slotNum = inventoryDataList[i].slotNum;
            inventorySlots[i].item.isEquipped = inventoryDataList[i].isEquipped;
            
            InventoryManager.instance.inventory.possessItems.Add(inventorySlots[i].item);
            Inventory.instance.onChangeItem.Invoke(); // 인벤토리 다시 그려주기
        }
        
    }
    
    public void GameExit() { // 게임을 종료하는 메소드
        Application.Quit();
    }

    public void GoToMainMenuButtonClick() { // 메인메뉴로 나가는 메소드
        SoundManager.instance.PlaySound(AudioClipName.Click);
        isLive = false;
        PanelManager.instance.PanelOff(); // 모든 패널 꺼주기
        SoundManager.instance.PlaySound(AudioClipName.Quit);
        StopCoroutine("FilterPanelFadeOut");
        StartCoroutine("FilterPanelFadeOut");
    }

    public void NewGameButtonClick() { // 새로하기 버튼을 클릭했을때 실행하는 메소드

        SoundManager.instance.PlaySound(AudioClipName.Click);
        //adManager.ShowIBannerAd(); // 배너 광고 호출하기

        if(ItemManager.instance.fieldItemParent.transform.childCount > 0) { // 이미 만들어진 필드 아이템이 있다면
            for(int i = 0; i < ItemManager.instance.fieldItemParent.transform.childCount; i++) {
                Transform targetObject = ItemManager.instance.fieldItemParent.transform.GetChild(i);
                Destroy(targetObject.gameObject);
            }
            ItemManager.instance.GenerateItem(); // 아이템 생성 메소드 호출
            
        } else { // 기존에 만들어진 필드 아이템이 없다면
            ItemManager.instance.GenerateItem(); // 아이템 생성 메소드 호출
        }

        if(Inventory.instance.possessItems.Count > 0) { // 현재 인벤토리에 보유중인 아이템이 있다면
            Inventory.instance.possessItems.Clear(); // 보유한 아이템 전부 삭제하기
            Inventory.instance.onChangeItem.Invoke(); // 인벤토리 다시 그려주기
        }

        player.transform.position = Vector3.zero;
        maxHealth = 100;
        curHealth = maxHealth; // 체력 초기화
        curMana = 0;
        curGold = startGold;
        curExp = 0;
        curLevel = 1;
        curGameTime = 25200; // 게임시간은 오전 7시부터 시작
        originMoveSpeed = 5;
        curMoveSpeed = originMoveSpeed;
        questManager.questId = 10;
        questManager.questActionIndex = 0;
        talkIndex = 0;
        currentQuestText.text = questManager.CheckQuest();
        isHouse = false; // 기본적으로 밖에서 시작하니까

        Inventory.instance.hasCandy = false;
        Inventory.instance.hasChestKey = false;
        Inventory.instance.hasStoreKey = false;

        InitGame();
    }

    private void ResetTile()
    {
        for(int i = 0; i < iceInfinite.Length; i++) // 빙판 타일과 나무 타일을 원래 위치로 돌려주기
        {
            iceInfinite[i].transform.position = Vector3.zero;
            treeInfinite[i].transform.position = Vector3.zero;
        }
    }
    
    private void InitGame()
    {
        Invoke("ResetTile", 1.5f); // 무한맵 타일의 위치를 초기화 해주기
        
        questManager.ControlObject();
        isLive = true;
        Player.instance.isDead = false;
        Player.instance.isAttack = false;
        Player.instance.isDamaged = false;
        Player.instance.isSlide = false;
        isAction = false;

        Player.instance.anim.SetTrigger("start"); // 아래를 바라보는 애니메이션으로 바꿔주기
        NPC.instance.CancelInvoke(); // 모든 메소드의 invoke를 중지시킴
        NPC.instance.Think(); // NPC 생각 메소드 호출
        Animal.instance.CancelInvoke();
        Animal.instance.Think();
        
        startPanel.SetActive(false);
        PanelManager.instance.RedrawStatsPanel();
        
        StopCoroutine("FilterPanelFadeIn");
        StartCoroutine("FilterPanelFadeIn");
        Invoke("GlobalLightOn", 0.1f);
    }

    private IEnumerator FilterPanelFadeIn() { // 검정색 필터를 서서히 투명하게 만들어줘서 Fade In 효과를 주는 코루틴
        
        filterPanel.SetActive(true);
        
        Image filterPanelImage = filterPanel.GetComponent<Image>();
        float fadeTime = 1;

        while(0 < fadeTime) {
            fadeTime -= 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01 초마다 실행
            filterPanelImage.color = new Color(0, 0, 0, fadeTime);
        }
        
        filterPanel.SetActive(false);
    }
    
    private IEnumerator FilterPanelFadeOut() { // 검정색 필터를 서서히 어둡게 만들어줘서 Fade Out 효과를 주는 코루틴
        
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

    public IEnumerator FilterPanelFadeOutAndIn() {
        isAction = true;
        isHealing = true;
        filterPanel.SetActive(true);
        
        Image filterPanelImage = filterPanel.GetComponent<Image>();
        float fadeTime = 0;

        yield return new WaitForSeconds(0.01f);
        talkPanel.SetActive(false); // 우선 대화창을 꺼주기

        while(fadeTime < 1) {
            fadeTime += 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01 초마다 실행
            filterPanelImage.color = new Color(0, 0, 0, fadeTime);
        }
        
        while(0 < fadeTime) {
            fadeTime -= 0.01f;
            yield return new WaitForSeconds(0.01f); // 0.01 초마다 실행
            filterPanelImage.color = new Color(0, 0, 0, fadeTime);
        }
        
        isAction = false;
        filterPanel.SetActive(false);
        expSlider.SetActive(true);
        isHealing = false;
    }
    
    private void GlobalLightOn() {
        globalLight.SetActive(true);
        PanelManager.instance.PanelOff();
        PanelManager.instance.PanelOn();
    }

    public bool IsNPCPanelOn() {
        if(storagePanel.activeSelf || turnTablePanel.activeSelf || groceryStorePanel.activeSelf || equipmentStorePanel.activeSelf) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsPanelOn() {
        if(equipmentPanel.activeSelf || statsPanel.activeSelf || questPanel.activeSelf || inventoryPanel.activeSelf || turnTablePanel.activeSelf || storagePanel.activeSelf || groceryStorePanel.activeSelf || equipmentStorePanel.activeSelf || helpPanel.activeSelf || chatPanel.activeSelf) {
            return true;
        } else {
            return false;
        }
    }

    public void EscPanelOff() { // ESC 키를 눌렀을때 한번에 꺼줄 패널들
        equipmentPanel.SetActive(false);
        statsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        turnTablePanel.SetActive(false);
        questPanel.SetActive(false);
        
        storagePanel.SetActive(false);
        groceryStorePanel.SetActive(false);
        equipmentStorePanel.SetActive(false);
        
        itemDescriptionPanel.SetActive(false);
        entrustAmountPanel.SetActive(false);
        purchaseAmountPanel.SetActive(false);
        sellAmountPanel.SetActive(false);
        sellConfirmPanel.SetActive(false);
        purchaseConfirmPanel.SetActive(false);
        
        helpPanel.SetActive(false);
        PanelManager.instance.ChatPanelOnOff(1);

        TurnTableManager.instance.StopTurnTable();
    }

    public void DetailPanelOff() {
        itemDescriptionPanel.SetActive(false);
        entrustAmountPanel.SetActive(false);
        purchaseAmountPanel.SetActive(false);
        sellAmountPanel.SetActive(false);
        sellConfirmPanel.SetActive(false);
        purchaseConfirmPanel.SetActive(false);
    }
    
}