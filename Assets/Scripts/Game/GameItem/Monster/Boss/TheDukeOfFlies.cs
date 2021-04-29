using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDukeOfFlies : Boss
{
    public Monster fly;

    protected override void InitializeCustomField() { }
    protected override void InitializeBehaviorTree()
    {
        behaviorTree.SetVariableValue("movementSpeed", movementSpeed);
    }
    protected override void InitializeSkills()
    {
        skills.Add(SummonFlies);
    }

    public IEnumerator SummonFlies()
    {
        animator.Play("Attack");
        animator.Update(0);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) { yield return null; }
    }
    private void AnimationEventSummonFlies()
    {
        int count = UnityEngine.Random.Range(3, 5);
        for (int i = 0; i < count; i++)
        {
            level.manager.GenerateGameObjectInCurrentRoom(fly.gameObject, transform.position + new Vector3(0, -0.1f, 0));
        }
    }
}