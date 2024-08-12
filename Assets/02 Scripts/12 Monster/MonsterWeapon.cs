using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{

    public GameObject fireBallPrefab; // 생성해줄 파이어볼 프리팹
    public float shotSpeed; // 파이어볼 발사 속도
    public float timer;
    
    private Monster monster;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > shotSpeed) {
            timer = 0;
            Fire();
        }
    }

    private void Fire() {
        if(!monster.scanner.target || monster.IsDead) { // 플레이어가 사정거리의 밖에 있으면 로직 실행안함
            return;
        }

        Vector3 targetPos = monster.scanner.target.transform.position; // 플레이어의 위치
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        
        GameObject fireballObj = Instantiate(fireBallPrefab, transform.position, Quaternion.identity, transform); // 파이어볼 생성
        fireballObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        Fireball fireball = fireballObj.GetComponent<Fireball>();
        fireball.Shot(dir); // 파이어볼 발사
    }
    
}