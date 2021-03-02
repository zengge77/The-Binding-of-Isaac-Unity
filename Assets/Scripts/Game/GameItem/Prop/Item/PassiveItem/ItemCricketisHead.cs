using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCricketisHead : PassiveItem
{
    protected override void SetID()
    {
        ID = 4;
    }

    protected override void Effect()
    {
        //角色的眼泪尺寸增大效果未实装
        player.Knockback += 0.1f;
        player.DamageBase += 0.5f;
        //多个 克里吉特的头,魔法菇,殉道者之血,彼列之书的组合不叠加
        if (!player.IsThisItemExist(ID, ItemMagicMushroom.ID))
        {
            player.DamageMultiple *= 1.5f;
        }
    }
}
