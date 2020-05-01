using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Prop
{
    public int ID { get; set; }
    public ItemInformation itemInformation;
    public Sprite Sprite { get; set; }

    private void Start()
    {
        SetID();
        itemInformation = ItemDateFromJson.GetItemInformation(ID);
        itemInformation.Sprite = GetComponent<SpriteRenderer>().sprite;
    }

    protected abstract void SetID();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && IsTrigger() && isCanBeTriggerWithPlayer)
        {
            Effect();
            UI.attributes.UpDateAttributes();
            After();
        }
    }
}
