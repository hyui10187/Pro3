using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantStoneSlot : EnchantSlot
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        slotNum = -1;
        item = new Item();
        item.itemCount = 0;
    }
    
    public void OnClickSlot() // 슬롯을 클릭했을때 호출할 메소드
    {
        itemImage = null; // 아이템 이미지 비워주기
        itemCount.gameObject.SetActive(false); // 강화 수치 글자 꺼주기
    }
    
}