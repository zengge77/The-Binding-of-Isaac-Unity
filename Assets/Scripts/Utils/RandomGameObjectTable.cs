using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameObjectTable : MonoBehaviour
{
    public List<SimplePairWithGameObjectInt> table;

    /// <summary>
    /// 根据概率获取物品，该算法简单但有效
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject(int revised = 0)
    {
        int num = Random.Range(1, 101);
        int total = 0;
        for (int i = 0; i < table.Count; i++)
        {
            total += table[i].value2;
            total += revised;
            if (total > num)
            {
                return table[i].value1;
            }
        }
        return null;
    }
}
