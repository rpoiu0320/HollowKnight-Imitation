using System.Collections;
using UnityEngine;

public class Dive : AttackSkill
{
    private GroundCheck groundCheck;
    private ContactFilter2D contactFilter;

    private new void Awake()
    {
        base.Awake();
        groundCheck = GetComponent<GroundCheck>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
    }

    private void FixedUpdate()
    {
        if (groundCheck.GroundLayerCheck())
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
