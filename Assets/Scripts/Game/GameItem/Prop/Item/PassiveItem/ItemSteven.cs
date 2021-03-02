using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSteven : PassiveItem
{
    protected override void SetID()
    {
        ID = 50;
    }

    protected override void Effect()
    {
        player.DamageBase += 1;
    }
}
