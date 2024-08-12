using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    
    public int damage; // 화살의 공격력
    public float speed; // 화살의 속도

    private Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Shot(Vector3 dir) {
        rigid.velocity = dir * speed;
        
        Invoke("DestroyArrow", 7f); // 화살이 아디에도 닿지 않아도 7초가 지나면 자동으로 없애주기
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.layer == 6) {
            DestroyableObject destroyableObject = other.GetComponent<DestroyableObject>();
            if(destroyableObject) {
                destroyableObject.curHealth -= damage;
                destroyableObject.Damaged();
                Destroy(gameObject);
            }
        }
        
        if(other.CompareTag("Monster")) { // 화살이 몬스터한테 닿거나 벽에 닿으면
            Destroy(gameObject); // 화살 삭제
            Monster monster = other.GetComponent<Monster>();
            //monster.curHealth -= damage;
            monster.Damaged(Player.instance.transform.position);
        } else if(other.CompareTag("Wall")) {
            Destroy(gameObject); // 화살 삭제
        }
    }

    private void DestroyArrow() {
        Destroy(gameObject);
    }
    
}