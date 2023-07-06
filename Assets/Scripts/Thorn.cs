using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour, IHittable
{
    public void TakeHit(int damage)
    {
        
    }

    private void test()
    {/*
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(jumpAttackDownPoint.position, jumpAttackDownRange, 0, hitMask);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Monster") || collider.gameObject.layer != LayerMask.NameToLayer("Spikes"))
                continue;

            IHittable hittable = collider.GetComponent<IHittable>();
            hittable?.TakeHit(player.data.Player[0].attackDamage);
            playerMover.rb.Velocity(Vector2.up * 10);
        }
        */
    }
}
