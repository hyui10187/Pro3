using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public int index; // 아이템의 인덱스
    public int itemCount; // 보유한 아이템의 갯수
    public int slotNum; // 아이템이 위치한 슬롯번호
    public bool isEquipped; // 아이템을 장착중인지 여부
}