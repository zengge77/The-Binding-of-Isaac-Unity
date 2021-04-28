using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    /// <summary>
    /// 自定义移动基类，定义通用抽象方法
    /// </summary>
    public abstract class Movement : Action
    {
        /// <summary>
        /// 开始移动
        /// </summary>
        protected abstract void BeginMovement();

        /// <summary>
        /// 暂停移动
        /// </summary>
        protected abstract void StopMovement();

        /// <summary>
        /// 设置终点
        /// </summary>
        /// <param name="destination"></param>
        protected abstract void SetDestination(Vector2 destination);

        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="destination"></param>
        protected abstract void SetSpeed(float speed);

        /// <summary>
        /// 是否已到达终点
        /// </summary>
        protected abstract bool HasArrived();
    }
}


