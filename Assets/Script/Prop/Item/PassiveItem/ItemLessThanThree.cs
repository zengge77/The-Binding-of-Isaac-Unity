using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLessThanThree : PassiveItem
{
    protected override void SetID()
    {
        ID = 15;
    }
    protected override void Effect()
    {
        player.AddHealth(player.MaxHealth, Player.HealthType.Normal, 2);
    }
}
