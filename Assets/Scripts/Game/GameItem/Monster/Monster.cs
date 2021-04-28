using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviorDesigner.Runtime;

public abstract class Monster : GameItem, IAttackable
{
    public override GameItemType gameItemType { get { return GameItemType.Monster; } }

    //组件
    protected Rigidbody2D rigid;
    protected Collider2D collid;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected AIPath ai;
    protected BehaviorTree behaviorTree;

    //自身字段
    //公开编辑字段，方便编辑
    [SerializeField]
    protected float maxHP;
    protected float HP;
    [SerializeField]
    protected float movementSpeed;
    [SerializeField]
    protected bool hasCollisionDamage;
    //每个怪物大类的统一字段
    protected float activateSeconds;
    protected int collisionDamage;
    protected float collisionFroce;
    protected float knockBackDistance;
    protected float knockBackSeconds;
    //其他字段
    protected bool isLive = true;

    //抽象方法，当子类需要更改字段时在InitalizeField方法里修改
    protected abstract void InitializeCateGoryField();
    protected abstract void InitializeCustomField();
    protected abstract void InitializeBehaviorTree();

    //初始化内容
    protected override void Awake()
    {
        base.Awake();

        rigid = GetComponent<Rigidbody2D>();
        collid = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ai = GetComponent<AIPath>();
        behaviorTree = GetComponent<BehaviorTree>();

        InitializeCateGoryField();
        InitializeCustomField();
        HP = maxHP;
        InitializeBehaviorTree();
    }

    //启动行为树
    protected virtual void Start()
    {
        behaviorTree?.DisableBehavior();
        StartCoroutine(EnableBehaviorTree(activateSeconds));
    }
    private IEnumerator EnableBehaviorTree(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        behaviorTree?.EnableBehavior();
    }

    /// <summary>
    /// 被攻击
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="direction"></param>
    /// <param name="forceMultiple"></param>
    public virtual void BeAttacked(float damage, Vector2 direction, float forceMultiple = 1f)
    {
        if (!isLive) { return; }
        HP = Mathf.Clamp(HP - damage, 0, maxHP);
        if (HP <= 0) { StartCoroutine(Death()); }
        else if (knockBackDistance > 0)
        { StartCoroutine(knockBackCoroutine(direction * forceMultiple)); }
    }

    /// <summary>
    /// 被击退
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    protected virtual IEnumerator knockBackCoroutine(Vector2 force)
    {
        float timeleft = knockBackSeconds;
        while (timeleft > 0)
        {
            transform.Translate(force * knockBackDistance * Time.deltaTime / knockBackSeconds);
            timeleft -= Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    protected virtual IEnumerator Death()
    {
        isLive = false;
        if (ai != null) { ai.isStopped = true; }
        behaviorTree?.DisableBehavior();
        collid.enabled = false;
        rigid.velocity = Vector2.zero;
        animator.Play("Death");
        yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) { yield return null; }
        Destroy(gameObject);
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollisionDamage && collision.gameObject.GetComponent<Player>())
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector2 froce = (collision.transform.position - contactPoint).normalized;
            player.BeAttacked(collisionDamage, froce, collisionFroce);
        }
    }
}
