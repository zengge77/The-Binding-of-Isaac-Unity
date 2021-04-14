using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : GameItem, IAttackable
{
    public override GameItemType gameItemType { get { return GameItemType.Monster; } }

    //需要初始化的数据
    [HideInInspector]
    public float MaxHP;
    [HideInInspector]
    public float HP;
    protected float ativateTime;
    protected bool hasCollisionDamage;
    protected int collisionDamage;
    protected float collisionFroce;
    protected float beKnockBackLength;
    protected float beKnockBackSeconds;

    protected bool isActivate = false;
    protected bool isLive = true;
    protected bool isCanMove = true;

    protected Animator animator;
    protected Rigidbody2D myRigidbody;
    protected SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Initialize();
    }

    protected abstract void Initialize();

    protected virtual void Start()
    {
        Invoke("DelayActivate", ativateTime);
    }

    void DelayActivate()
    {
        isActivate = true;
    }

    protected virtual void Update()
    {
        if (!isActivate || !isLive) { return; }
        Attack();
        Moving();
    }

    protected abstract void Attack();

    protected abstract void Moving();

    public virtual void BeAttacked(float damage, Vector2 direction, float forceMultiple = 1f)
    {
        if (!isLive) { return; }
        HP -= damage;
        if (HP <= 0) { Death(); }
        else { StartCoroutine(knockBackCoroutine(direction * forceMultiple)); }
    }

    protected IEnumerator knockBackCoroutine(Vector2 force)
    {
        float timeleft = beKnockBackSeconds;
        while (timeleft > 0)
        {
            transform.Translate(force * beKnockBackLength * Time.deltaTime / beKnockBackSeconds);
            timeleft -= Time.deltaTime;
            yield return null;
        }
    }

    protected virtual void Death()
    {
        isLive = false;
        transform.GetComponent<CircleCollider2D>().enabled = false;
        myRigidbody.velocity = Vector2.zero;
        animator.Play("Death");
    }

    protected virtual void Destroy()
    {
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
