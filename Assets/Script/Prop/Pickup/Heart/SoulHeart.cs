using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHeart : Heart
{
    public int Hp;

    protected override bool IsTrigger()
    {
        return true;
    }
    protected override void Effect()
    {
        player.AddHealth(Hp, Player.HealthType.Soul);
    }
}
