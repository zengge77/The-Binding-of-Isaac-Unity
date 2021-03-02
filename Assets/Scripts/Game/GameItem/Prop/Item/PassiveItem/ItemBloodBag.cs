using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBloodBag : PassiveItem
{
    protected override void SetID()
    {
        ID = 119;
    }
    protected override void Effect()
    {
        player.AddHealth(10, Player.HealthType.Normal, 2);
        player.Speed += 0.3f;
    }
}
