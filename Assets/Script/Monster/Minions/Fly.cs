using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Monster
{
    protected override void Initialize()
    {
        HP = MaxHP = 3;
        ativateTime = 0.5f;
        hasCollisionDamage = false;
        collisionDamage = 1;
        collisionFroce = 1;
        beKnockBackLength = 0.1f;
        beKnockBackSeconds = 0.1f;
    }

    protected override void Moving()
    {
        float speed = 0.7f;
        float dis = Vector2.Distance(transform.position, player.transform.position);
        transform.position = Vector2.Lerp(transform.position, player.transform.position,  Time.deltaTime * (1 / dis)* speed);
    }

    protected override void Attack()
    {

    }
}
