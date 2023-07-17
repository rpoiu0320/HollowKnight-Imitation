using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Howling : Skill
{
    private ContactFilter2D contactFilter;
    private Collider2D collider2d;
    private float attackTime = 0;
    public UnityEvent<Collider2D> OnKnockBack;

    private void OnEnable()
    {
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
        StopCoroutine(howlingRoutine);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            if (target.tag == "Monster")
                OnKnockBack?.Invoke(target.gameObject.GetComponent<Collider2D>());

            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit(GameManager.Data.HowlingDamage);
        }
    }
}
