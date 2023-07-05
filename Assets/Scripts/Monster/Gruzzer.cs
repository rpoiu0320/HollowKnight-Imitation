using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gruzzer : Monster
{
    private int damage = 1;
    private int curHp;

    public Gruzzer(int curHp) : base()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ºÎµóÈû");
        if (collision.tag == "Player")
        {
            IHittable hittable = collision.GetComponent<IHittable>();
            hittable?.TakeHit(damage);
        }
    }
}
