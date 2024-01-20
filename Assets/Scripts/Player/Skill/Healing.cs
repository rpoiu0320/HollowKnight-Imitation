using UnityEngine;

public class Healing : Skill
{
    [SerializeField] private ParticleSystem healEffect;

    private new void Awake()
    {
        base.Awake();

    }

    protected override void SkillActive(Collider2D collision)
    {
        
    }
}
