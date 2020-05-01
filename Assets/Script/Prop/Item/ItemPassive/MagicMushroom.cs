using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroom : PassiveItem
{
    protected override void SetID()
    {
        ID = 12;
    }

    protected override void Effect()
    {
        player.AddHealth(player.MaxHealth, Player.HealthType.Normal, 2);
        player.transform.localScale *= 1.05f;
        player.Range += 5.25f;
        //上抛速度+0.5f;
        player.Speed += 0.3f;
        player.DamageBase += 0.3f;

        //多个 克里吉特的头,魔法菇,殉道者之血,彼列之书的组合不叠加
        //不叠加，是取最大值吗？多个自己叠加吗?
        player.DamageMultiple *= 1.5f;

    }
}
