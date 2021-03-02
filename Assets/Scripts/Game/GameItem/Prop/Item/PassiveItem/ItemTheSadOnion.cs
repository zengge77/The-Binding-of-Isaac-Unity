using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTheSadOnion : PassiveItem
{
    protected override void SetID()
    {
        ID = 1;
    }

    protected override void Effect()
    {
        player.Tears += 0.7f;
    }
}
