using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomGameObjectTable))]
public class Obstacles : GameItem
{
    public override GameItemType gameItemType { get { return GameItemType.Obstacles; } }

    protected GenerateGameObject generateGameObject;

    protected override void Awake()
    {
        base.Awake();
        generateGameObject = level.generateGameObject;
    }

    protected virtual GameObject GenerateReward()
    {
        GameObject randomObject = GetComponent<RandomGameObjectTable>().GetRandomObject();
        if (randomObject == null) { return null; }

        return generateGameObject.GeneratePropInCurrentRoom(randomObject, transform.position);
    }
}
