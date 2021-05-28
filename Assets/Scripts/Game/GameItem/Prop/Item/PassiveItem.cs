using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : Item
{
    protected override bool IsTrigger()
    {
        return true;
    }

    protected override void After()
    {
        player.itemModle.AddItem(this);
        Destroy(gameObject);
    }
}
