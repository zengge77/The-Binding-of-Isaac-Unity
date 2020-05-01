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
        hpSlider = UIManager.Instance.bossHp;
        hpSlider.gameObject.SetActive(true);
        hpSlider.value = 1;
    }

    protected override void Update()
    {
        base.Update();
        UpdateHPSlider();
    }

    protected void UpdateHPSlider()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, HP / MaxHP, Time.deltaTime * 3);
    }

    protected override void Death()
    {
        base.Death();
        hpSlider.gameObject.SetActive(false);
    }

    protected abstract override void Initialize();
    protected abstract override void Attack();
    protected abstract override void Moving();

}
