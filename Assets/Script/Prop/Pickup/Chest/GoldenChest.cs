using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenChest : Chest
{
    protected override bool IsTrigger()
    {
        if (player.KeyNum > 0 && !isOpened)
        {
            player.KeyNum--;
            return true;
        }
        return false;
    }
}
