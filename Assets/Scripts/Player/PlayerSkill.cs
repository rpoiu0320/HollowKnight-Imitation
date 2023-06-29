using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private Animator skillAnimator;
    private PlayerMover playerMover;

    private void Awake()
    {
        playerMover = playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        skillAnimator = GetComponent<Animator>();
    }

    public void OnHowling()
    {
        Howling howling = GameManager.Resource.Instantiate<Howling>("Prefab/Player/Skill/Howling", playerMover.transform.position, GameObject.Find("PoolManager").transform);
    }

    public void OnShotSoul()
    {
        ShotSoul shotSoul = GameManager.Resource.Instantiate<ShotSoul>("Prefab/Player/Skill/ShotSoul", playerMover.transform.position, GameObject.Find("PoolManager").transform);
    }

    public void OnDive()
    {
        Dive dive = GameManager.Resource.Instantiate<Dive>("Prefab/Player/Skill/Dive", playerMover.transform.position, GameObject.Find("PoolManager").transform);
    }

    public void OnChargeSkill()
    {
        Debug.Log("UseChargeSkill");
    }

    

   
}
