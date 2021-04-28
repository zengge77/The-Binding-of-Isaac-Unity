using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Custom
{
    [TaskDescription("基础攻击")]
    [TaskCategory("攻击")]
    public class BaseAttack : Action
    {
        private TheDukeOfFlies monster;

        public override void OnAwake()
        {
            base.OnAwake();
            monster = GetComponent<TheDukeOfFlies>();
        }

        public override void OnStart()
        {
            base.OnStart();
            monster.Attack();
        }
    }
}
