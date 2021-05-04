using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    //实现难度比想象中大，现在这个算法是每帧指定position，不受碰撞影响；理想算法是做成每帧增量，受碰撞影响
    [TaskDescription("未完成！！！环绕目标移动,不使用寻路")]
    [TaskCategory("未完成")]
    public class Surround : NormalMovement
    {
        public SharedGameObject target;
        public SharedFloat movementSpeed;

        private Transform aroundPoint;//围绕的物体
        public float angularSpeed;//角速度
        public float aroundRadius;//半径
        private float angled;

        public override void OnStart()
        {
            base.OnStart();
            aroundPoint = target.Value.transform;

            //设置物体初始位置为围绕物体的正上方距离为半径的点
            transform.position = (Vector2)aroundPoint.position + Vector2.up * aroundRadius;
        }

        public override TaskStatus OnUpdate()
        {
            angled += (angularSpeed * Time.deltaTime) % 360;//累加已经转过的角度
            float posX = aroundRadius * Mathf.Sin(angled * Mathf.Deg2Rad);//计算x位置
            float posY = aroundRadius * Mathf.Cos(angled * Mathf.Deg2Rad);//计算y位置
            transform.position = new Vector2(posX, posY) + (Vector2)aroundPoint.position;//更新位置

            return TaskStatus.Running;

            //float a = Angle_360(transform.position, aroundPoint.position + Vector3.up);
            //Vector2 dis = transform.position - aroundPoint.position;
        }

        public float Angle_360(Vector3 from_, Vector3 to_)
        {
            Vector3 v3 = Vector3.Cross(from_, to_); //叉乘判断正方向

            if (v3.z > 0)
                return Vector3.Angle(from_, to_);
            else
                return 360 - Vector3.Angle(from_, to_);
        }
    }
}
