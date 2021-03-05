using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RandomGameObjectTable))]


public class Rock : Obstacles, IDestructible
{
    public List<Sprite> shapes;
    public List<Sprite> fragment;

    RockGroup rockGroup;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        rockGroup = GetComponentInParent<RockGroup>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = shapes[UnityEngine.Random.Range(0, shapes.Count)];
    }


    public void DestorySelf()
    {
        generate.GenerateTracesInCurrentRoom(fragment, 3, transform.position, 0.5f);
        GenerateReward();

        if (rockGroup != null)
        {
            rockGroup.DestroyRock(this);
        }
        Destroy(gameObject);
    }
}
