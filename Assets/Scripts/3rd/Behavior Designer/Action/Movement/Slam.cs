using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskDescription("往特定方向冲撞并反弹,不使用寻路")]
    [TaskCategory("移动")]
    public class Slam : NormalMovement
    {
        public SharedFloat movementSpeed;
        public SharedVector2 direction;
        private Vector2 oldDirection;//方向缓存，具体看OnCollisionEnter2D内解释

        public override void OnStart()
        {
            direction.Value = Random.insideUnitCircle.normalized;
            oldDirection = direction.Value;
        }

        public override TaskStatus OnUpdate()
        {
            transform.Translate(direction.Value * movementSpeed.Value * Time.deltaTime);
            oldDirection = direction.Value;
            return TaskStatus.Running;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (CommonUnit.LayerCheck(collision.gameObject, "Wall", "Obstacle"))
            {
                float x = collision.GetContact(0).normal.x;

                /* 当一次性接触到2个或以上的碰撞体时，会多次触发该方法，导致direction多次在原direction上更新。
                 * 这里引入oldDirection，direction更新时不在原direction上改动，而是在oldDirection上。
                 * oldDirection在碰撞结束后的下一帧再随direction跟新。这样即使多次调用也能得到同样的结果。
                 */

                //碰撞点为左右
                if (Mathf.Abs(x) > 0.9f) { direction.Value = new Vector2(-oldDirection.x, oldDirection.y); }
                //上下
                else if (Mathf.Abs(x) < 0.1f) { direction.Value = new Vector2(oldDirection.x, -oldDirection.y); }
                //其他
                else { direction.Value = -oldDirection; }

            }
        }
    }
}