using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSoul : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private PlayerData data;
    private PlayerMover playerMover;
    private SpriteRenderer render;
    private Collider2D collider2d;
    private Animator shotSoulAnimator;
    private ContactFilter2D contactFilter;
    private float direction;
    private bool isBump = false;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        render = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
        shotSoulAnimator = GetComponent<Animator>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
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
            transform.Translate(new Vector3(7f, 0, 0));

        yield return new WaitForSeconds(0.4f);

        StopCoroutine(bumpRoutine);
        GameManager.Resource.Destory(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit((int)data.Player[0].shotSoulDamage);
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
