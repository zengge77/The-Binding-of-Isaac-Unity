using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDukeOfFlies : Boss
{
    public AttackFly attackFly;
    private List<AttackFly> flyList = new List<AttackFly>();

    protected override void InitializeCustomField() { }
    protected override void InitializeBehaviorTree()
    {
        behaviorTree.SetVariableValue("self", gameObject);
        behaviorTree.SetVariableValue("movementSpeed", movementSpeed);
    }
    protected override void InitializeSkills()
    {
        skills.Add(SummonFlies);
        skills.Add(SunmmonBigFly);
        skills.Add(CheckFliesCount);
        skills.Add(EnableFliesTrack);
    }

    private IEnumerator SummonFlies()
    {
        yield return PlayAnimationAndEvent("Attack", 0.8f, AnimationEventSummonFlies);
    }
    private void AnimationEventSummonFlies()
    {
        int count = UnityEngine.Random.Range(2, 5);
        Vector3 SummonDistance = new Vector3(0, -0.2f, 0);
        for (int i = 0; i < count; i++)
        {
            //扇形分布
            Vector3 RotateAxis = i % 2 == 0 ? Vector3.forward : -Vector3.forward;
            float angle = 45 * ((i / 2) + 1);
            Vector3 SummonPosition = Quaternion.AngleAxis(angle, RotateAxis) * SummonDistance + transform.position;
            AttackFly fly = level.manager.GenerateGameObjectInCurrentRoom(attackFly, SummonPosition);
            flyList.Add(fly);
        }
    }

    private IEnumerator SunmmonBigFly()
    {
        yield return PlayAnimationAndEvent("Attack", 0.8f, AnimationEventBigFly);
    }
    private void AnimationEventBigFly()
    {
        Vector3 SummonDistance = new Vector3(0, -0.2f, 0) + transform.position;
        AttackFly fly = level.manager.GenerateGameObjectInCurrentRoom(attackFly, SummonDistance);
        fly.SetMaxHP(10);
        fly.transform.localScale *= 1.2f;
        flyList.Add(fly);
    }

    private IEnumerator CheckFliesCount()
    {
        flyList.RemoveAll(fly => fly == null);
        if (flyList.Count > 8)
        {
            yield return EnableFliesTrack();
        }
    }

    private IEnumerator EnableFliesTrack()
    {
        //flyList.RemoveAll(fly => fly == null);
        //for (int i = 0; i < flyList.Count; i++)
        //{
        //    flyList[i].enableTrack = true;
        //}
        yield return null;
    }
}