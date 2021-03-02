using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dip : Monster
{

    Vector2 destination;

    protected override void Initialize()
    {
        HP = MaxHP = 3; //3 + 层数 * 1
        ativateTime = 1f;
        hasCollisionDamage = true;
        collisionDamage = 1;
        collisionFroce = 1;
        beKnockBackLength = 0.2f;
        beKnockBackSeconds = 0.1f;
    }

    protected override void Start()
    {
        base.Start();
        SelectNewDestination();
    }

    protected override void Moving()
    {
        if (Vector2.Distance(transform.position, destination) > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime / 2);
        }
        else
        {
            SelectNewDestination();
            if (destination.x > 0) { spriteRenderer.flipX = false; }
            else { spriteRenderer.flipX = true; }
        }
    }

    void SelectNewDestination()
    {
        float multiple = UnityEngine.Random.Range(2, 5);
        Vector2 point = UnityEngine.Random.insideUnitCircle * multiple;
        destination = (Vector2)transform.position - point;
    }

    protected override void Attack() { }

    private void OnCollisionStay2D(Collision2D collision)
    {
        SelectNewDestination();
    }
}
