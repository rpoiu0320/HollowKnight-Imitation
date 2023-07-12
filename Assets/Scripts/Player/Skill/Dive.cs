using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dive : Skill
{
    private Player player;
    private PlayerMover playerMover;
    private Animator diveAnimator;
    private Collider2D collider2d;
    private ContactFilter2D contactFilter;
    public UnityEvent<Collider2D> OnKnockBack;

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerMover = player.GetComponent<PlayerMover>();
        diveAnimator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        diveRoutine = StartCoroutine(DiveRoutine());
    }

    Coroutine diveRoutine;
    IEnumerator DiveRoutine()
    {
        yield return new WaitUntil(() => playerMover.IsGround() == true);

        diveAnimator.SetTrigger("GroundCheck");
        collider2d.enabled = true;

        yield return new WaitForSeconds(0.35f);

        GameManager.Resource.Destory(gameObject);
        StopCoroutine(diveRoutine);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit(player.DiveDagame);
            OnKnockBack?.Invoke(target);
        }
    }
}
