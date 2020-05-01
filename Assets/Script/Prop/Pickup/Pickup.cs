using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : Prop
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && IsTrigger() && isCanBeTriggerWithPlayer)
        {
            Effect();
            UI.attributes.UpDateAttributes();
            After();
        }
    }
}
