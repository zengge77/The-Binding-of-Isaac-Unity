using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGroup : Obstacles
{
    List<Rock> rockList;
    bool isBeDestroyed = false;

    void Start()
    {
        rockList = new List<Rock>();
        rockList.AddRange(GetComponentsInChildren<Rock>());
        foreach (Rock go in rockList)
        {
            go.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void DestroyRock(Rock rock)
    {
        rockList.Remove(rock);

        if (!isBeDestroyed)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            foreach (Rock go in rockList)
            {
                go.GetComponent<SpriteRenderer>().enabled = true;
            }
            isBeDestroyed = true;
        }

        if (rockList.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
