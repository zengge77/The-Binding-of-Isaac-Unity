using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragonSlayerSword : PassiveItem
{
    protected override void SetID()
    {
        ID = 0;
    }

    protected override void Effect()
    {
        player.DamageBase += 5;
        int value = player.SoulHealth + player.Health - 1;
        player.ReduceHealth(value);
    }

}
