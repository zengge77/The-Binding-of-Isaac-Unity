using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskDescription("追踪目标移动,使用寻路")]
    [TaskCategory("移动")]
    public class Track : GridGraphMovement
    {
        public SharedGameObject target;
        public SharedFloat speed;

        public override void OnStart()
        {
            BeginMovement();
            SetSpeed(speed.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
            {
                return TaskStatus.Failure;
            }
            else
            {
                SetDestination(target.Value.transform.position);
                return TaskStatus.Running;
            }
        }
    }
}
