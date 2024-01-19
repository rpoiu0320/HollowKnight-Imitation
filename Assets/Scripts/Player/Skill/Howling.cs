using System.Collections;
using UnityEngine;

public class Howling : AttackSkill
{
    private ContactFilter2D contactFilter;
    private float attackTime = 0;

    private new void Awake()
    {
        base.Awake();
        collider2d = GetComponent<Collider2D>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        howlingRoutine = StartCoroutine(HowlingRoutine());
    }

    private void Update()
    {
        attackTime += Time.deltaTime;
    }

    Coroutine howlingRoutine;
    IEnumerator HowlingRoutine()
    {
        while (attackTime < 1f)
        {
            yield return new WaitForSeconds(0.25f);

            collider2d.enabled = false;
            collider2d.enabled = true;
        }

        GameManager.Resource.Destory(gameObject);
    }

    protected override void SkillActive(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            if (target.tag == "Monster")
                knockBack.OnKnockBackRoutine(target);

            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit(GameManager.Data.HowlingDamage);
        }
    }
}
