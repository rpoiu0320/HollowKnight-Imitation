using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSkiller : MonoBehaviour
{
    private PlayerMover playerMover;
    private Animator animator;
    private float attackCooldown;
    private float chargeTime;
    private bool isSkill;
    

    private void Awake()
    {
        playerMover = playerMover = GetComponent<PlayerMover>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isSkill)
        {
            chargeTime += Time.deltaTime;
        }
    }

    Coroutine skillRoutine;
    IEnumerator SkillRoutine()
    {
        while (isSkill)
        {
            chargeTime += Time.deltaTime;
            
            if (chargeTime >= 0.5f && playerMover.IsGround())
            {
                OnChargeSkill();
                yield break;
            }
            yield return null;
        }

        if(!isSkill)
        {
            Debug.Log(chargeTime);
            if (chargeTime < 0.4f)
            {
                if(playerMover.InputDir().y > 0)
                {
                    animator.SetTrigger("Skill");
                    OnHowling();
                }
                else if(playerMover.InputDir().y < 0 && !playerMover.IsGround())
                {
                    animator.SetTrigger("Skill");
                    OnDive();
                }
                else
                {
                    animator.SetTrigger("Skill");
                    OnShotSoul();
                }
                yield break;
            }
        }
        yield break;
    }

    private void OnSkill(InputValue value)
    {
        if (playerMover.LimitMove())
            return;

        isSkill = value.isPressed;
        
        if(isSkill)
        {
            chargeTime = 0;
            skillRoutine = StartCoroutine(SkillRoutine());
        }
    }

    public void OnHowling()
    {
        Howling howling = GameManager.Resource.Instantiate<Howling>
            ("Prefab/Player/Skill/Howling", new Vector3(playerMover.transform.position.x, playerMover.transform.position.y + 10f, playerMover.transform.position.z), GameObject.Find("SkillPoint").transform);
    }

    public void OnShotSoul()
    {
        ShotSoul shotSoul = GameManager.Resource.Instantiate<ShotSoul>
            ("Prefab/Player/Skill/ShotSoul", playerMover.transform.position, GameObject.Find("PoolManager").transform);
    }

    public void OnDive()
    {
        Dive dive = GameManager.Resource.Instantiate<Dive>
            ("Prefab/Player/Skill/Dive", playerMover.transform.position, GameObject.Find("SkillPoint").transform);
    }

    public void OnChargeSkill()
    {
        ChargeSkill chargeSkill = GameManager.Resource.Instantiate<ChargeSkill>
            ("Prefab/Player/Skill/ChargeSkill", playerMover.transform.position, GameObject.Find("SkillPoint").transform);
    }

    Coroutine chargeSkillRoutine;
    IEnumerator ChargeSkillRoutine()
    {
        yield return null;
    }
}
