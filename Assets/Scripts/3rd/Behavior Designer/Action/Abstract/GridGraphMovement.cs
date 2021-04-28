using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    /// <summary>
    /// 使用A星寻路GridGraph的移动基类
    /// </summary>
    public abstract class GridGraphMovement : Movement
    {
        protected AIPath ai;

        public override void OnAwake()
        {
            ai = GetComponent<AIPath>();
        }

        protected override void BeginMovement()
        {
            ai.isStopped = false;
        }

        protected override void StopMovement()
        {
            ai.isStopped = true;
        }

        protected override void SetDestination(Vector2 destination)
        {
            ai.destination = destination;
        }

        protected override void SetSpeed(float speed)
        {
            ai.maxSpeed = speed;
        }

        protected override bool HasArrived()
        {
            return ai.reachedDestination;
        }
    }
}
