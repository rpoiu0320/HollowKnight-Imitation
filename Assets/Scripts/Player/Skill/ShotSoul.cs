using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ShotSoul : Skill
{
    [SerializeField] float moveSpeed;

    private Player player;
    private PlayerMover playerMover;
    private SpriteRenderer render;
    private Collider2D collider2d;
    private Animator shotSoulAnimator;
    //private ContactFilter2D contactFilter;
    private float direction;
    private bool isBump = false;
    public UnityEvent<Collider2D> OnKnockBack;

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerMover = player.GetComponent<PlayerMover>();
        render = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
        shotSoulAnimator = GetComponent<Animator>();
        //contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        direction = playerMover.LastDirX();
    }

    private void Update()
    {
        if (!isBump)
            Move();
    }

    private void Move()
    {
        if (direction > 0)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = false;
        }
        else if (direction < 0)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = true;
        }
    }

    Coroutine bumpRoutine;
    IEnumerator BumpRoutine()
    {
        isBump = true;
        shotSoulAnimator.SetTrigger("IsBump");
        collider2d.enabled = false;

        if (direction > 0)
            transform.Translate(new Vector3(7f, 0, 0));
        else if (direction < 0)
            transform.Translate(new Vector3(-4f, 0, 0));

        yield return new WaitForSeconds(0.4f);

        GameManager.Resource.Destory(gameObject);

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit(player.ShotSoulDamage);
            OnKnockBack?.Invoke(target);
        }
        else if(target.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            bumpRoutine = StartCoroutine(BumpRoutine());
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (!isBump && target.gameObject.layer == LayerMask.NameToLayer("MapBoundary"))
        {
            GameManager.Resource.Destory(gameObject);
        }
    }
}
