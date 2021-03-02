using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{

    /// <summary>
    /// 受击功能
    /// </summary>
    /// <param name="damage">伤害</param>
    /// <param name="direction">受力方向(需要归一化) </param>
    /// /// <param name="forceMultiple">力的大小倍数</param>
    void BeAttacked(float damage, Vector2 direction, float forceMultiple = 1f);

    #region 参考实现
    //if (!isLive) { return; }
    //HP -= damage;
    //if (HP <= 0) { Death(); }
    //else { StartCoroutine(knockBackCoroutine(direction * forceMultiple)); }
    #endregion

    #region 参考调用
    //Vector3 contactPoint = collision.contacts[0].point;
    //Vector2 froce = (collision.transform.position - contactPoint).normalized;
    //player.BeAttacked(collisionDamage, froce, collisionFroce); 
    #endregion

    #region 参考击退效果
    //protected IEnumerator knockBackCoroutine(Vector2 force)
    //{
    //    float timeleft = beKnockBackSeconds;
    //    while (timeleft > 0)
    //    {
    //        transform.Translate(force * beKnockBackLength * Time.deltaTime / beKnockBackSeconds);
    //        timeleft -= Time.deltaTime;
    //        yield return null;
    //    }
    //}
    #endregion
}
