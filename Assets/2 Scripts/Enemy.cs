using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public static Enemy instance;
    
    public float curHealth; // 적의 현재체력
    public float maxHealth; // 적의 최대체력
    public float exp;
    public bool isDead;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private WaitForFixedUpdate wait;

    private void Awake() {
        curHealth = maxHealth; // 몬스터의 현재 체력을 최대 체력으로 초기화 해줌
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void Update() {

        if(!isDead && curHealth <= 0) { // 죽지 않았으면서 현재 체력이 0보다 작거나 같으면
            Dead();
        }
    }

    public void Damaged(Vector3 playerPos) {
        
        if(!isDead && curHealth > 0) // 죽지 않았으면서 현재 체력이 0보다 작거나 같으면
            StartCoroutine(KnockBack(playerPos));
    }

    private IEnumerator KnockBack(Vector3 playerPos) { // 몬스터가 플레이어의 공격에 맞았을때 넉백 효과를 주기 위한 메소드
        
        yield return wait; // 하나의 물리 프레임 딜레이
        
        Vector3 dirVec = transform.position - playerPos;
        anim.SetTrigger("hit");
        rigid.AddForce(dirVec.normalized * 7f, ForceMode2D.Impulse);

        StartCoroutine(KnockBackEnd());
    }

    private IEnumerator KnockBackEnd() { // 몬스터가 맞아서 밀려난 이후 다시 원래 자리를 찾는 메소드

        yield return wait;
        rigid.velocity = Vector2.zero;
    }
    
    private void Dead() {
        rigid.velocity = Vector2.zero;
        anim.SetTrigger("dead");
        sprite.color = new Color(1, 1, 1, 1);
        isDead = true; // 죽었다는 플래그 값 올려주기
        
        Invoke("Delete", 4f);
    }

    private void Delete() { // 몬스터의 묘지를 꺼주는 메소드
        gameObject.SetActive(false);
    }
    
}