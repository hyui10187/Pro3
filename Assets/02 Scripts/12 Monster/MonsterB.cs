using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB : Monster
{
    private void Awake()
    {
        jsonIndex = 1;
    }

    protected override void SpawnItem()
    {
        ItemManager.instance.DropGold(transform.position);
    }
}
