using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkiller : MonoBehaviour
{
    [SerializeField] private Animator healingAnimator;
    [SerializeField] private Animator onHealAnimator;

    private Player player;
    private bool ActionLimite { get { return player.actionLimite; } set { player.actionLimite = value; } }
    private Animator Animator { get { return player.animator; } }
    private Rigidbody2D Rb { get { return player.rb; } }
    private SpriteRenderer Render { get { return player.render; } }
    private Vector2 InputDIr { get { return player.inputDir; } }
    private bool IsGround { get { return player.isGround; } }
    private float pressTime = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnSkill(InputValue value)
    {
        bool isSkill = value.isPressed;

        if (isSkill && player.isGround && !player.actionLimite)
        {
            pressTime += Time.deltaTime;
            healingRoutine = StartCoroutine(HealingRoutine());

            return;
        }    

        if (!isSkill)
        {
            if (healingRoutine != null)
            {
                Animator.SetBool("IsHealing", player.actionLimite = false);
                healingAnimator.SetTrigger("HealEnd");
                StopCoroutine(healingRoutine);
                
                return;
            }

            pressTime = 0;

            if (GameManager.Data.CurSoul >= 3 || !ActionLimite)
                attackSkillRoutine = StartCoroutine(AttackSkillRoutine(InputDIr.y, IsGround));
        }
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

    Coroutine healingRoutine;
    IEnumerator HealingRoutine()
    {
        float healingTime = 0;
        player.animator.SetBool("IsHealing", player.actionLimite = true);
        healingAnimator.SetTrigger("HealStart");

        while (GameManager.Data.CurSoul > 1)
        {
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
    }
}
