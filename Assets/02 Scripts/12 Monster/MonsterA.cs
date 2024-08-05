using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA : Monster
{
    private void Awake()
    {
        jsonIndex = 0;
    }

    protected override void Delete()
    {
        base.Delete();
        SpawnManager.instance.enemyCount--; // 퀘스트 몬스터는 죽을때마다 몬스터의 숫자를 하나씩 빼주기
    }
    
    protected override void SpawnItem()
    {
        ItemManager.instance.DropGold(transform.position);
    }
    
}