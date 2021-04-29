using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskDescription("获取附近随机地点，不考虑碰撞")]
    [TaskCategory("动作")]
    public class GetRandomDestination : DirectionModify
    {
        //随机点模长为1，再乘以随机倍数
        public float minMultiple;
        public float maxMultiple;

        //最后赋值的随机点
        public SharedVector2 destination;

        //继承自DirectionModify类，使随机点与该方向夹角>90度，避免卡墙
        public override Vector2 ModificationDirection { get; set; } = Vector2.zero;

        public override void OnStart()
        {
            Vector2 point = Random.insideUnitCircle;
            if (ModificationDirection != Vector2.zero)
            {
                //点积，1为通向，0为垂直，-1为相反，这里取-0.1，使得最终随机点与修饰方向>90度
                while (Vector2.Dot(point, ModificationDirection) >= -0.1)
                {
                    point = Random.insideUnitCircle;
                }
            }
            float multiple = Random.Range(minMultiple, maxMultiple);
            destination.Value = (Vector2)transform.position + point * multiple;
        }

        public override void OnEnd()
        {
            ModificationDirection = Vector2.zero;
        }
    }
}
