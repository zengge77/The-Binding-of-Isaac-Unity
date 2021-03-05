using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBomb : MonoBehaviour
{
    public Sprite traces;
    float detonatingTime = 1.5f;
    float radius = 0.5f;

    void Start()
    {
        StartCoroutine(Detonating(detonatingTime));
    }

    IEnumerator Detonating(float detonatingTime)
    {
        SpriteRenderer SR = GetComponent<SpriteRenderer>();

        float[] flashCD = new float[] { 0f, 0.4f, 0.3f, 0.2f };
        float flashTime = 0.05f;
        int flashCDIndex = 0;

        float time = 0;
        float timeGate = flashCD[0];
        while (time < detonatingTime)
        {
            if (time >= timeGate && SR.color == Color.white)
            {
                SR.color = Color.red;
                timeGate += flashTime;
                if (flashCDIndex < flashCD.Length - 1)
                {
                    flashCDIndex++;
                }
            }
            else if (time >= timeGate && SR.color == Color.red)
            {
                SR.color = Color.white;
                timeGate = timeGate + flashCD[flashCDIndex] - flashTime;
            }

            time += Time.deltaTime;
            yield return 0;
        }
        SR.color = Color.white;
        Explosion();
    }

    void Explosion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var item in colliders)
        {
            Vector2 force = (item.transform.position - transform.position).normalized;
            if (item.GetComponent<IDestructible>() != null)
            {
                item.GetComponent<IDestructible>().DestorySelf();
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().BeAttacked(2, force, 1.5f);
            }
            else if (item.GetComponent<IAttackable>() != null)
            {
                item.GetComponent<IAttackable>().BeAttacked(10, force, 1.5f);
            }
            else if (item.GetComponent<Rigidbody2D>())
            {
                item.GetComponent<Rigidbody2D>().AddForce(force * 10);
            }
        }
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().Play("Explosion");
        GameManager.Instance.level.generate.GenerateTraceInCurrentRoom(traces, transform.position);
        Invoke("Destroy", 0.5f);
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
