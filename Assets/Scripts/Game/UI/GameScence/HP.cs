using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public GameObject heartFull;
    public GameObject heartHalf;
    public GameObject heartVoid;
    public GameObject soulHeartFull;
    public GameObject soulHeartHalf;

    Player player;

    void Start()
    {
        player = GameManager.Instance.player;
    }

    public void UpdateHP()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < player.Health / 2; i++)
        {
            Instantiate(heartFull, transform);
        }
        for (int i = 0; i < player.Health % 2; i++)
        {
            Instantiate(heartHalf, transform);
        }
        for (int i = 0; i < (player.MaxHealth - player.Health) / 2; i++)
        {
            Instantiate(heartVoid, transform);
        }

        for (int i = 0; i < player.SoulHealth / 2; i++)
        {
            Instantiate(soulHeartFull, transform);
        }
        for (int i = 0; i < player.SoulHealth % 2; i++)
        {
            Instantiate(soulHeartHalf, transform);
        }
    }

}
