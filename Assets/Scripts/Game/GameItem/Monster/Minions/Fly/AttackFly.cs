using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFly : Fly
{
    protected override void InitializeCustomField() { }
    protected override void InitializeBehaviorTree()
    {
        behaviorTree.SetVariableValue("self", gameObject);
        behaviorTree.SetVariableValue("player", player.gameObject);
        behaviorTree.SetVariableValue("movementSpeed", movementSpeed);
    }
    protected override void InitializeSkills() { }

    public bool enableTrack = false;
}
