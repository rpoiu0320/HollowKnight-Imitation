using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSkiller : MonoBehaviour
{
    private ChargeSkill chargeSkill;
    private PlayerMover playerMover;
    private Animator animator;
    private float attackCooldown;
    private float skillPressedTime;
    private float chargeSkillTime;
    private bool isSkill;
    private bool endSkill;



    private void Awake()
    {
        playerMover = playerMover = GetComponent<PlayerMover>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isSkill)
        {
            skillPressedTime += Time.deltaTime;
        }
    }

    Coroutine skillRoutine;
    IEnumerator SkillRoutine()
    {        
        while (isSkill)
        {
            skillPressedTime += Time.deltaTime;
            
            if (skillPressedTime >= 0.5f && playerMover.IsGround())
            {
                chargeSkillRoutine = StartCoroutine(ChargeSkillRoutine());

                yield break;
            }
            yield return null;
        }

        if(!isSkill)
        {
            Debug.Log(skillPressedTime);
            if (skillPressedTime < 0.4f)
            {
                if(playerMover.InputDir().y > 0)
                    OnHowling();
                else if(playerMover.InputDir().y < 0 && !playerMover.IsGround())
                    OnDive();
                else
                    OnShotSoul();

                animator.SetTrigger("Skill");

                yield break;
            }
        }

        StopCoroutine(skillRoutine);
    }

    private void OnSkill(InputValue value)
    {
        isSkill = value.isPressed;
        
        if(isSkill)
        {
            endSkill = false;
            skillPressedTime = 0;
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

        this.chargeSkill = chargeSkill;
    }

    Coroutine chargeSkillRoutine;
    IEnumerator ChargeSkillRoutine()
    {
        animator.SetBool("IsChargeSkill", true);
        OnChargeSkill();
        chargeSkillTime = 0;

        while (isSkill)
        {
            chargeSkillTime += Time.deltaTime;

            if(chargeSkillTime > 0.5f)
            {
                Debug.Log("Èú");
            }

            yield return null;
        }

        animator.SetBool("IsChargeSkill", false);
        GameManager.Resource.Destory(chargeSkill.gameObject);
        StopCoroutine(chargeSkillRoutine);
    }

    public bool EndSkill()
    {
        return endSkill;
    }
}
