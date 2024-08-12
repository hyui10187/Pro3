using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Area")) // Player의 Area
            return;

        Vector3 playerDir = Player.instance.dirVec;
        float dirY = playerDir.y;
        float posY = transform.position.y + (dirY * 60);

        if(0 < posY) // 빙판 타일이 원래 위치보다 위쪽으로 올라오지는 않도록 처리하기
            return;

        transform.Translate(Vector3.up * dirY * 60);
    }
    
}