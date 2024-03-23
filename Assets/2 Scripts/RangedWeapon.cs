using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour {

    public static RangedWeapon instance;
    
    public GameObject arrowPrefab; // 생성해줄 화살 프리팹
    public float shotSpeed; // 화살 발사 속도
    public float timer;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        // timer += Time.deltaTime;
        //
        // if(timer > shotSpeed) {
        //     timer = 0;
        //     Fire();
        // }
    }

    public void Fire(Vector3 dirVec) { // 화살을 발사하는 메소드
        
        SoundManager.instance.PlayAttackSound();

        GameObject arrowObj = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform); // 총알 생성
        
        if(0 < dirVec.x) {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, -135);
        } else if(dirVec.x < 0) {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, 45);
        } else if(0 < dirVec.y) {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, -45);
        } else if(dirVec.y < 0) {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, 135);
        }

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrow.Shot(dirVec); // 총알 발사
    }
    
}