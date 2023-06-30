using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Howling : MonoBehaviour
{
    [SerializeField] float damage;
    private PlayerMover playerMover;
    private ContactFilter2D contactFilter;
    private Collider2D collider2d;
    private Collider2D[] colliderResults = new Collider2D[10];
    private float attackTime = 0;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        collider2d = GetComponent<Collider2D>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        howlingRoutine = StartCoroutine(HowlingRoutine());
    }

    private void OnDisable()
    {
        playerMover.LookSync();
        playerMover.LimitMove(false);
        Debug.Log("end");
    }

    private void Update()
    {
        // Debug.Log(Physics2D.OverlapCollider(collider2d, contactFilter, colliderResults));
        attackTime += Time.deltaTime;
    }

    Coroutine howlingRoutine;

    IEnumerator HowlingRoutine()
    {
        playerMover.LimitMove(true);

        while (attackTime < 1f)
        {
            yield return new WaitForSeconds(0.2f);

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
            Debug.Log("Monster Enter");
        }
    }
}
