using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Boss : Monster
{
    protected Slider hpSlider;

    protected override void Awake()
    {
        base.Awake();
        ActiveHPSlider();
    }

    protected virtual void Update()
    {
        UpdateHPSlider();
    }

    private void ActiveHPSlider()
    {
        hpSlider = UIManager.Instance.bossHp;
        hpSlider.gameObject.SetActive(true);
        hpSlider.value = 1;
    }
    private void UpdateHPSlider()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, HP / maxHP, Time.deltaTime * 3);
    }

    //Boss类统一变量，子类视需要可重写
    protected override void InitializeCateGoryField()
    {
        activateSeconds = 1f;
        collisionDamage = 1;
        collisionFroce = 1f;
        knockBackDistance = 0;
        knockBackSeconds = 0;
    }

    protected override IEnumerator Death()
    {
        hpSlider.gameObject.SetActive(false);

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
}
