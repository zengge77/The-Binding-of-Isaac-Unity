using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskDescription("碰撞方向修饰，发生碰撞时返回True，并为绑定的Task更新修饰方向")]
    [TaskCategory("修饰")]
    public class CollisionDirectionModification : Action
    {
        //要修饰的task，需手动绑定
        public DirectionModify task;

        private bool isCollision = false;

        public override TaskStatus OnUpdate()
        {
            if (isCollision)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Running;
            }
        }

        public override void OnEnd()
        {
            isCollision = false;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            Vector3 contactPoint = collision.contacts[0].point;
            //注意先归一化
            Vector2 froce = (contactPoint - transform.position).normalized;
            task.ModificationDirection = froce;
            isCollision = true;
        }
    }
}
