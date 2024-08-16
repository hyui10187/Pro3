using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    
    public float curHealth; // 적의 현재체력
    public float maxHealth; // 적의 최대체력
    public float exp;
    public bool isDead;

    private Animator anim;

    private void Awake()
    {
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isDead && curHealth <= 0) // 죽지 않았으면서 현재 체력이 0보다 작거나 같으면
            Dead();
    }
    
    public void Damaged()
    {
        if(!isDead && curHealth > 0) // 죽지 않았으면서 현재 체력이 0보다 작거나 같으면
            anim.SetTrigger("damaged");
    }

    private void Dead()
    {
        GameManager.instance.curExp += exp;
        isDead = true; // 죽었다는 플래그 값 올려주기
        
        Delete();
        SpawnItem();
    }

    private void Delete()
    {
        gameObject.SetActive(false); // 나무를 꺼주기
    }

    private void SpawnItem() // 게임 오브젝트를 부쉈을때 아이템을 생성해주는 메소드
    {
        if(gameObject.CompareTag("Tree")) // 나무를 부쉈으면
        {
            ItemManager.instance.DropFruit(transform.position); // 나무 열매를 드랍
        }
        else if(gameObject.CompareTag("Stump")) // 그루터기를 부쉈으면
        {
            if(gameObject.name.Contains("BigStump")) // 큰나무 밑동일 경우
            {
                ItemManager.instance.DropMaterial(transform.position, 0); // 목재를 2개 드랍해주기
                ItemManager.instance.DropMaterial(transform.position, 0);
                return;
            }
            
            ItemManager.instance.DropMaterial(transform.position, 0); // 나무를 드랍
        }
    }
    
}