using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Howling : MonoBehaviour
{
    private PlayerData data;
    private ContactFilter2D contactFilter;
    private Collider2D collider2d;
    private float attackTime = 0;

    private void OnEnable()
    {
        data = GameObject.FindWithTag("Player").GetComponent<Player>().data;
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
            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit(data.Player[0].howlingDamage);
        }
    }
}
