using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public static Animal instance;
    
    public int nextMove;
    public int isHorizon;
    public bool isCollision ; // 플레이어랑 충돌했는지 플래그
    public Vector2 boxSize; // 탐색하는 범위
    
    private Rigidbody2D rigid;
    private Animator anim;
    private Vector3 dirVec;

    private void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        anim.enabled = true; // 기본적으로 애니메이션은 켜주기

        RaycastHit2D[] rayHit = new RaycastHit2D[3];
        Vector2 rayPos = Vector2.zero;
        Vector2[] verticalVec = { new Vector2(-0.3f, 0), Vector2.zero, new Vector2(0.3f, 0) }; // 세로 광선의 간격
        Vector2[] horizontalVec = { new Vector2(0, -0.6f), new Vector2(0, -0.1f) }; // 가로 광선의 간격

        if(!GameManager.instance.isLive) // 게임이 실행중이 아니라면 NPC가 스스로 움직이지 않도록
        {
            return;
        }

        if(isCollision) // 플레이어가 이동형 NPC한테 말을 걸었으면
        {
            anim.enabled = false; // 이동 애니메이션 꺼주기
            rigid.velocity = Vector2.zero; // 이동 멈추기
            return;
        }

        if(isHorizon == 1) // 가로 이동이면
        {
            rigid.velocity = new Vector2(nextMove, 0);
            dirVec = Vector3.right * nextMove;

            for(int i = 0; i < 2; i++)
            {
                rayPos = rigid.position + (horizontalVec[i] * Mathf.Abs(nextMove));
                Debug.DrawRay(rayPos, dirVec * 1f, Color.green);
                rayHit[i] = Physics2D.Raycast(rayPos, dirVec, 1f, LayerMask.GetMask("Wall", "Object", "Player"));
            }
            
        }
        else // 세로 이동이면
        {
            rigid.velocity = new Vector2(0, nextMove);
            dirVec = Vector3.up * nextMove;
            
            for(int i = 0; i < 3; i++)
            {
                rayPos = rigid.position + (verticalVec[i] * nextMove);
                Debug.DrawRay(rayPos, dirVec * 1f, Color.green);
                rayHit[i] = Physics2D.Raycast(rayPos, dirVec, 1f, LayerMask.GetMask("Wall", "Object", "Player"));
            }
        }

        anim.SetBool("isChange", false); // 기본적으로 isChange 플래그 값을 false로 업데이트

        for(int i = 0; i < rayHit.Length; i++)
        {
            if(rayHit[i].collider != null)
            {
                Turn();
                break;
            }
        }
        
    }

    public void Think() // NPC가 어디로 이동할지 생각하는 메소드
    {
        anim.SetInteger("walkVertical", 0); // 이동할때는 일단 기본적으로 값을 0으로 초기화
        anim.SetInteger("walkHorizon", 0);  // 이동할때는 일단 기본적으로 값을 0으로 초기화
        
        nextMove = Random.Range(-1, 2); // -1, 0, 1 중에 랜덤으로 추출
        isHorizon = Random.Range(0, 2); // 0, 1 중에 랜덤으로 추출 (0: 세로 이동, 1: 가로 이동)

        if(isHorizon == 1) // 가로 이동이면
        {
            anim.SetInteger("walkHorizon", nextMove);
            anim.SetBool("isChange", true); // 방향을 전환할때는 플래그 값을 true로 순간적으로 바꿔줌
            
        }
        else // 세로 이동이면
        {
            anim.SetInteger("walkVertical", nextMove);
            anim.SetBool("isChange", true); // 방향을 전환할때는 플래그 값을 true로 순간적으로 바꿔줌
        }

        float nextThinkTime = Random.Range(2f, 5f); // 2~4초를 생각하도록
        Invoke("Think", nextThinkTime);
    }

    private void Turn()
    {
        nextMove *= -1;
        
        if(isHorizon == 1)
        {
            anim.SetInteger("walkHorizon", nextMove);
        }
        else
        {
            anim.SetInteger("walkVertical", nextMove);    
        }
        
        anim.SetBool("isChange", true); // 방향을 전환할때는 플래그 값을 true로 순간적으로 바꿔줌
        
        CancelInvoke(); // 방향을 바꿔주면 기존에 실행중이던 Invoke 메소드는 중지시키기
        
        float nextThinkTime = Random.Range(2f, 5f); // 2~4초를 생각하도록
        Invoke("Think", nextThinkTime); // 방향을 바꿔준 시점부터 다시 시간초를 세어서 Think 메소드를 호출하기
    }
    
}