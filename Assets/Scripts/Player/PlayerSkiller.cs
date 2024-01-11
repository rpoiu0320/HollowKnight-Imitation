using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkiller : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnSkill(InputValue value)
    {
        bool isSkill = value.isPressed;

        if (GameManager.Data.CurSoul < 3 || player.actionLimite)
            return;

        if (isSkill)
            skillRoutine = StartCoroutine(SkillRoutine(player.inputDir.y, player.isGround));
    }

    Coroutine skillRoutine;
    IEnumerator SkillRoutine(float dirY, bool isGround)
    {
        GameManager.Data.DecreaseCurSoul();
        player.actionLimite = true;

        if (!isGround && dirY < 0)
            yield return StartCoroutine(DiveRoutine());
        else if (dirY > 0)
            yield return StartCoroutine(HowlingRoutine());
        else
            yield return StartCoroutine(ShotSoulRoutine());

        player.actionLimite = false;
    }

    IEnumerator DiveRoutine()
    {
        player.animator.SetTrigger("Skill");
        Dive dive = GameManager.Resource.Instantiate<Dive>
            ("Prefab/Player/Skill/Dive", transform.position, transform);

        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(0.4f);

        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.rb.velocity = new Vector2(0f, -60f);

        yield return new WaitUntil(() => player.isGround == true);

        dive.DiveToGround();

        yield return new WaitForSeconds(0.4f);
    }

    IEnumerator HowlingRoutine()
    {
        player.animator.SetTrigger("Skill");
        GameManager.Resource.Instantiate<Howling>("Prefab/Player/Skill/Howling", transform.position);
        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(1f);

        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.rb.velocity = new Vector2(0f, -0.01f);
    }

    IEnumerator ShotSoulRoutine()
    {
        player.animator.SetTrigger("Skill");
        ShotSoul shotSoul = GameManager.Resource.Instantiate<ShotSoul>
            ("Prefab/Player/Skill/ShotSoul", transform.position);

        shotSoul.render.flipX = player.render.flipX;
        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(0.4f);

        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.rb.velocity = new Vector2(0f, -0.01f);
    }
}
