using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskDescription("使用技能")]
    [TaskCategory("攻击")]
    public class UseSkill : Action
    {
        public int skillIndex;

        private Monster monster;
        private SkillStateInfo info;

        public override void OnAwake()
        {
            base.OnAwake();
            monster = GetComponent<Monster>();
        }

        public override void OnStart()
        {
            base.OnStart();
            info = new SkillStateInfo();
            monster.UseSkill(skillIndex, info);
        }

        public override TaskStatus OnUpdate()
        {
            if (info.isDone) { return TaskStatus.Success; }
            else { return TaskStatus.Running; }
        }
    }
}
