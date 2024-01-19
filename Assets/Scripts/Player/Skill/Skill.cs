using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skill : MonoBehaviour
{
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected abstract void SkillActive(Collider2D collision);
}
