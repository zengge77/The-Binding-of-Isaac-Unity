using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskDescription("朝目的地冲锋,不使用寻路")]
    [TaskCategory("移动")]
    public class Charge : NormalMovement
    {
        public float arriveDistance;//视为到达的距离
        public float spentSecond;//花费的时间，因为是插值移动，所以只是估计

        public SharedVector2 destination;

        private SpriteRenderer spriteRenderer;

        //计时机制，暂时不用
        //private float time = 0f;
        //private float timeLimit = 2f;

        //翻转sprite，可以单独提取出来task
        public override void OnAwake()
        {
            base.OnAwake();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public override void OnStart()
        {
            base.OnStart();
            if (destination.Value.x - transform.position.x > 0) { spriteRenderer.flipX = false; }
            else { spriteRenderer.flipX = true; }
        }

        public override TaskStatus OnUpdate()
        {
            //计时机制，暂时不用
            //time += Time.deltaTime;
            //if (time >= timeLimit){ return TaskStatus.Success;}

            //暂时使用插值移动，先快后慢
            if (Vector2.Distance(transform.position, destination.Value) > arriveDistance)
            {
                transform.position = Vector2.Lerp(transform.position, destination.Value, Time.deltaTime / spentSecond);
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }

        public override void OnEnd()
        {
            //time = 0;
        }
    }
}
