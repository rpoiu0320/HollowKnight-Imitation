using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dive : Skill
{
    public Dive(bool isGround)
    {
        this.isGround = isGround;
    }

    private ContactFilter2D contactFilter;
    private bool isGround;

    private void OnEnable()
    {
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
    }

    public void DiveToGround()
    {
        diveRoutine = StartCoroutine(DiveRoutine());
    }

    Coroutine diveRoutine;
    IEnumerator DiveRoutine()
    {
        animator.SetTrigger("GroundCheck");
        collider2d.enabled = true;

        yield return new WaitForSeconds(0.35f);

        GameManager.Resource.Destory(gameObject);
        StopCoroutine(diveRoutine);
    }

    protected override void SkillActive(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit(GameManager.Data.DiveDagame);
            knockBack.OnKnockBackRoutine(target);
        }
    }
}
