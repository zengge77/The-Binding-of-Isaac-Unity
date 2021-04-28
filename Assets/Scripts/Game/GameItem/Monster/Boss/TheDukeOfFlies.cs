using System;
using System.Collections.Generic;
using UnityEngine;

public class TheDukeOfFlies : Boss
{
    public Monster fly;

    float attackCD = 5f;
    float timing = 0;
    Vector2 movementDirection;

    protected override void InitializeCustomField() { }
    protected override void InitializeBehaviorTree()
    {
        behaviorTree.SetVariableValue("movementSpeed", movementSpeed);
    }


    public void Attack()
    {
        animator.Play("Attack");
    }

    private void SummonFlies()
    {
        int count = UnityEngine.Random.Range(3, 5);
        for (int i = 0; i < count; i++)
        {
            level.manager.GenerateGameObjectInCurrentRoom(fly.gameObject, transform.position + new Vector3(0, -0.1f, 0));
        }
    }
}
