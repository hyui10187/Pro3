using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Monster
{
    
    private void Awake()
    {
        jsonIndex = 2;
    }

    protected override void SpawnItem()
    {
        ItemManager.instance.DropGold(transform.position);
    }
    
}