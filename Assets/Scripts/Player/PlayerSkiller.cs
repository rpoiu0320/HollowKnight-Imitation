using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkiller : MonoBehaviour
{
    [SerializeField] private Animator healingAnimator;
    [SerializeField] private Animator onHealAnimator;

    private bool ActionLimite { get { return player.actionLimite; } set { player.actionLimite = value; } }
    private Animator Animator { get { return player.animator; } }
    private Rigidbody2D Rb { get { return player.rb; } }
    private SpriteRenderer Render { get { return player.render; } }
    private Vector2 InputDIr { get { return player.inputDir; } }
    private bool IsGround { get { return player.isGround; } }
    private Player player;
    private bool isSkill = false;
    private bool pressCoroutineRunningCheck = false;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void OnSkill(InputValue value)
    {
        isSkill = value.isPressed;

        if (isSkill && !pressCoroutineRunningCheck)
            pressTimeCheckRoutine = StartCoroutine(PressTimeCheckRoutine());    // healing이 시작하는 조건

        if (!isSkill && healingRoutine == null)   // 스킬이 시작되는 조건
            if (GameManager.Data.CurSoul >= 3 && !ActionLimite)     // healing 제외 다른 스킬이 시작되는 조건
                attackSkillRoutine = StartCoroutine(AttackSkillRoutine(InputDIr.y, IsGround));
    }

    #region AttackSkills
    Coroutine attackSkillRoutine;
    IEnumerator AttackSkillRoutine(float dirY, bool isGround)
    {
        GameManager.Data.DecreaseCurSoul();
        ActionLimite = true;

        if (!isGround && dirY < 0)
            yield return StartCoroutine(DiveRoutine());
        else if (dirY > 0)
            yield return StartCoroutine(HowlingRoutine());
        else
            yield return StartCoroutine(ShotSoulRoutine());

        ActionLimite = false;
    }

    IEnumerator DiveRoutine()
    {
        Animator.SetTrigger("Skill");
        Dive dive = GameManager.Resource.Instantiate<Dive>
            ("Prefab/Player/Skill/Dive", transform.position, transform);

        Rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(0.4f);

        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rb.velocity = new Vector2(0f, -60f);

        yield return new WaitUntil(() => IsGround == true);

        yield return new WaitForSeconds(0.4f);
    }

    IEnumerator HowlingRoutine()
    {
        Animator.SetTrigger("Skill");
        GameManager.Resource.Instantiate<Howling>("Prefab/Player/Skill/Howling", transform.position);
        Rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(1f);

        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rb.velocity = new Vector2(0f, -0.01f);
    }

    IEnumerator ShotSoulRoutine()
    {
        Animator.SetTrigger("Skill");
        ShotSoul shotSoul = GameManager.Resource.Instantiate<ShotSoul>
            ("Prefab/Player/Skill/ShotSoul", transform.position);

        shotSoul.render.flipX = Render.flipX;
        Rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(0.4f);

        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rb.velocity = new Vector2(0f, -0.01f);
    }
    #endregion

    Coroutine pressTimeCheckRoutine;
    IEnumerator PressTimeCheckRoutine()
    {
        float pressTime = 0;
        pressCoroutineRunningCheck = true;

        while (player.isGround && !player.actionLimite && isSkill)
        {
            pressTime += Time.deltaTime;

            if (pressTime > 1f)
            {
                healingRoutine = StartCoroutine(HealingRoutine());

                break;
            }

            yield return null;
        }

        pressCoroutineRunningCheck = false;
    }

    Coroutine healingRoutine;
    IEnumerator HealingRoutine()
    {
        Debug.Log("HealingStart");
        float healingTime = 0;
        player.animator.SetBool("IsHealing", player.actionLimite = true);
        healingAnimator.SetTrigger("HealStart");
        
        while (isSkill)
        {
            if (GameManager.Data.CurSoul < 1)
                break;

            if (healingTime > 1f)
            {
                onHealAnimator.SetTrigger("OnHeal");
                GameManager.Data.DecreaseCurSoul(1);
                GameManager.Data.IncreaseCurHp();
                healingTime = 0;
            }

            healingTime += Time.deltaTime;

            yield return null;
        }

        Animator.SetBool("IsHealing", player.actionLimite = false);
        healingAnimator.SetTrigger("HealEnd");

        yield break;
    }
}
