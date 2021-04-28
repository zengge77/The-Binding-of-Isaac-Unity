using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    public abstract class DirectionModify : Action
    {
        public abstract Vector2 ModificationDirection { get; set; }
    }
}
