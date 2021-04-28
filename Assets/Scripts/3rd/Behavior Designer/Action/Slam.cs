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
        
        public override void OnStart()
        {
            direction.Value = Random.insideUnitCircle.normalized;
        }

        public override TaskStatus OnUpdate()
        {
            transform.Translate(direction.Value * movementSpeed.Value * Time.deltaTime);
            return TaskStatus.Running;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (CommonUnit.LayerCheck(collision.gameObject, "Wall", "Obstacle"))
            {
                float x = collision.contacts[0].normal.x;

                //碰撞点为左右
                if (Mathf.Abs(x) > 0.9f)
                {
                    direction.Value = new Vector2(-direction.Value.x, direction.Value.y);
                }
                //上下
                else if (Mathf.Abs(x) < 0.1f)
                {
                    direction.Value = new Vector2(direction.Value.x, -direction.Value.y);
                }
                //其他
                else
                {
                    direction.Value = -direction.Value;
                }
            }
        }
    }
}