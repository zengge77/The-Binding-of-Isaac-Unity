using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Heart : Pickup
{
    protected override void After()
    {
        Destroy(gameObject);
    }
}
