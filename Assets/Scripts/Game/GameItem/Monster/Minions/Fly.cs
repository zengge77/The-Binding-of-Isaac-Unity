using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Minion
{
    protected override void InitializeCustomField() { }
    protected override void InitializeBehaviorTree()
    {
        if (behaviorTree != null)
        {
            behaviorTree.SetVariableValue("player", player.gameObject);
            behaviorTree.SetVariableValue("movementSpeed", movementSpeed);
        }
    }
    protected override void InitializeSkills() { }
}
