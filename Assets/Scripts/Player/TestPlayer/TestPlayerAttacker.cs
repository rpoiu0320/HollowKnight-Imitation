using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerAttacker : MonoBehaviour
{
    [Header("AttackInfo")]
    [SerializeField] private AttackInfo normalAttackInfo;
    [SerializeField] private AttackInfo topAttackInfo;
    [SerializeField] private AttackInfo bottomAttackInfo;
    [Header("Etc")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Player player;

    private LayerMask assailableLayer;
    private Animator attackAnimator;
    private SpriteRenderer attackRenderer;
    private float curCooldown = 0;

    [Serializable]
    public class AttackInfo
    {
        public Vector2 attackPoint;
        public Vector2 attackRange;
    }

    private void Awake()
    {
        assailableLayer = LayerMask.GetMask("Monster", "Spikes");
        attackAnimator = GetComponent<Animator>();
        attackRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        attackRenderer.flipX = player.render.flipX ? true : false;
    }

    private void OnAttack(InputValue value)
    {
        if (player.actionLimite && curCooldown != 0)
            return;

        attackRoutine = StartCoroutine(AttackRoutine());
    }

    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        Attack(player.inputDir.y, player.isGround);

        while (curCooldown < attackCooldown)
        {
            curCooldown += Time.deltaTime;

            yield return null;
        }

        curCooldown = 0;
    }

    private void Attack(float dirY, bool isGround)
    {
        Collider2D[] collider2Ds;
        player.animator.SetTrigger("Attack");

        if (!isGround && dirY < 0)
        {
            attackAnimator.SetTrigger("BottomAttack");
            collider2Ds = Physics2D.OverlapBoxAll(bottomAttackInfo.attackPoint, bottomAttackInfo.attackRange, 0, assailableLayer);
        }
        else if (dirY > 0)
        {
            attackAnimator.SetTrigger("TopAttack");
            collider2Ds = Physics2D.OverlapBoxAll(topAttackInfo.attackPoint, topAttackInfo.attackRange, 0, assailableLayer);
        }
        else
        {
            attackAnimator.SetTrigger("NormalAttack");
            collider2Ds = Physics2D.OverlapBoxAll(normalAttackInfo.attackPoint, normalAttackInfo.attackRange, 0, assailableLayer);
        }

        OverlapBoxCheck(collider2Ds);
    }

    private void OverlapBoxCheck(Collider2D[] inBoxColliders)
    {
        foreach (Collider2D collider in inBoxColliders)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Monster")
                && collider.gameObject.layer != LayerMask.NameToLayer("Spikes"))
                continue;

            SomethingHit(collider);
        }
    }

    private void SomethingHit(Collider2D collider)
    {
        IHittable hittable = collider.GetComponent<IHittable>();
        hittable?.TakeHit(GameManager.Data.AttackDamage);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            ParticleSystem hitEffect = GameManager.Resource.Instantiate<ParticleSystem>
            ("Prefab/Effect/AttackHitEffect", collider.gameObject.transform.position, GameObject.Find("PoolManager").transform);
            hitEffect.Play();
            GameManager.Data.IncreaseCurSoul();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(normalAttackInfo.attackPoint, normalAttackInfo.attackRange);
        Gizmos.DrawWireCube(topAttackInfo.attackPoint, topAttackInfo.attackRange);
        Gizmos.DrawWireCube(bottomAttackInfo.attackPoint, bottomAttackInfo.attackRange);
    }
}
