using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSkill : Skill
{
    protected Collider2D collider2d;
    protected KnockBack knockBack;

    protected new void Awake()
    {
        base.Awake();
        collider2d = GetComponent<Collider2D>();
        knockBack = GetComponent<KnockBack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SkillActive(collision);
    }
}
