using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public GameObject arrowPrefab; // 생성해줄 화살 프리팹
    public float shotCoolTime; // 화살발사 쿨타임
    public float timer;

    private void Update()
    {
        if(timer < shotCoolTime) // timer가 너무 무제한으로 커지는 것을 막아주기 위한 로직
            timer += Time.deltaTime;
    }

    public void Fire(Vector3 dirVec) // 화살을 발사하는 메소드
    {
        if(timer < shotCoolTime) // 발사 쿨타임이 차지 않았으면
            return;

        if(Inventory.instance.arrowCnt == 0) // 화살을 보유하고 있지 않으면
        {
            AlertManager.instance.BigAlertMessageOn(ItemName.공백, 18);
            return;
        }
        
        SoundManager.instance.PlaySound(AudioClipName.Attack);
        GameObject arrowObj = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform); // 화살 생성
        
        if(0 < dirVec.x) // 플레이어가 오른쪽을 바라보고 있으면
        {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, -135);
            arrowObj.transform.position += Vector3.right;
        }
        else if(dirVec.x < 0) // 플레이어가 왼쪽을 바라보고 있으면
        {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, 45);
            arrowObj.transform.position += Vector3.left;
        }
        else if(0 < dirVec.y) // 플레이어가 위쪽을 바라보고 있으면
        {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, -45);
            arrowObj.transform.position += Vector3.up;
        }
        else if(dirVec.y < 0) // 플레이어가 아래쪽을 바라보고 있으면
        {
            arrowObj.transform.rotation *= Quaternion.Euler(0, 0, 135);
            arrowObj.transform.position += Vector3.down;
        }

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrow.Shot(dirVec); // 화살 발사
        Inventory.instance.arrowCnt--;
        UpdateArrowCount();
        
        timer = 0; // 타이머 초기화 해주기
    }

    private void UpdateArrowCount() // 화살의 갯수를 줄여주는 해주는 메소드
    {
        int num = -1;
        
        for(int i = 0; i < Inventory.instance.possessItems.Count; i++)
        {
            if(Inventory.instance.possessItems[i].itemName == ItemName.화살)
                num = i;
        }

        if(num != -1)
            Inventory.instance.RemoveItem(num);
    }
    
}