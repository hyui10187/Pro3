using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {
    
    public static Player instance;

    public GameManager gameManager;
    public float moveSpeed = 5f; // 이동속도
    
    private Rigidbody2D rigid;
    private Vector3 dirVec; // 플레이어의 방향에 대한 변수
    private GameObject scanObj;
    private Animator anim;
    private SpriteRenderer sprite;
    
    public float h;
    public float v;

    public bool isHorizonMove; // 대각선 이동을 막아주기 위한 플래그 변수
    public bool isDamaged;
    public bool isDead; // 플레이어가 죽었는지 체크하기 위한 변수
    public bool isSlide;


    private void Awake() {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    private void Update() {
        
        if(isDead) {
            return;
        }
        
        // Move Value
        h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal"); // GameManager의 isAction 플래그값이 true 라면 h와 v의 값을 0으로 만들어서 이동하지 못하도록 한다
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        // Check Button Down & Up
        bool hDown = gameManager.isAction ? false : Input.GetButtonDown("Horizontal"); // 버튼을 누른 여부를 저장하는 변수들도 isAction 플래그값을 기준으로 결정한다
        bool vDown = gameManager.isAction ? false : Input.GetButtonDown("Vertical");
        bool hUp = gameManager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = gameManager.isAction ? false : Input.GetButtonUp("Vertical");

        // Check Horizontal Move
        if(hDown) {
            isHorizonMove = true;
        } else if(vDown) {
            isHorizonMove = false;
        } else if(hUp || vUp) {
            isHorizonMove = h != 0;
        }
        
        // Animation
        if(anim.GetInteger("hAxisRaw") != h) { // 키보드의 방향키를 누를때 1번만 값을 주도록 조건 추가
            anim.SetBool("isChange", true); // 키를 누르고 나서 Update 메소드가 1번째로 호출될때는 true로 플래그 값을 올려준다
            anim.SetInteger("hAxisRaw", (int)h);
            
        } else if(anim.GetInteger("vAxisRaw") != v) {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
            
        } else {
            anim.SetBool("isChange", false); // 키를 누르고 나서 Update 메소드가 2번째로 호출될때부터는 false로 플래그 값을 변경해준다
        }
        
        // Direction   // 순서대로 상하좌우 값을 주는 것이다
        if(vDown && v == 1) // 키보드 위아래 방향키를 눌렀으면서(vDown) 그 값이 1이면 위쪽 방향키를 누른 것이다
            dirVec = Vector3.up; // 그러면 방향을 위쪽으로 설정해준다
        else if(vDown && v == -1)
            dirVec = Vector3.down;
        else if(hDown && h == -1)
            dirVec = Vector3.left;
        else if(hDown && h == 1)
            dirVec = Vector3.right;

        // Scan Object
        if(Input.GetButtonDown("Jump") && scanObj != null) { // 플레이어가 스페이스 바를 눌렀으면서 스캔한 오브젝트가 있는 경우
            gameManager.Action(scanObj); // GameManager한테 스캔한 게임 오브젝트를 파라미터로 던져주기
        }

        // Inventory Open/Close
        if(Input.GetButtonDown("Inventory")) { // 플레이어가 I 키를 눌렀으면
            gameManager.ControlInventory();
        }
        
    }
    
    private void FixedUpdate() {
        
        if(isDead) {
            return;
        }

        if(!isDamaged && !isSlide) { // 몬스터한테 맞았을때는 대각선으로 이동해야 하니까 조건을 걸어줌  // 미끄러질때는 이동이 안되도록 조건을 걸어줌
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
        anim.SetTrigger("dead"); // 묘비로 변하는 애니메이션 켜주기
        rigid.velocity = Vector2.zero;
        GameManager.instance.gaugeUI.SetActive(false);
        sprite.color = new Color(1, 1, 1, 1);
        isDead = true; // 플래그 올려주기
    }
    
}