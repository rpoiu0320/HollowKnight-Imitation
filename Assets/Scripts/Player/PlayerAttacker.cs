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
    private bool isAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
    }

    private void AttackCooldown()
    {
        if(isAttack)
        {
            return;
        }
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
}
