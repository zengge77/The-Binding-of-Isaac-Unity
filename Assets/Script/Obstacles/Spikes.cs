using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Obstacles
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            Vector2 direction = Vector3.Normalize(player.transform.position - transform.position);
            player.BeAttacked(1, direction);
        }
    }
}
