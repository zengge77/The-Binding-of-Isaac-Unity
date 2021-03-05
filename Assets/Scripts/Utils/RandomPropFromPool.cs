using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPropFromPool : MonoBehaviour, IRandomGameObject
{
    public PropPoolType type;

    void Start()
    {
        Generate();
    }

    public GameObject Generate()
    {
        Level level = GameManager.Instance.level;
        Prop prop = level.pools.GetProp(type);
        GameObject go = level.generate.GenerateGameObjectInCurrentRoom(prop.gameObject, transform.position);
        Destroy(gameObject);
        return go.gameObject;
    }
}
