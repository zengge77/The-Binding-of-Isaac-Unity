using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownChest : Chest
{
    protected override bool IsTrigger()
    {
        return !isOpened;
    }
}
