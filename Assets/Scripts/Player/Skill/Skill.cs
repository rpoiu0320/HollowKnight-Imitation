using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skill : MonoBehaviour
{
    public SpriteRenderer render;
    protected Animator animator;
    protected Collider2D collider2d;
    protected UnityEvent<Collider2D> OnKnockBack;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SkillActive(collision);
    }

    protected abstract void SkillActive(Collider2D collision);
}
