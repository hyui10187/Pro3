using System;
using System.Collections;
using System.Collections.Generic;
using MarksAssets.VibrationWebGL;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    
    [Header("Move")]
    public float h;
    public float v;
    public float touchX; // 조이스틱에서 받아온 x 값
    public float touchY; // 조이스틱에서 받아온 y 값

    [Header("Flag")]
    public bool isHorizonMove; // 대각선 이동을 막아주기 위한 플래그 변수
    public bool isDamaged;
    public bool isDead; // 플레이어가 죽었는지 체크하기 위한 변수
    public bool isSlide;
    public bool isAttack;

    [Header("Attack")]
    public Vector2 boxSize; // 공격이 적중되는 범위
    public float curTime;
    public float coolTime; // 공격을 한번 하고 다음 공격을 할때까지의 쿨타임

    [Header("Mobile")]
    public int upValue;
    public int downValue;
    public int leftValue;
    public int rightValue;
    public bool upDown;
    public bool downDown;
    public bool leftDown;
    public bool rightDown;
    public bool upUp;
    public bool downUp;
    public bool leftUp;
    public bool rightUp;
    public VirtualJoystick virtualJoystick;

    [Header("ETC")]
    public RangedWeapon rangedWeapon;
    public GameObject area;
    public bool isFlipInit;
    //public AdManager adManager;
    
    private Rigidbody2D rigid;
    public Vector3 dirVec; // 플레이어의 방향에 대한 변수
    public GameObject scanObj;
    public Animator anim;
    private SpriteRenderer sprite;

    public Action OnSleepHandler;

    private void Awake() {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    private void Update() {
        
        if(isDead || !GameManager.instance.isLive) // 플레이어가 죽었거나, 게임이 라이브 상태가 아니면
            return;

        if(curTime > 0) {
            curTime -= Time.deltaTime;
        }

        area.transform.position = transform.position;
        
        JoystickTouch();
        PlayerMove();
        PlayerScan();
        
        if(Input.GetButtonDown("Jump")) { // 플레이어가 스페이스 바를 눌렀으면
            PlayerAction();
        }

        if(Input.GetButtonDown("Quest")) {
            PanelManager.instance.QuestOnOff();
        }

        if(Input.GetButtonDown("Stats")) { // 플레이어가 S 키를 눌렀으면
            PanelManager.instance.StatsOnOff();
        }
        
        if(Input.GetButtonDown("Equipment")) { // 플레이어가 E 키를 눌렀으면
            PanelManager.instance.EquipmentOnOff();
        }
        
        if(Input.GetButtonDown("Inventory")) { // 플레이어가 I 키를 눌렀으면
            PanelManager.instance.InventoryOnOff();
        }

        if(Input.GetButtonDown("Cancel")) { // 플레이어가 ESC 키를 눌렀으면
            PlayerEsc();
        }

        if(Input.GetButtonDown("Help")) { // 플레이어가 H 키를 눌렀으면
            PanelManager.instance.HelpOnOff();
        }
        
        if(Input.GetButtonDown("Chat") && !GameManager.instance.chatPanel.activeSelf) { // 플레이어가 C 키를 눌렀으면
            PanelManager.instance.ChatPanelOnOff(0);
        }
        
        if(Input.GetButtonDown("Attack")) { // 플레이어가 A 키를 눌렀으면
            PlayerAttack();
        }
        
        // 모바일용 변수들은 매 프레임마다 초기화 해주기
        upDown = false;
        downDown = false;
        leftDown = false;
        rightDown = false;
    }

    private void PlayerEsc() {
        if(GameManager.instance.IsPanelOn()) {
            GameManager.instance.EscPanelOff();
        } else {
            PanelManager.instance.MenuOnOff();
        }
    }
    
    private void PlayerMove() {

        if(GameManager.instance.IsNPCPanelOn()) {
            return;
        }
        
        // Move Value
        h = GameManager.instance.isAction ? 0 : Input.GetAxisRaw("Horizontal") + leftValue + rightValue; // GameManager의 isAction 플래그값이 true 라면 h와 v의 값을 0으로 만들어서 이동하지 못하도록 한다
        v = GameManager.instance.isAction ? 0 : Input.GetAxisRaw("Vertical") + upValue + downValue;

        // Check Button Down & Up
        bool hDown = GameManager.instance.isAction ? false : Input.GetButtonDown("Horizontal") || leftDown || rightDown; // 버튼을 누른 여부를 저장하는 변수들도 isAction 플래그값을 기준으로 결정한다
        bool vDown = GameManager.instance.isAction ? false : Input.GetButtonDown("Vertical") || upDown || downDown;
        bool hUp = GameManager.instance.isAction ? false : Input.GetButtonUp("Horizontal") || leftUp || rightUp;
        bool vUp = GameManager.instance.isAction ? false : Input.GetButtonUp("Vertical") || upUp || downUp ;
        
        if(hDown) {
            isHorizonMove = true;
            dirVec = new Vector3(h, 0, 0);
            anim.SetFloat("horizonMove", h);
            anim.SetFloat("verticalMove", 0);

        } else if(vDown) {
            isHorizonMove = false;
            dirVec = new Vector3(0, v, 0);
            anim.SetFloat("verticalMove", v);
            anim.SetFloat("horizonMove", 0);

        } else if(hUp || vUp) {
            isHorizonMove = h != 0;

            if(isHorizonMove) {
                dirVec = new Vector3(h, 0, 0);
                anim.SetFloat("horizonMove", h);
                anim.SetFloat("verticalMove", 0);
            }

            if(!isHorizonMove && v != 0) {
                dirVec = new Vector3(0, v, 0);
                anim.SetFloat("verticalMove", v);
                anim.SetFloat("horizonMove", 0);
            }
        }
        
        bool isHorizonChanged = false;
        bool isVerticalChanged = false;

        if(!isAttack) {
            if(anim.GetInteger("hAxisRaw") != h) {
                anim.SetInteger("hAxisRaw", (int)h);
                isHorizonChanged = true;

                if(v != 0 && h == 0) {
                    isVerticalChanged = true;
                }
            }

            if(anim.GetInteger("vAxisRaw") != v) {
                anim.SetInteger("vAxisRaw", (int)v);
                isVerticalChanged = true;
                
                if(h != 0 && v == 0) {
                    isHorizonChanged = true;
                }
            }
        }
        
        anim.SetBool("isHorizonChanged", isHorizonChanged);
        anim.SetBool("isVerticalChanged", isVerticalChanged);
    }

    public void PlayerAction() {
        if(scanObj == null || GameManager.instance.IsNPCPanelOn() || isDead || isDamaged || !GameManager.instance.isLive || GameManager.instance.chatPanel.activeSelf) {
            return;
        }
        GameManager.instance.Action(scanObj); // GameManager한테 스캔한 게임 오브젝트를 파라미터로 던져주기
    }

    public void PlayerSleep() {
        SoundManager.instance.PlaySound(AudioClipName.Heal);
        //adManager.ShowInterstitialAd(); // 전면 광고 호출하기
        //adManager.ShowIBannerAd();

        Invoke("PlayerDirFlip", 1.5f);
        OnSleepHandler?.Invoke();
    }

    private void PlayerDirFlip() {
        dirVec *= -1; // 플레이어 캐릭터가 침대에 들어갈때 방향과 반대 방향을 바라보도록
        anim.SetInteger("dirX", (int)dirVec.x);
        anim.SetInteger("dirY", (int)dirVec.y);
        
        if(dirVec.y < 0) {
            anim.SetTrigger("start");
        }
    }

    private void PlayerScan() { // 플레이어가 NPC한테 가까이 다가갔을때 NPC 이름을 띄워주기 위한 메소드
        
        Debug.DrawRay(rigid.position, dirVec * 1f, new Color(0, 2f, 0)); // 첫번째 파라미터는 광선을 쏘는 위치, 두번째 파라미터는 광선을 쏘는 방향, 세번째 파라미터는 광선의 길이
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 1f, LayerMask.GetMask("Object", "NPC", "Animal"));
                                                                                                               // 네번째 파라미터는 스캔할 Layer
        if(rayHit.collider != null) { // 찾아낸 게임 오브젝트가 뭐라도 있으면
            scanObj = rayHit.collider.gameObject;
        } else {
            scanObj = null;
        }

        if(scanObj) {
            ObjData objData = scanObj.GetComponent<ObjData>();
            if(objData != null && objData.isNpc) {
                GameManager.instance.npcNamePanel.transform.position = Camera.main.WorldToScreenPoint(scanObj.transform.position + new Vector3(0, 1, 0));
                GameManager.instance.npcNamePanelText.text = scanObj.gameObject.name;
                GameManager.instance.npcNamePanel.SetActive(true);

                if(!GameManager.instance.talkPanel.activeSelf && !GameManager.instance.IsPanelOn()) {
                    GameManager.instance.talkInformMessage.SetActive(true);   
                } else {
                    GameManager.instance.talkInformMessage.SetActive(false);
                }
            } else {
                GameManager.instance.npcNamePanel.SetActive(false);
                GameManager.instance.talkInformMessage.SetActive(false);
            }
        } else {
            GameManager.instance.npcNamePanel.SetActive(false);
            GameManager.instance.talkInformMessage.SetActive(false);
        }
    }
    
    public void PlayerAttack() {

        if(isAttack || 0 < curTime || GameManager.instance.isAction || GameManager.instance.IsNPCPanelOn() || isDead || !GameManager.instance.isLive || GameManager.instance.chatPanel.activeSelf) {
            return; // 플레이어가 이미 공격 중이거나 공격 쿨타임이 남아있거나 창고 패널이 켜져있거나 상점 패널이 켜져있으면 돌려보내기
        }
        
        if(!Inventory.instance.equipSword && !Inventory.instance.equipBow) { // 무기를 장착하고 있지 않으면
            SoundManager.instance.PlaySound(AudioClipName.Alert);
            AlertManager.instance.SmallAlertMessageOn(ItemName.공백, 6); // 공격불가 알림메시지 띄워주기
            return;
        }

        if(Inventory.instance.equipSword) { // 소드를 장착하고 공격할 경우
            anim.SetTrigger("attack");
            anim.SetBool("isAttack", true);
            SoundManager.instance.PlaySound(AudioClipName.Attack);
            curTime = coolTime; // 쿨타임을 초기화 해줌
            isAttack = true;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position + dirVec, boxSize, 0);

            foreach(Collider2D collision in collider2Ds) {

                if(collision.CompareTag("Monster")) { // 플레이어의 공격에 맞은게 Monster 라면 
                    Monster monster = collision.gameObject.GetComponent<Monster>();
                    monster.CurHealth -= GameManager.instance.itemAttackPower + GameManager.instance.playerAttackPower;
                    monster.Damaged(transform.position); // Enemy의 피격 메소드 실행
                }

                if(collision.CompareTag("Tree")) { // 플레이어의 공격에 맞은게 Tree 라면
                    DestroyableObject desObject = collision.GetComponent<DestroyableObject>();
                    desObject.curHealth -= GameManager.instance.itemAttackPower + GameManager.instance.playerAttackPower;
                    desObject.Damaged();
                }

                if(collision.CompareTag("Stump")) { // 플레이어의 공격에 맞은게 Stump 라면
                    DestroyableObject desObject = collision.GetComponent<DestroyableObject>();
                    desObject.curHealth -= GameManager.instance.itemAttackPower + GameManager.instance.playerAttackPower;
                    desObject.Damaged();
                }
            }   
        } else if(Inventory.instance.equipBow) { // 활을 장착하고 공격할 경우
            rangedWeapon.Fire(dirVec);
        }

    }
    
    public void AttackEnd() {
        anim.SetBool("isAttack", false);
        isAttack = false;
        
        if(h != 0 || v != 0) { // 계속 이동하는 상태이면서 공격 애니메이션이 끝났을 경우
            CancelInvoke("KeepWalk");
            Invoke("KeepWalk", 0.1f);
        }
    }
    
    private void JoystickTouch() { // 가상 조이스틱 터치

        if(GameManager.instance.IsNPCPanelOn()) {
            return;
        }
        
        touchX = virtualJoystick.TouchHorizontal();
        touchY = virtualJoystick.TouchVertical();
        
        if(Mathf.Abs(touchY) < Mathf.Abs(touchX)) { // x 값이 y 값보다 경우

            touchY = 0;

            if(0 < touchX) {
                rightDown = true;
                upValue = 0;
                downValue = 0;
                leftValue = 0;
                rightValue = 1;
            } else {
                leftDown = true;
                upValue = 0;
                downValue = 0;
                rightValue = 0;
                leftValue = -1;
            }

        } else if(Mathf.Abs(touchY) > Mathf.Abs(touchX)) { // y값이 더 클 경우
         
            touchX = 0;

            if(0 < touchY) {
                upDown = true;
                rightValue = 0;
                leftValue = 0;
                downValue = 0;
                upValue = 1;
            } else {
                downDown = true;
                rightValue = 0;
                leftValue = 0;
                upValue = 0;
                downValue = -1;
            }
            
        } else { // x와 y의 값이 동일할 경우
            touchX = 0;
            touchY = 0;
            rightValue = 0;
            leftValue = 0;
            upValue = 0;
            downValue = 0;
        }
        
    }
    
    private void OnDrawGizmos() { // 피격범위 박스를 유니티 에디터에서 표시하기 위한 메소드
        Gizmos.color = Color.red; // 박스의 색상은 빨간색으로
        Gizmos.DrawWireCube(transform.position + dirVec, boxSize);
    }

    private void FixedUpdate() {

        float moveSpeed = GameManager.instance.curMoveSpeed;
        
        if(isDead || GameManager.instance.IsNPCPanelOn()) {
            return;
        }
        
        if(!isDamaged && !isSlide && !isAttack) { // 몬스터한테 맞았을때는 대각선으로 이동해야 하니까 조건을 걸어줌  // 미끄러질때는 이동이 안되도록 조건을 걸어줌
            anim.SetInteger("dirX", 0);
            anim.SetInteger("dirY", 0);
            Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v); // 대각선 이동을 막아주기 위한 로직
            rigid.velocity = moveVec * moveSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(isDead) {
            return;
        }

        if(other.gameObject.CompareTag("HouseEnter")) { // 플레이어가 집으로 들어가면
            SoundManager.instance.PlaySound(AudioClipName.Door);
            transform.position = GameManager.instance.housePos.position; // 건물 안의 위치로 이동시킴
            GameManager.instance.isHouse = true; // 집으로 들어갔다는 플래그 값을 올려줌

        } else if(other.gameObject.CompareTag("FieldEnter")) { // 플레이어가 필드로 나가면
            SoundManager.instance.PlaySound(AudioClipName.Stair);
            transform.position = GameManager.instance.winterFieldPos.position; // 건물 밖의 위치로 이동시킴
            GameManager.instance.isHouse = false; // 집으로 들어갔다는 플래그 값을 내려줌
            
        } else if(other.gameObject.CompareTag("Monster")) { // 플레이어가 적한테 닿으면

            Monster monster = other.gameObject.GetComponent<Monster>();
            
            if(monster.IsDead) {
                return;
            }
            PlayerDamaged(other.transform.position, monster.CollisionDamage);
            
        } else if(other.gameObject.CompareTag("Obstacle")) {
            PlayerDamaged(other.transform.position, GameManager.instance.obstacleDamage);
            
        } else if(other.gameObject.CompareTag("Ice")) { // 얼음
            Slide();
            
        } else if(other.gameObject.CompareTag("Fireball")) { // 플레이어가 몬스터의 파이어볼에 맞으면
            Fireball fireball = other.GetComponent<Fireball>();
            PlayerDamaged(Vector2.zero, fireball.damage); // 파이어볼의 공격력만큼 체력을 깎아주기
            
        } else if(other.CompareTag("Thorn")) { // 플레이어가 가시에 닿았으면
            PlayerDamaged(Vector2.zero, (int)GameManager.instance.curHealth); // 체력을 전부 닳도록 해주기
        }

        for(int i = 1; i < GameManager.instance.downStairPos.Length; i++) { // 계단을 이용했을때 이동로직
            if(other.gameObject.name == "DownStair " + i) {
                SoundManager.instance.PlaySound(AudioClipName.Stair);
                transform.position = GameManager.instance.downStairPos[i].position;
            } else if(other.gameObject.name == "UpStair " + i) {
                SoundManager.instance.PlaySound(AudioClipName.Stair);
                transform.position = GameManager.instance.upStairPos[i].position;
            }
        }

        for(int i = 1; i < GameManager.instance.downLadderPos.Length; i++) { // 사다리를 이용했을때 이동로직
            if(other.gameObject.name == "DownLadder " + i) {
                SoundManager.instance.PlaySound(AudioClipName.Stair);
                transform.position = GameManager.instance.downLadderPos[i].position;
            } else if(other.gameObject.name == "UpLadder " + i) {
                SoundManager.instance.PlaySound(AudioClipName.Stair);
                transform.position = GameManager.instance.upLadderPos[i].position;
            }
        }
        
        for(int i = 1; i < GameManager.instance.downHolePos.Length; i++) { // 사다리를 이용했을때 이동로직
            if(other.gameObject.name == "DownHole " + i) {
                transform.position = GameManager.instance.downHolePos[i].position;
            }
        }

        if(other.gameObject.name == "DoorInCheck 1" && Inventory.instance.hasStoreKey && !Inventory.instance.isDoorOpen) {
            Transform parentTransform = other.transform.parent; // 부모 Transform을 우선 가져오고
            Transform[] childTransforms = parentTransform.GetComponentsInChildren<Transform>(true); // 꺼져있는 자식 컴포넌트도 가져오기

            foreach(Transform childTransform in childTransforms) {
                if(childTransform.gameObject.name == "Locked") {
                    childTransform.gameObject.SetActive(false);
                } else if(childTransform.gameObject.name == "Unlocked") {
                    childTransform.gameObject.SetActive(true);
                    break;
                }
            }
        }
        
        if(other.gameObject.name == "ChestCheck 1" && Inventory.instance.hasChestKey && !Inventory.instance.isChestOpen) {
            Transform parentTransform = other.transform.parent; // 부모 Transform을 우선 가져오고
            Transform[] childTransforms = parentTransform.GetComponentsInChildren<Transform>(true); // 꺼져있는 자식 컴포넌트도 가져오기

            foreach(Transform childTransform in childTransforms) {
                if(childTransform.gameObject.name == "Locked") {
                    childTransform.gameObject.SetActive(false);
                } else if(childTransform.gameObject.name == "Unlocked") {
                    childTransform.gameObject.SetActive(true);
                    break;
                }
            }
        }
        
        for(int i = 1; i < GameManager.instance.doorInPos.Length; i++) { // 문을 이용했을때 이동로직
            if(other.gameObject.name == "DoorIn " + i) {
                SoundManager.instance.PlaySound(AudioClipName.Door);
                transform.position = GameManager.instance.doorInPos[i].position;
            } else if(other.gameObject.name == "DoorOut " + i) {
                transform.position = GameManager.instance.doorOutPos[i].position;
            }
        }
        
        if(other.CompareTag("FireplaceSound")) {
            SoundManager.instance.PlaySound(AudioClipName.FirePlace, true);
        }
        
        if(other.CompareTag("WaterSound")) {
            SoundManager.instance.PlaySound(AudioClipName.Water, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("FireplaceSound")) {
            SoundManager.instance.StopSound();
        }
        
        if(other.CompareTag("WaterSound")) {
            SoundManager.instance.StopSound();
        }
    }

    private void Slide() {

        if(h != 0) {
            isSlide = true;
            rigid.AddForce(new Vector2(h, 0) * 5, ForceMode2D.Impulse);
   
        } else if(v != 0) {
            isSlide = true;
            rigid.AddForce(new Vector2(0, v) * 5, ForceMode2D.Impulse);
        }

        StartCoroutine(SlideEnd());
    }
    
    private IEnumerator SlideEnd() {
        yield return new WaitForSeconds(1f); // 미끄러지고 2초 뒤에
        isSlide = false; // 플래그 내려주기
    }

    public void PlayerDamaged(Vector2 targetPos, int damage) {

        if(isDead) {
            return;
        }
        
        VibrationWebGL.Vibrate(100); // 플레이어가 대미지를 입으면 진동 피드백을 주기

        if(targetPos != Vector2.zero) {
            gameObject.layer = 10; // 무적 효과를 위해 플레이어의 Layer를 PlayerDamaged로 변경해주기
            sprite.color = new Color(1, 1, 1, 0.4f);

            //int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
            //rigid.AddForce(new Vector2(dir, 1) * 3f, ForceMode2D.Impulse); // 맞은 반대 방향으로 튕겨나가도록
            //isDamaged = true;
        }

        GameManager.instance.healthManaMessageText.text = "-" + damage;
        AlertManager.instance.HealthMessageOn();
        GameManager.instance.curHealth -= damage; // 플레이어의 체력을 깎아주기

        if(GameManager.instance.curHealth <= 0) { // 플레이어의 체력이 0 이하가 되면
            PlayerDead();
        }
        
        //Invoke("InitVelocity", 0.5f);
        Invoke("PlayerDamagedOff", 2f);
    }

    private void InitVelocity() {
        rigid.velocity = Vector2.zero;
    }
    
    private void PlayerDamagedOff() {
        
        if(isDead) {
            return;
        }
        
        gameObject.layer = 9; // 무적 효과를 위해 플레이어의 Layer를 PlayerDamaged로 변경해준다
        sprite.color = new Color(1, 1, 1, 1);
        isDamaged = false;
        GameManager.instance.healthManaMessage.SetActive(false);
    }

    public void PlayerDead() {
        
        if(isDead) {
            return;
        }

        SoundManager.instance.PlaySound(AudioClipName.Dead);
        anim.SetTrigger("dead"); // 묘비로 변하는 애니메이션 켜주기
        rigid.velocity = Vector2.zero;
        GameManager.instance.expSlider.SetActive(false);
        GameManager.instance.inventoryPanel.SetActive(false);
        GameManager.instance.itemDescriptionPanel.SetActive(false);
        sprite.color = new Color(1, 1, 1, 1);
        isDead = true; // 플래그 올려주기
        
        Invoke("DeadPanelOn", 2.5f); // 2.5초 뒤에 DeadPanel 켜주기
    }

    private void DeadPanelOn() {
        GameManager.instance.deadPanel.SetActive(true);
    }

    private void KeepWalk() { // 걸으면서 공격할 경우에 공격이 끝나고 다시 걷는 애니메이션을 실행시켜주기 위한 메소드
        anim.SetBool("isHorizonChanged", true);
        anim.SetBool("isVerticalChanged", true);
    }

}