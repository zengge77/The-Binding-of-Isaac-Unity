using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomGameObjectTable))]
public class Obstacles : GameItem
{
    public override GameItemType gameItemType { get { return GameItemType.Obstacles; } }

    protected GameItemManager manager;

    protected override void Awake()
    {
        base.Awake();
        manager = level.manager;
    }

    protected virtual GameObject GenerateReward()
    {
        GameObject randomObject = GetComponent<RandomGameObjectTable>().GetGameObject();
        if (randomObject == null) { return null; }

        return manager.GenerateGameObjectInCurrentRoom(randomObject, transform.position);
        
    }
}
