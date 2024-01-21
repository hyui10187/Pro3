using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour {

    public static NPC instance;
    
    public int nextMove;
    
    private Rigidbody2D rigid;
    private Animator anim; 
    
    private void Awake() {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        // Move
        rigid.velocity = new Vector2(rigid.velocity.x, nextMove);
        
        // Platform Check
        Debug.DrawRay(rigid.position, Vector3.up * nextMove, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.up * nextMove, 1f, LayerMask.GetMask("Wall"));

        anim.SetBool("isChange", false);
        
        if(rayHit.collider != null) {
            Turn();
        }
    }

    public void Think() { // NPC가 어디로 이동할지 생각하는 메소드
        nextMove = Random.Range(-1, 2); // -1, 0, 1 값중에 랜덤으로 추출

        float nextThinkTime = Random.Range(2f, 5f);
        anim.SetInteger("walkDirection", nextMove);
        anim.SetBool("isChange", true);
        
        Invoke("Think", nextThinkTime);
    }

    private void Turn() {
        nextMove *= -1;
        anim.SetInteger("walkDirection", nextMove);
        anim.SetBool("isChange", true);
        
        CancelInvoke(); // 방향을 바꿔주면 기존에 실행중이던 Invoke 메소드는 중지시키기
        Invoke("Think", 2); // 방향을 바꿔준 시점부터 다시 시간초를 세어서 Think 메소드를 호출하기
    }
    
}