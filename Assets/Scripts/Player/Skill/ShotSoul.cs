using System.Collections;
using UnityEngine;

public class ShotSoul : AttackSkill
{
    [SerializeField] private float moveSpeed;

    private bool isBump = false;
    public SpriteRenderer render;

    private new void Awake()
    {
        base.Awake();
        render = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!isBump)
            Move();
    }

    private void Move()
    {
        if (!render.flipX)
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        else
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
    }

    Coroutine bumpRoutine;
    IEnumerator BumpRoutine()
    {
        isBump = true;
        animator.SetTrigger("IsBump");
        collider2d.enabled = false;

        if (!render.flipX)
            transform.Translate(new Vector3(7f, 0, 0));
        else
            transform.Translate(new Vector3(-4f, 0, 0));

        yield return new WaitForSeconds(0.4f);

        GameManager.Resource.Destroy(gameObject);

        yield break;
    }

    protected override void SkillActive(Collider2D target)
    {
        if(target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hittable = target.GetComponent<IHittable>();
            hittable?.TakeHit(GameManager.Data.ShotSoulDamage);
            knockBack.OnKnockBackRoutine(target);
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
            GameManager.Resource.Destroy(gameObject);
        }
    }
}
