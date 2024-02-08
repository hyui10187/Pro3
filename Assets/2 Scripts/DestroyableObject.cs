using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {
    
    public float curHealth; // 적의 현재체력
    public float maxHealth; // 적의 최대체력
    public float exp;
    public bool isDead;

    private Animator anim;

    private void Awake() {
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if(!isDead && curHealth <= 0) { // 죽지 않았으면서 현재 체력이 0보다 작거나 같으면
            Dead();
        }
    }
    
    public void Damaged() {

        if(!isDead && curHealth > 0) { // 죽지 않았으면서 현재 체력이 0보다 작거나 같으면
            anim.SetTrigger("damaged");
        }
    }

    private void Dead() {
        GameManager.instance.curExp += exp;
        isDead = true; // 죽었다는 플래그 값 올려주기
        
        Delete();
        SpawnItem();
    }

    private void Delete() {
        gameObject.SetActive(false); // 나무를 꺼주기
    }

    private void SpawnItem() { // 나무가 죽으면 열매 아이템을 드랍해주는 메소드
        ItemManager.instance.DropFruit(transform.position);
    }
    
}