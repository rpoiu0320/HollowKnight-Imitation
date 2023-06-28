using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] Transform attackPoint;

    private Animator animator;
    private PlayerMover playerMover;
    private float attackCooldown;
    private bool possibleAttack;
    private bool isAttack;
    private bool isSkill;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        Debug.Log(isAttack);
        //AttackCooldown();
    }

    private void OnSkill(InputValue value)
    {
        if (playerMover.IsDash())     // 대시 중 공격 안되도록
            return;

        isSkill = value.isPressed;

        if (isSkill)
        {
            animator.SetTrigger("Skill");
            switch (playerMover.InputDir().y)
            {
                case 0:
                    ShotSoul();
                    break;
                case 1:
                    Howling();
                    break;
                case -1:
                    Dive();
                    break;
            }
        }
    }

    private void Howling()
    {

    }

    private void ShotSoul()
    {
        ShotSoul shotSoul = GameManager.Resource.Instantiate<ShotSoul>("Prefab/Player/Skill/ShotSoul", attackPoint.position, GameObject.Find("PoolManager").transform);
    }

    private void Dive()
    {

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
        if (playerMover.IsDash())     // 대시 중 공격 안됨
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
