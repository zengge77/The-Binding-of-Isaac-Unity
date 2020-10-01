using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : Prop
{
    public enum Goodstype { Item, Pickup }
    public Goodstype goodstype;

    private GameObject newProp;
    private int price;

    private void Start()
    {
        GameObject prototypeProp = null;
        //获取道具
        switch (goodstype)
        {
            case Goodstype.Item:
                prototypeProp = pools.GetProp(Pools.PropPoolType.Shop).gameObject;
                price = 15;
                break;
            case Goodstype.Pickup:
                prototypeProp = pools.GetPickupGoods();
                price = 3;
                break;
            default:
                break;
        }

        //生成道具
        Transform propContainer = level.currentRoom.PropContainer;
        newProp = Instantiate(prototypeProp, propContainer);
        newProp.transform.position = transform.position;
        newProp.GetComponent<Collider2D>().enabled = false;
        newProp.GetComponent<SpriteRenderer>().enabled = false;

        //设置价格，UI
        GetComponent<SpriteRenderer>().sprite = newProp.GetComponent<SpriteRenderer>().sprite;
        GetComponentInChildren<TextMesh>().text = price.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && IsTrigger())
        {
            Effect();
            After();
        }
    }

    protected override bool IsTrigger()
    {
        return player.CoinNum >= price;
    }

    protected override void Effect()
    {
        player.CoinNum -= price;
        UI.attributes.UpDateAttributes();
        newProp.GetComponent<Collider2D>().enabled = true;
        newProp.GetComponent<SpriteRenderer>().enabled = true;
    }

    protected override void After()
    {
        Destroy(gameObject);
    }
}
