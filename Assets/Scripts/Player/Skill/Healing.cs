using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : Skill
{
    [SerializeField]private ParticleSystem healEffect;

    protected override void SkillActive(Collider2D collision)
    {
        throw new System.NotImplementedException();
    }
}
