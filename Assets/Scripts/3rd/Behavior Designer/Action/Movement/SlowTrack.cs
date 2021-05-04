using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    //其实该Task的设计和最初构设有很大差别，但也能完成目标。暂时使用该算法，待完成Grid内智能取点功能再时需求重写
    [TaskDescription("追踪目标但是小幅度随机移动,使用寻路")]
    [TaskCategory("移动")]
    public class SlowTrack : GridGraphMovement
    {
        //主目标
        public SharedGameObject target;
        public SharedFloat movementSpeed;

        //随机目标，每次寻路计算完成后在路径上偏移计算获得
        private Vector2 currentTarget;
        [Tooltip("截取寻路路径的片段下标，建议值4")]
        public int pathFragment;
        [Tooltip("在路径片段上再添加的偏移量，建议值0.3")]
        public float offsetRadius;

        //缓存路径
        private List<Vector3> buffer = new List<Vector3>();
        bool stale;

        //委托用
        Path path;

        public override void OnStart()
        {
            BeginMovement();
            SetSpeed(movementSpeed.Value);

            currentTarget = target.Value.transform.position;
            SetDestination(currentTarget);
            seeker.postProcessPath += SetCurrentTarget;
        }

        /// <summary>
        /// 寻路计算完成时调用，设置随机目标
        /// </summary>
        /// <param name="path"></param>
        public void SetCurrentTarget(Path path)
        {
            ai.GetRemainingPath(buffer, out stale);
            if (buffer.Count >= pathFragment)
            {
                currentTarget = (Vector2)buffer[pathFragment - 1] + Random.insideUnitCircle * offsetRadius;
            }
            else
            {
                currentTarget = target.Value.transform.position;
            }
            SetDestination(currentTarget);
        }

        public override TaskStatus OnUpdate()
        {
            //到达随机目标后，将目标设置回主目标
            //if (((Vector2)transform.position - currentTarget).magnitude <= 0.1f)
            //if (ai.reachedDestination)
            //{
            //    SetDestination(target.Value.transform.position);
            //    ai.SearchPath();
            //    isDone = true;
            //}

            return TaskStatus.Running;
        }
    }
}