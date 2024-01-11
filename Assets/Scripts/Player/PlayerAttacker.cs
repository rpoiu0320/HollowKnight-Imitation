using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [Header("AttackInfo")]
    [SerializeField] private AttackInfo normalAttackInfo;
    [SerializeField] private AttackInfo topAttackInfo;
    [SerializeField] private AttackInfo bottomAttackInfo;
    [Header("Etc")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject attackBox;
    [SerializeField] private Animator attackAnimator;
    [SerializeField] private SpriteRenderer attackRenderer;

    private LayerMask assailableLayer;
    private Player player;
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
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.inputDir.x != 0 && player.inputDir.y == 0)
         ResetAttackBoxPosition();
    }

    private void ResetAttackBoxPosition()
    {
        if (player.render.flipX)
        {
            attackRenderer.flipX = true;
            attackBox.transform.localPosition = new Vector2(-normalAttackInfo.attackPoint.x, normalAttackInfo.attackPoint.y);
        }
        else
        {
            attackRenderer.flipX = false;
            attackBox.transform.localPosition = new Vector2(normalAttackInfo.attackPoint.x, normalAttackInfo.attackPoint.y);
        }
    }

    private void OnAttack(InputValue value)
    {
        if (player.actionLimite || curCooldown != 0)
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

        ResetAttackBoxPosition();
        curCooldown = 0;
    }

    private void Attack(float dirY, bool isGround)
    {
        Collider2D[] collider2Ds;
        player.animator.SetTrigger("Attack");

        if (!isGround && dirY < 0)
        {
            attackAnimator.SetTrigger("BottomAttack");
            attackBox.transform.localPosition = new Vector2(bottomAttackInfo.attackPoint.x, bottomAttackInfo.attackPoint.y);
            collider2Ds = Physics2D.OverlapBoxAll(attackBox.transform.position, bottomAttackInfo.attackRange, 0, assailableLayer);
        }
        else if (dirY > 0)
        {
            attackAnimator.SetTrigger("TopAttack");
            attackBox.transform.localPosition = new Vector2(topAttackInfo.attackPoint.x, topAttackInfo.attackPoint.y);
            collider2Ds = Physics2D.OverlapBoxAll(attackBox.transform.position, topAttackInfo.attackRange, 0, assailableLayer);
        }
        else
        {
            attackAnimator.SetTrigger("NormalAttack");
            collider2Ds = Physics2D.OverlapBoxAll(attackBox.transform.position, normalAttackInfo.attackRange, 0, assailableLayer);
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
            GameManager.Data.IncreaseCurSoul();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackBox.transform.position, normalAttackInfo.attackRange);
        Gizmos.DrawWireCube(attackBox.transform.position, topAttackInfo.attackRange);
        Gizmos.DrawWireCube(attackBox.transform.position, bottomAttackInfo.attackRange);
    }
}
