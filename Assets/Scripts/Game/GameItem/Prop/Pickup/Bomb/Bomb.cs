using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Pickup
{
    public int value;

    protected override bool IsTrigger()
    {
        return true;
    }

    protected override void Effect()
    {
        player.BombNum += value;
    }

    protected override void After()
    {
        Destroy(gameObject);
    }
}
