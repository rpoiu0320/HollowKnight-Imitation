using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMover;

public class TestPlayerSkiller : MonoBehaviour
{
    private Player player;
    private LayerMask assailableLayer;

    private void Awake()
    {
        assailableLayer = LayerMask.GetMask("Monster", "Spikes");
        player = GetComponent<Player>();
    }

    private void OnSkill(InputValue value)
    {
        if (GameManager.Data.CurSoul < 3 || player.actionLimite)
            return;

        skillRoutine = StartCoroutine(SkillRoutine(player.inputDir.y, player.isGround));
    }

    Coroutine skillRoutine;
    IEnumerator SkillRoutine(float dirY, bool isGround)
    {
        player.actionLimite = true;
        Skill skill;

        if (!isGround && dirY < 0)
        {
            skill = UseDive();

            yield return new WaitUntil(() => skill.gameObject.activeSelf == false);
        }
        else if (dirY > 0)
        {
            skill = UseHowling();

            yield return new WaitUntil(() => skill.gameObject.activeSelf == false);
        }
        else
        {
            skill = UseShotSoul();
            skill.render.flipX = player.render.flipX;
            player.rb.constraints = RigidbodyConstraints2D.FreezeAll;

            yield return new WaitForSeconds(0.4f);

            player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            player.rb.velocity = new Vector2(0f, -0.01f);
        }

        player.actionLimite = false;
    }

    private Skill UseHowling()
    {
        player.animator.SetTrigger("Skill");
        Howling howling = GameManager.Resource.Instantiate<Howling>
            ("Prefab/Player/Skill/Howling", player.transform.position, gameObject);

        return howling;
    }

    private Skill UseShotSoul()
    {
        player.animator.SetTrigger("Skill");
        ShotSoul shotSoul = GameManager.Resource.Instantiate<ShotSoul>
            ("Prefab/Player/Skill/ShotSoul", player.transform.position);

        return shotSoul;
    }

    private Skill UseDive()
    {
        player.animator.SetTrigger("Skill");
        Dive dive = GameManager.Resource.Instantiate<Dive>
            ("Prefab/Player/Skill/Dive", new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), gameObject);

        return dive;
    }
}
