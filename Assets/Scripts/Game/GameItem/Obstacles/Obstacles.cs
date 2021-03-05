using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomGameObjectTable))]
public class Obstacles : GameItem
{
    public override GameItemType gameItemType { get { return GameItemType.Obstacles; } }

    protected Generate generate;

    protected override void Awake()
    {
        base.Awake();
        generate = level.generate;
    }

    protected virtual GameObject GenerateReward()
    {
        GameObject randomObject = GetComponent<RandomGameObjectTable>().GetRandomObject();
        if (randomObject == null) { return null; }

        return generate.GenerateGameObjectInCurrentRoom(randomObject, transform.position);
        
    }
}
