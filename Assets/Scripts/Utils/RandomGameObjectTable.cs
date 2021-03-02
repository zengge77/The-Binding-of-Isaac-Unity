using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameObjectTable : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public List<int> probability;

    /// <summary>
    /// 根据概率获取物品，该算法简单但有效
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomObject(int revised = 0)
    {
        if (gameObjects.Count != probability.Count)
        { Debug.Log("物体和概率数组长度不一致"); return null; }

        int num = Random.Range(1, 101);
        int total = 0;
        for (int i = 0; i < probability.Count; i++)
        {
            total += probability[i];
            total += revised;
            if (total > num)
            {
                return gameObjects[i];
            }
        }
        return null;
    }
}
