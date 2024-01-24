using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour {

    public static NPC instance;
    
    public int nextMove;
    public int isHorizon;
    public bool isCollision ; // 플레이어랑 충돌했는지 플래그
    
    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake() {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {

        if(!GameManager.instance.isLive) { // 게임이 실행중이 아니라면 이동시키지 않기
            return;
        }

        if(isCollision) {
            anim.enabled = false; // 애니메이션 꺼주기
            rigid.mass = 10000;
            return;
        }

        RaycastHit2D rayHit;
        
        // Move
        if(isHorizon == 1) { // 가로 이동이면
            rigid.velocity = new Vector2(nextMove, 0);
            Debug.DrawRay(rigid.position, Vector3.right * nextMove, new Color(0, 1, 0));
            rayHit = Physics2D.Raycast(rigid.position, Vector3.right * nextMove, 1f, LayerMask.GetMask("Wall"));
            
        } else { // 세로 이동이면
            rigid.velocity = new Vector2(0, nextMove);
            Debug.DrawRay(rigid.position, Vector3.up * nextMove, new Color(0, 1, 0));
            rayHit = Physics2D.Raycast(rigid.position, Vector3.up * nextMove, 1f, LayerMask.GetMask("Wall"));
        }

        anim.SetBool("isChange", false); // 기본적으로 isChange 플래그 값을 false로 업데이트
        
        if(rayHit.collider != null) {
            Turn();
        }
    }

    public void Think() { // NPC가 어디로 이동할지 생각하는 메소드
        
        anim.SetInteger("walkVertical", 0); // 이동할때는 일단 기본적으로 값을 0으로 초기화
        anim.SetInteger("walkHorizon", 0);  // 이동할때는 일단 기본적으로 값을 0으로 초기화
        
        nextMove = Random.Range(-1, 2); // -1, 0, 1 중에 랜덤으로 추출
        isHorizon = Random.Range(0, 2); // 0, 1 중에 랜덤으로 추출 (0: 세로 이동, 1: 가로 이동)

        if(isHorizon == 1) { // 가로 이동이면
            anim.SetInteger("walkHorizon", nextMove);
            anim.SetBool("isChange", true); // 방향을 전환할때는 플래그 값을 true로 순간적으로 바꿔줌
            
        } else { // 세로 이동이면
            anim.SetInteger("walkVertical", nextMove);
            anim.SetBool("isChange", true); // 방향을 전환할때는 플래그 값을 true로 순간적으로 바꿔줌
        }

        float nextThinkTime = Random.Range(2f, 5f); // 2~4초를 생각하도록
        Invoke("Think", nextThinkTime);
    }

    private void Turn() {

        nextMove *= -1;
        
        if(isHorizon == 1) {
            anim.SetInteger("walkHorizon", nextMove);
            
        } else {
            anim.SetInteger("walkVertical", nextMove);    
        }
        
        anim.SetBool("isChange", true); // 방향을 전환할때는 플래그 값을 true로 순간적으로 바꿔줌
        
        CancelInvoke(); // 방향을 바꿔주면 기존에 실행중이던 Invoke 메소드는 중지시키기
        
        float nextThinkTime = Random.Range(2f, 5f); // 2~4초를 생각하도록
        Invoke("Think", nextThinkTime); // 방향을 바꿔준 시점부터 다시 시간초를 세어서 Think 메소드를 호출하기
    }

}