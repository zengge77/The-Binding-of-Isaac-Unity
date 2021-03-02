using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoidRage : PassiveItem
{
    protected override void SetID()
    {
        ID = 14;
    }
    protected override void Effect()
    {
        if (!player.IsThisItemExist(ID)) { player.Speed += 0.6f; }
        player.Range += 5.25f;
        //上抛速度+0.50
    }
}
