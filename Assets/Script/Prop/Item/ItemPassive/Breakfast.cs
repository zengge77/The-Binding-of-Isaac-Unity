using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakfast : PassiveItem
{
    protected override void SetID()
    {
        ID = 25;
    }

    protected override void Effect()
    {
        player.AddHealth(2, Player.HealthType.Normal, 2);
    }
}
