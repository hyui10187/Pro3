using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject prefab; // 생성해줄 총알 프리팹
    public float shotSpeed; // 총알 발사 속도
    public float timer;
    
    private Enemy enemy;

    private void Awake() {
        enemy = GetComponentInParent<Enemy>();
    }

    private void Update() {
        timer += Time.deltaTime;

        if(timer > shotSpeed) {
            timer = 0;
            Fire();
        }
    }

    private void Fire() {
        if(!enemy.scanner.target || enemy.isDead) { // 플레이어가 사정거리의 밖에 있으면 로직 실행안함
            return;
        }

        Vector3 targetPos = enemy.scanner.target.transform.position; // 플레이어의 위치
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        
        GameObject bulletObj = Instantiate(prefab, transform.position, Quaternion.identity, transform); // 총알 생성
        bulletObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Shot(dir); // 총알 발사
    }
    
}