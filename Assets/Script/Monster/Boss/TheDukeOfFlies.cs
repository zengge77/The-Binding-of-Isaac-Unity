using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDukeOfFlies : Boss
{
    public Monster fly;

    float movementSpeed = 1.2f;
    Vector2 movementDirection;

    float attackCD = 5f;
    float timing = 0;

    protected override void Initialize()
    {
        HP = MaxHP = 110;
        ativateTime = 1f;
        hasCollisionDamage = true;
        collisionDamage = 1;
        collisionFroce = 1;
        beKnockBackLength = 0;
        beKnockBackSeconds = 0;
    }

    protected override void Start()
    {
        base.Start();
        movementDirection = UnityEngine.Random.insideUnitCircle.normalized;
    }

    protected override void Attack()
    {
        timing += Time.deltaTime;
        if (timing >= attackCD)
        {
            animator.Play("Attack");
            timing = 0;
        }
    }

    private void SummonFlies()
    {
        int count = UnityEngine.Random.Range(3, 5);
        for (int i = 0; i < count; i++)
        {
            Instantiate(fly, transform.position + new Vector3(0, -0.1f, 0), Quaternion.identity, transform.parent);
        }
    }

    protected override void Moving()
    {
        transform.Translate(movementDirection * Time.deltaTime * movementSpeed);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Tool.IsItOfThisType(new List<Type>() { typeof(Obstacles) }, collision.gameObject) || Tool.IsItOfThisType(new List<String>() { "Walls" }, collision.gameObject))
        {
            float x = collision.contacts[0].normal.x;

            //碰撞点为左右
            if (Mathf.Abs(x) > 0.9f)
            {
                movementDirection = new Vector2(-movementDirection.x, movementDirection.y);
            }
            //上下
            else if (Mathf.Abs(x) < 0.1f)
            {
                movementDirection = new Vector2(movementDirection.x, -movementDirection.y);

            }
            //其他
            else
            {
                movementDirection = -movementDirection;
            }
        }
    }
}
