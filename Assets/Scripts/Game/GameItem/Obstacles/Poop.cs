using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : Obstacles, IAttackable, IDestructible
{
    public List<Sprite> shapes;
    public List<Sprite> fragment;
    SpriteRenderer spriteRenderer;

    int HP = 4;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            spriteRenderer.sprite = shapes[shapes.Count - (HP + 1)];
        }
    }

    public void DestorySelf()
    {
        manager.GenerateTracesInCurrentRoom(fragment, 3, transform.position, 0.3f);
        manager.GenerateTraceInCurrentRoom(shapes[shapes.Count - 1], transform.position);
        GenerateReward();
        Destroy(gameObject);
    }
}
