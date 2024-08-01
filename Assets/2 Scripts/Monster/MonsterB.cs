using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB : Monster
{
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        maxHealth = 150;
        CurHealth = MaxHealth;
        exp = 30;
        collisionDamage = 15;
    }

    protected override void SpawnItem()
    {
        ItemManager.instance.DropGold(transform.position);
    }
}
