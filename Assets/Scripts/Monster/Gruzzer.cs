using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gruzzer : Monster
{
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ºÎµóÈû");
        if (collision.tag == "Player")
        {
            IHittable hittable = collision.GetComponent<IHittable>();
            hittable?.TakeHit(0);
        }
    }
}
