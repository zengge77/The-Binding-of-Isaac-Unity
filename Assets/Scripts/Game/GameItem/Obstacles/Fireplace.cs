using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : Obstacles, IAttackable, IDestructible
{
    public GameObject Fire;

    int type;
    public List<Sprite> WoodShapes;
    public List<Sprite> ExtinguishWoodShapes;

    int HP = 3;

    private void Start()
    {
        type = UnityEngine.Random.Range(0, WoodShapes.Count);
        SpriteRenderer WoodSpriteRenderer = GetComponent<SpriteRenderer>();
        WoodSpriteRenderer.sprite = WoodShapes[type];
    }

    public void BeAttacked(float damage, Vector2 direction, float forceMultiple = 1f)
    {
        if (HP > 0)
        {
            HP--;
            if (HP == 0)
            {
                DestorySelf();
                return;
            }
            Fire.transform.localScale *= 0.8f;
        }
    }

    public void DestorySelf()
    {
        generate.GenerateTraceInCurrentRoom(ExtinguishWoodShapes[type], transform.position);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector2 froce = (collision.transform.position - contactPoint).normalized;
            player.BeAttacked(1, froce);
        }
    }
}
