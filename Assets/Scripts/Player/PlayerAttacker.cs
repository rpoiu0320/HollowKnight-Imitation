using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    private Animator Animator { get { return player.animator; } }
    private Rigidbody2D Rb { get { return player.rb; } }
    private SpriteRenderer Render { get { return player.render; } }
    private Vector2 InputDIr { get { return player.inputDir; } }
    private bool ActionLimite { get { return player.actionLimite; } }
    private bool IsGround { get { return player.isGround; } }

    private Player player;
    private KnockBack knockback;
    private LayerMask assailableLayer;
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
        knockback = GetComponent<KnockBack>();
    }

    private void Update()
    {
        if (InputDIr.x != 0 && InputDIr.y == 0)
         ResetAttackBoxPosition();
    }

    #region AttackPosition
    /// <summary>
    /// 바라보는 방향 따라 공격 범위 이동
    /// </summary>
    private void ResetAttackBoxPosition()
    {
        if (Render.flipX)
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
    #endregion

    // 공격 과정 전체
    #region Attack
    private void OnAttack(InputValue value)
    {
        if (ActionLimite || curCooldown != 0)
            return;

        attackRoutine = StartCoroutine(AttackRoutine());
    }

    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        Attack(InputDIr.y, IsGround);

        while (curCooldown < attackCooldown)
        {
            curCooldown += Time.deltaTime;

            yield return null;
        }

        ResetAttackBoxPosition();
        curCooldown = 0;
    }

    /// <summary>
    /// 입력값, 상황에 따라 알맞는 공격 진행
    /// </summary>
    /// <param name="dirY">Y축 입력값</param>
    /// <param name="isGround">땅 밟고있는지 유무</param>
    private void Attack(float dirY, bool isGround)
    {
        Collider2D[] collider2Ds;
        Animator.SetTrigger("Attack");

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

    /// <summary>
    /// 탐지된 Colliers를 기반으로 판정 진행
    /// </summary>
    /// <param name="inBoxColliders">공격 범위 내 탐지된 Col</param>
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

    /// <summary>
    /// 공격 판정 후속처리
    /// </summary>
    /// <param name="collider"></param>
    private void SomethingHit(Collider2D collider)
    {
        IHittable hittable = collider.GetComponent<IHittable>();
        hittable?.TakeHit(GameManager.Data.AttackDamage);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            GameManager.Data.IncreaseCurSoul();
            knockback.OnKnockBackRoutine(collider);
        }

        if (InputDIr.y < 0)
            Rb.velocity = new Vector2(Rb.velocity.x, 40);
        else if (InputDIr.y == 0)
        {
            if (Render.flipX)
                Rb.velocity = new Vector2(15, Rb.velocity.y);
            else
                Rb.velocity = new Vector2(-15, Rb.velocity.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackBox.transform.position, normalAttackInfo.attackRange);
        Gizmos.DrawWireCube(attackBox.transform.position, topAttackInfo.attackRange);
        Gizmos.DrawWireCube(attackBox.transform.position, bottomAttackInfo.attackRange);
    }
#endregion
}
