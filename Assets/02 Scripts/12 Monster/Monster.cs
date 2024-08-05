using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour 
{
    [Header("Component")]
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;

    [Header("Stats")]
    [SerializeField] private float curHealth; // 몬스터의 현재체력
    [SerializeField] protected float maxHealth; // 몬스터의 최대체력
    [SerializeField] protected float exp; // 몬스터를 잡으면 주는 경험치
    [SerializeField] protected int collisionDamage;
    [SerializeField] private bool isDead;
    
    [Header("ETC")]
    private WaitForFixedUpdate wait;
    public Scanner scanner;
    public int jsonIndex; // JSON에서 가져온 값의 인덱스

    public float CurHealth
    {
        get => curHealth;
        set => curHealth = value;
    }
    
    public float MaxHealth => maxHealth;
    public float EXP => exp;
    public int CollisionDamage => collisionDamage;
    
    public bool IsDead => isDead;

    private void Start()
    {
        Init();
    }

    private void Update() 
    {
        if(!isDead && CurHealth <= 0) // 죽지 않았으면서 현재 체력이 0보다 작거나 같으면
            Dead();
    }

    private void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
        wait = new WaitForFixedUpdate();
        
        EnemyDataJson enemyDataJson = JsonManager.Instance.enemyDataJson;
        
        maxHealth = enemyDataJson.monsterData[jsonIndex].maxHealth;
        CurHealth = MaxHealth;
        exp = enemyDataJson.monsterData[jsonIndex].exp;
        collisionDamage = enemyDataJson.monsterData[jsonIndex].collisionDamage;
    }

    public void Damaged(Vector3 playerPos)
    {
        if(!isDead && CurHealth > 0) // 죽지 않았으면서 현재 체력이 0보다 크다면
            StartCoroutine(KnockBack(playerPos));
    }

    private IEnumerator KnockBack(Vector3 playerPos) { // 몬스터가 플레이어의 공격에 맞았을때 넉백 효과를 주기 위한 메소드
        
        yield return wait; // 하나의 물리 프레임 딜레이
        
        Vector3 dirVec = transform.position - playerPos;
        anim.SetTrigger("hit");
        rigid.AddForce(dirVec.normalized * 7f, ForceMode2D.Impulse);

        StartCoroutine(KnockBackEnd());
    }

    private IEnumerator KnockBackEnd() // 몬스터가 맞아서 밀려난 이후 다시 원래 자리를 찾는 메소드
    {
        yield return wait;
        rigid.velocity = Vector2.zero;
    }
    
    private void Dead()
    {
        GameManager.instance.curExp += EXP;
        rigid.velocity = Vector2.zero;
        anim.SetTrigger("dead");
        sprite.color = new Color(1, 1, 1, 1);
        isDead = true; // 죽었다는 플래그 값 올려주기

        Invoke("Delete", 2);
        Invoke("SpawnItem", 2.1f);
    }

    protected virtual void Delete() // 몬스터가 죽고 묘지를 꺼주는 메소드
    {
        gameObject.SetActive(false);
    }
    
    protected abstract void SpawnItem(); // 몬스터가 죽고 아이템을 드롭해주는 메소드

}