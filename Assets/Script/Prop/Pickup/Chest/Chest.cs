using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RandomGameObjectTable))]

public abstract class Chest : Pickup
{
    [Header("打开后的外形")]
    public Sprite shapeAfterOpen;

    [Header("数量")]
    [SerializeField]
    protected int mixNum;
    [SerializeField]
    protected int maxNum;

    private RandomGameObjectTable table;
    protected RandomGameObjectTable Table
    {
        get
        {
            if (table == null)
            {
                table = GetComponent<RandomGameObjectTable>();
            }
            return table;
        }
    }

    protected bool isOpened = false;

    protected override void After()
    {
        GetComponent<SpriteRenderer>().sprite = shapeAfterOpen;
        isOpened = true;
    }

    [ContextMenu("打开")]
    protected override void Effect()
    {
        Transform propContainer = level.currentRoom.PropContainer;
        int num = UnityEngine.Random.Range(mixNum, maxNum + 1);

        for (int i = 0; i < num; i++)
        {
            var newProp = Instantiate(Table.GetRandomObject(), propContainer);
            newProp.transform.position = transform.position;

            Vector2 force = UnityEngine.Random.insideUnitCircle * 7;
            if (newProp.GetComponent<Rigidbody2D>())
            {
                newProp.GetComponent<Rigidbody2D>().AddForce(force);
            }
            else if (newProp.GetComponent<IRandomObject>() != null)
            {
                newProp.GetComponent<IRandomObject>().Generate().GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
    }
}
