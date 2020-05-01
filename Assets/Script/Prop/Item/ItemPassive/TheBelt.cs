using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBelt : PassiveItem
{
    protected override void SetID()
    {
        ID = 28;
    }

    protected override void Effect()
    {
        player.Speed += 0.3f;
    }
}
