using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickup
{
    public int value;

    protected override bool IsTrigger()
    {
        return true;
    }

    protected override void After()
    {
        Destroy(gameObject);
    }

    protected override void Effect()
    {
        player.KeyNum += value;
    }
}
