using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elite : Monster
{
    //精英类统一变量，子类视需要可重写
    protected override void InitializeCateGoryField()
    {
        activateSeconds = 1f;
        collisionDamage = 1;
        collisionFroce = 1f;
        knockBackDistance = 0.1f;
        knockBackSeconds = 0.1f;
    }
}
