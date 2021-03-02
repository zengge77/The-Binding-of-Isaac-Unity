using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPools : MonoBehaviour
{

    public GameObject bulletPrefab;
    Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Take()
    {
        if (pool.Count > 0)
        {
            GameObject instanceToReuse = pool.Dequeue();
            instanceToReuse.SetActive(true);
            return instanceToReuse;
        }
        return Instantiate(bulletPrefab);
    }

    public void Back(GameObject gameObjectToPool)
    {
        pool.Enqueue(gameObjectToPool);
        gameObjectToPool.transform.SetParent(transform);
        gameObjectToPool.SetActive(false);
    }
}
