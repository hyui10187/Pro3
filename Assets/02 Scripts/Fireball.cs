using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage; // 파이어볼의 공격력
    public float speed; // 파이어볼의 속도

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Shot(Vector3 dir)
    {
        rigid.velocity = dir * speed;
        
        Invoke("DestroyFireball", 7f); // 파이어볼이 아디에도 닿지 않아도 7초가 지나면 자동으로 없애주기
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Wall")) // 파이어볼이 플레이어한테 닿거나 벽에 닿으면
        {
            Destroy(gameObject); // 파이어볼 삭제
        }
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
    
}