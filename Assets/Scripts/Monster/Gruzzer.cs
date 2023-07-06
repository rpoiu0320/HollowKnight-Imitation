using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gruzzer : Monster
{
    private Animator animator;
    private Flying flying;

    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        flying = GetComponent<Flying>();
    }

    private void Update()
    {
        if (curHp <= 0 && alive)
        {
            flying.IsFly(false);
            gruzzerdieRoutine = StartCoroutine(GruzzerdieRoutine());
        }
    }

    Coroutine gruzzerdieRoutine;
    IEnumerator GruzzerdieRoutine()
    {
        alive = false;
        animator.SetTrigger("IsDie");

        yield return new WaitForSeconds(0.35f);

        GameManager.Resource.Destory(gameObject);

        yield break;
    }
}
