using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSkiller : MonoBehaviour
{
    [SerializeField] float diveSpeed;

    private ChargeSkill chargeSkill;
    private PlayerMover playerMover;
    private Animator animator;
    private float skillPressedTime;
    private float chargeSkillTime;
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
            skillPressedTime += Time.deltaTime;
        }
    }

    Coroutine skillRoutine;
    IEnumerator SkillRoutine()
    {        
        while (isSkill)
        {
            skillPressedTime += Time.deltaTime;
            
            if (skillPressedTime >= 0.4f && playerMover.IsGround())
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
                if(playerMover.LookDir().y > 0)
                {
                    // TODO : 공중에서 Howling사용 시 스킬이 끝나도 내려오지 않음, 공격 및 위, 아래를 제외한 다른 입력을 해야 내려옴

                    OnHowling();
                    animator.SetTrigger("Skill");
                    playerMover.LimitMove(true);

                    yield return new WaitForSeconds(1f);

                    playerMover.LimitMove(false);
                }
                else if(playerMover.LookDir().y < 0 && !playerMover.IsGround())
                {
                    OnDive();
                    animator.SetTrigger("Skill");
                    playerMover.LimitMove(true);

                    yield return new WaitForSeconds(0.5f);

                    while (!playerMover.IsGround())
                    {
                        playerMover.transform.Translate(new Vector3(0, -diveSpeed * Time.deltaTime, 0));

                        yield return null;
                    }

                    yield return new WaitForSeconds(0.35f);

                    playerMover.LimitMove(false);
                }
                else
                {
                    OnShotSoul();
                    playerMover.LimitMove(true);
                    animator.SetTrigger("Skill");

                    yield return new WaitForSeconds(0.3f);

                    playerMover.LimitMove(false);
                }

                

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
                Debug.Log("힐");
            }

            yield return null;
        }

        animator.SetBool("IsChargeSkill", false);
        GameManager.Resource.Destory(chargeSkill.gameObject);
        StopCoroutine(chargeSkillRoutine);
    }

    public bool IsSkill()
    {
        return isSkill;
    }
}
