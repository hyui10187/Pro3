using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float damage; // 총알의 공격력
    public float speed; // 총알의 속도

    private Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Shot(Vector3 dir) {
        rigid.velocity = dir * speed;
        
        Invoke("DestroyBullet", 7f);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Player") || other.CompareTag("Wall")) {
            Destroy(gameObject); // 총알 삭제
        }
    }

    private void DestroyBullet() {
        Destroy(gameObject);
    }
    
}