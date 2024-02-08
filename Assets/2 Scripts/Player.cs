using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {
    
    public static Player instance;
    
    public float h;
    public float v;

    [Header("Flag")]
    public bool isHorizonMove; // 대각선 이동을 막아주기 위한 플래그 변수
    public bool isDamaged;
    public bool isDead; // 플레이어가 죽었는지 체크하기 위한 변수
    public bool isSlide;
    public bool isAttack;
    public bool hasWeapon; // 무기를 소유하고 있는지 플래그

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
    
    private Rigidbody2D rigid;
    private Vector3 dirVec; // 플레이어의 방향에 대한 변수
    public GameObject scanObj;
    public Animator anim;
    private SpriteRenderer sprite;
    
    private void Awake() {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    private void Update() {
        
        if(isDead || !GameManager.instance.isLive) // 죽었으면 모든 행동 실행 못하게
            return;

        if(curTime > 0) {
            curTime -= Time.deltaTime;
        }
        
        // Move Value
        h = GameManager.instance.isAction ? 0 : Input.GetAxisRaw("Horizontal") + leftValue + rightValue; // GameManager의 isAction 플래그값이 true 라면 h와 v의 값을 0으로 만들어서 이동하지 못하도록 한다
        v = GameManager.instance.isAction ? 0 : Input.GetAxisRaw("Vertical") + upValue + downValue;

        // Check Button Down & Up
        bool hDown = GameManager.instance.isAction ? false : Input.GetButtonDown("Horizontal") || leftDown || rightDown; // 버튼을 누른 여부를 저장하는 변수들도 isAction 플래그값을 기준으로 결정한다
        bool vDown = GameManager.instance.isAction ? false : Input.GetButtonDown("Vertical") || upDown || downDown;
        bool hUp = GameManager.instance.isAction ? false : Input.GetButtonUp("Horizontal") || leftUp || rightUp;
        bool vUp = GameManager.instance.isAction ? false : Input.GetButtonUp("Vertical") || upUp || downUp ;

        // Check Horizontal Move
        if(hDown) {
            isHorizonMove = true;
        } else if(vDown) {
            isHorizonMove = false;
        } else if(hUp || vUp) {
            isHorizonMove = h != 0;
        }

        if(!isAttack) { // 공격중이면 공격 애니메이션이 끝나기 전에는 이동 애니메이션이 실행되지 않도록
            
            if(anim.GetInteger("hAxisRaw") != h) {
                // 키보드의 방향키를 누를때 1번만 값을 주도록 조건 추가
                anim.SetBool("isChange", true); // 키를 누르고 나서 Update 메소드가 1번째로 호출될때는 true로 플래그 값을 올려준다
                anim.SetInteger("hAxisRaw", (int)h);
                
            } else if(anim.GetInteger("vAxisRaw") != v) {
                anim.SetBool("isChange", true);
                anim.SetInteger("vAxisRaw", (int)v);
                
            } else {
                anim.SetBool("isChange", false); // 키를 누르고 나서 Update 메소드가 2번째로 호출될때부터는 false로 플래그 값을 변경해준다
            }
        }

        // Direction   // 순서대로 상하좌우 값을 주는 것이다
        if(vDown && v == 1) { // 키보드 위아래 방향키를 눌렀으면서(vDown) 그 값이 1이면 위쪽 방향키를 누른 것이다
            dirVec = Vector3.up; // 그러면 방향을 위쪽으로 설정해준다
            anim.SetFloat("verticalMove", v);
            anim.SetFloat("horizonMove", 0);
            
        } else if(vDown && v == -1) {
            dirVec = Vector3.down;
            anim.SetFloat("verticalMove", v);
            anim.SetFloat("horizonMove", 0);
            
        } else if(hDown && h == -1) {
            dirVec = Vector3.left;
            anim.SetFloat("horizonMove", h);
            anim.SetFloat("verticalMove", 0);
            
        } else if(hDown && h == 1) {
            dirVec = Vector3.right;
            anim.SetFloat("horizonMove", h);
            anim.SetFloat("verticalMove", 0);
        }

        // Scan Object
        if(Input.GetButtonDown("Jump") && scanObj != null) { // 플레이어가 스페이스 바를 눌렀으면서 스캔한 오브젝트가 있는 경우
            GameManager.instance.Action(scanObj); // GameManager한테 스캔한 게임 오브젝트를 파라미터로 던져주기
        }

        // Inventory Open/Close
        if(Input.GetButtonDown("Inventory")) { // 플레이어가 I 키를 눌렀으면
            GameManager.instance.ControlInventory();
        }

        if(Input.GetButtonDown("Cancel")) { // 플레이어가 ESC 키를 눌렀으면
            GameManager.instance.ControlMenuPanel();
        }
        
        if(Input.GetButtonDown("Attack") && !isAttack && curTime <= 0) { // 플레이어가 A 키를 눌렀으며, 공격 중이 아니며, 쿨타임이 안남았을 경우
            anim.SetTrigger("attack");
            anim.SetBool("isAttack", true);
            curTime = coolTime; // 쿨타임을 초기화 해줌
            isAttack = true;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position + dirVec, boxSize, 0);

            foreach(Collider2D collision in collider2Ds) {

                if(collision.CompareTag("Enemy")) { // 플레이어의 공격에 맞은게 Enemy 라면 
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                    enemy.curHealth -= 20; // Enemy의 체력을 깎아주기
                    enemy.Damaged(transform.position); // Enemy의 피격 메소드 실행
                }

                if(collision.CompareTag("Tree")) {
                    DestroyableObject desObject = collision.GetComponent<DestroyableObject>();
                    desObject.curHealth -= 20;
                    desObject.Damaged();
                }
            }
        }
        
        // Mobile Var Init   // 모바일용 변수들은 매 프레임마다 초기화 해주기
        upDown = false;
        downDown = false;
        leftDown = false;
        rightDown = false;
        upUp = false;
        downUp = false;
        leftUp = false;
        rightUp = false;
    }

    private void OnDrawGizmos() { // 피격범위 박스를 에디터에서 표시하기 위한 메소드
        Gizmos.color = Color.red; // 박스의 색상은 빨간색으로
        Gizmos.DrawWireCube(transform.position + dirVec, boxSize);
    }

    private void FixedUpdate() {

        float moveSpeed = GameManager.instance.curMoveSpeed;
        
        if(isDead) {
            return;
        }

        if(!isDamaged && !isSlide && !isAttack) { // 몬스터한테 맞았을때는 대각선으로 이동해야 하니까 조건을 걸어줌  // 미끄러질때는 이동이 안되도록 조건을 걸어줌
            Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v); // 대각선 이동을 막아주기 위한 로직
            rigid.velocity = moveVec * moveSpeed;
        }

        Debug.DrawRay(rigid.position, dirVec * 1f, new Color(0, 2f, 0)); // 첫번째 파라미터는 광선을 쏘는 위치, 두번째 파라미터는 광선을 쏘는 방향, 세번째 파라미터는 광선의 길이
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 1f, LayerMask.GetMask("Object"));
                                                                                                               // 네번째 파라미터는 스캔할 Layer
        if(rayHit.collider != null) { // 찾아낸 게임 오브젝트가 뭐라도 있으면
            scanObj = rayHit.collider.gameObject;
        } else {
            scanObj = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(isDead) {
            return;
        }
        
        if(other.gameObject.tag == "HouseEnter") { // 플레이어가 집으로 들어가면
            transform.position = GameManager.instance.housePos.position; // 건물 안의 위치로 이동시킴
            GameManager.instance.isHouse = true; // 집으로 들어갔다는 플래그 값을 올려줌

        } else if(other.gameObject.tag == "WinterFieldEnter") { // 플레이어가 밖으로 나가면
            transform.position = GameManager.instance.winterFieldPos.position; // 건물 밖의 위치로 이동시킴
            GameManager.instance.isHouse = false; // 집으로 들어갔다는 플래그 값을 내려줌
            
        } else if(other.gameObject.tag == "Enemy") { // 플레이어가 적한테 맞으면
            OnDamaged(other.transform.position);
            
        } else if(other.gameObject.tag == "Ice") {
            Slide();
            
        } else if(other.gameObject.tag == "Bullet") {
            Bullet bullet = other.GetComponent<Bullet>();
            GameManager.instance.curHealth -= bullet.damage; // 몬스터의 공격에 맞았으면 그만큼 체력을 깎아주기
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

    private void OnDamaged(Vector2 targetPos) {

        if(isDead) {
            return;
        }
            
        gameObject.layer = 10; // 무적 효과를 위해 플레이어의 Layer를 PlayerDamaged로 변경해준다
        sprite.color = new Color(1, 1, 1, 0.4f);

        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;

        rigid.AddForce(new Vector2(dir, 1) * 1, ForceMode2D.Impulse); // 맞은 반대 방향으로 튕겨나가도록
        GameManager.instance.curHealth -= 10; // 플레이어의 체력을 깎아주기
        isDamaged = true;

        if(GameManager.instance.curHealth <= 0) { // 플레이어의 체력이 0 이하가 되면
            PlayerDead();
        }
        
        Invoke("OffDamaged", 2f);
    }

    private void OffDamaged() {
        
        if(isDead) {
            return;
        }
        
        gameObject.layer = 9; // 무적 효과를 위해 플레이어의 Layer를 PlayerDamaged로 변경해준다
        sprite.color = new Color(1, 1, 1, 1);
        isDamaged = false;
    }

    public void PlayerDead() {
        
        if(isDead) {
            return;
        }
        
        anim.SetTrigger("dead"); // 묘비로 변하는 애니메이션 켜주기
        rigid.velocity = Vector2.zero;
        GameManager.instance.ControlInventory();
        GameManager.instance.gaugeUI.SetActive(false);
        GameManager.instance.expSlider.SetActive(false);
        sprite.color = new Color(1, 1, 1, 1);
        isDead = true; // 플래그 올려주기
        
        Invoke("DeadPanelOn", 2.5f); // 2.5초 뒤에 DeadPanel 켜주기
    }

    private void DeadPanelOn() {
        GameManager.instance.deadPanel.SetActive(true);
    }
    
    public void AttackEnd() {
        anim.SetBool("isAttack", false);
        isAttack = false;
    }

    public void ButtonDown(string type) {

        switch(type) {
            case "Up":
                upValue = 1;
                upDown = true;
                break;
            
            case "Down":
                downValue = -1;
                downDown = true;
                break;
            
            case "Left":
                leftValue = -1;
                leftDown = true;
                break;
            
            case "Right":
                rightValue = 1;
                rightDown = true;
                break;
            
            case "Space": // 초록색 버튼
                if(scanObj != null) {
                    GameManager.instance.Action(scanObj);
                }
                break;

            case "Attack": // 파란색 버튼
                if(!isAttack && curTime <= 0) { // 플레이어가 파란색 버튼을 눌렀으며, 공격 중이 아니며, 쿨타임이 안남았을 경우
                    anim.SetTrigger("attack");
                    anim.SetBool("isAttack", true);
                    curTime = coolTime; // 쿨타임을 초기화 해줌
                    isAttack = true;

                    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position + dirVec, boxSize, 0);

                    foreach(Collider2D collision in collider2Ds) {

                        if(collision.CompareTag("Enemy")) { // 플레이어의 공격에 맞은게 Enemy 라면 
                            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                            enemy.curHealth -= 20; // Enemy의 체력을 깎아주기
                            enemy.Damaged(transform.position); // Enemy의 피격 메소드 실행
                        }
                    }
                }
                break;
            
            case "Inventory": // 파란색 버튼
                GameManager.instance.ControlInventory();
                break;
            
            case "ESC": // 빨간색 버튼
                GameManager.instance.ControlMenuPanel();
                break;
        }
    }

    public void ButtonUp(string type) {

        switch(type) { // 버튼에서 손을 떼면 자동으로 값을 0으로 초기화
            case "Up":
                upValue = 0;
                upUp = true;
                break;
            
            case "Down":
                downValue = 0;
                downUp = true;
                break;
            
            case "Left":
                leftValue = 0;
                leftUp = true;
                break;
            
            case "Right":
                rightValue = 0;
                rightUp = true;
                break;
        }
        
    }
    
}