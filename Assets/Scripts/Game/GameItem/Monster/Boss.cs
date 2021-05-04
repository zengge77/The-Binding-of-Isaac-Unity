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
    protected override void OnDeath()
    {
        hpSlider.gameObject.SetActive(false);
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
}
