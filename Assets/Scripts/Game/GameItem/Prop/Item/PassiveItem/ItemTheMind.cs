using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTheMind : PassiveItem
{
    protected override void SetID()
    {
        ID = 333;
    }

    protected override void Effect()
    {
        UI.miniMap.ShowAllMinMap();
    }

}
