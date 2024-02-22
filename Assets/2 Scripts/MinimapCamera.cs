using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {

    public bool x; // 각각의 값이 true이면 target의 좌표, false이면 현재 좌표를 그대로 사용
    public bool y;
    public bool z;
    public Transform target;

    private void Update() {

        if(!target) {
            return;
        }

        float a = x ? target.position.x : transform.position.x;
        float b = y ? target.position.y : transform.position.y;
        float c = z ? target.position.z : transform.position.z;
        
        Vector3 targetPos = new Vector3(a, b, c);

        transform.position = targetPos; // Minimap Camera의 위치를 플레이어의 위치를 따라서 움직여주기
    }
    
}