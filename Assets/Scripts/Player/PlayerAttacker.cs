using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] Transform attackPoint;

    private Animator animator;
    private PlayerMover playerMover;
    private float attackCooldown;
    private float chargeTime;
    private bool isAttack;
    private bool isSkill;
    public UnityEvent OnHowling;
    public UnityEvent OnShotSoul;
    public UnityEvent OnDive;
    public UnityEvent OnCharge;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {

    }

    private void OnSkill(InputValue value)
    {
        if (playerMover.LimitMove())
            return;

        isSkill = value.isPressed;
        chargeTime = 0;

        if (isSkill)
        {
            chargeTime += Time.deltaTime;

            animator.SetTrigger("Skill");

            if (chargeTime > 0.5f)
            {
                OnCharge?.Invoke();
            }
            else if (playerMover.InputDir().y > 0)
                OnHowling?.Invoke();
            else if (playerMover.InputDir().y < 0 && !playerMover.IsGround())
                OnDive?.Invoke();
            else
                OnShotSoul?.Invoke();
        }
    }

    private void AttackCooldown()
    {
        if(isAttack)
        {
            return;
        }

        attackCooldown += Time.deltaTime;
    }

    private void OnAttack(InputValue value)
    {
        if (playerMover.LimitMove())     // 대시 중 공격 안됨
            return;

        isAttack = value.isPressed;

        if (isAttack)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void AttackUp()
    {

    }

    private void JumpAttackDown()
    {

    }
    
    public bool IsAttack()
    {
        return isAttack;
    }

    public bool IsSkill()
    {
        return isSkill;
    }
}
