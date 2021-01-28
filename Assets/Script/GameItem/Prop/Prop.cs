using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prop : GameItem
{
    public override GameItemType GameItemType { get { return GameItemType.Prop; } }

    protected UIManager UI;
    protected Pools pools;

    protected bool isCanBeTriggerWithPlayer = false;
    protected float triggerTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        //现使用预制体变体的方法创建新道具，使变体拥有预本体的特性，暂时不用代码同一特性，看后续需求修改
        //同一类变体出自同一本体，多个本体之间不相干
        //注意：不要轻易修改本体，绝对不要从变体覆盖至变体
        //gameObject.tag = "Prop";
        //gameObject.layer = 14;
        //GetComponent<SpriteRenderer>().sortingOrder = GameDate.RENDERERORDER_PROP;
        UI = UIManager.Instance;
        pools = level.pools;
        Invoke("TriggerWithPlayer", triggerTime);
    }

    void TriggerWithPlayer()
    {
        isCanBeTriggerWithPlayer = true;
    }

    protected abstract bool IsTrigger();

    protected abstract void Effect();

    protected abstract void After();
}
